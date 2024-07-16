using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using LOGISTIK.Helper;
using LOGISTIK.Data;
using LOGISTIK.Models;
using System.Collections.Generic;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using LOGISTIK.Security;
using System.Net.Mail;
using System.Net;
using MailMessage = System.Net.Mail.MailMessage;

namespace LOGISTIK.Controllers
{
    public class LOGISTIKController : Controller
    {
        private LOGISTIKContext db = new LOGISTIKContext();

        /*====================================
         ||GET: INDEX FOR SHOW DATA IN TABLE||
         ===================================== 
      */
        [HttpGet]
        public ActionResult Index(DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Set nilai default pada parameter fromDate dan toDate
            if (!fromDate.HasValue || !toDate.HasValue)
            {
                var today = DateTime.Now.Date;
                var twoMonthsAgo = today.AddMonths(-2);
                fromDate = new DateTime(twoMonthsAgo.Year, twoMonthsAgo.Month, 1);
                toDate = today;
            }

            var user = db.Users.ToList();
            var cargos = db.Cargos.ToList();
            var requestors = db.Requestors.OrderByDescending(d => d.Date).ToList();
            var equipment = db.Equipments.ToList();
            var dimensions = db.Dimensions.ToList();

            //filtering date
            if (fromDate.HasValue && toDate.HasValue)
            {
                DateTime? fromDateNullable = fromDate.HasValue ? fromDate.Value.Date : (DateTime?)null;
                DateTime? toDateNullable = toDate.HasValue ? toDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59) : (DateTime?)null;

                var filteringRequestor = requestors.Join(equipment, r => r.Id, e => e.Id, (r, e) => new { Requestor = r, Equipment = e })
                    .Where(x => x.Requestor.Date > fromDateNullable && x.Requestor.Date < toDateNullable)
                    .Select(x => x.Requestor)
                    .ToList();

                requestors = filteringRequestor;
            }

            var shipment = new CombineViewModel
            {
                ListUser = user,
                ListCargo = cargos,
                ListDimension = dimensions,
                ListRequstor = requestors,
                ListEquipment = equipment,

                UserModel = new Models.User(),
                CargoModel = new Models.Cargo(),
                DimensionModel = new Models.Dimension(),
                RequestorModel = new Models.Requestor(),
                EquipmentModel = new Models.Equipment()
            };

            // Set nilai default pada ViewBag untuk tampilan input field tanggal
            ViewBag.ToDate = toDate.Value.ToString("yyyy-MM-dd");
            ViewBag.FromDate = fromDate.Value.ToString("yyyy-MM-dd");

            return View(shipment);
        }

        /*====================================
          ||GET: CREATE DATA TABLE REQUESTOR||
          ==================================== 
       */
        [HttpGet]
        public ActionResult Create()
        {
            var data = new CombineViewModel();
            // Inisialisasi model-model di dalam CombineViewModel
            data.CargoModel = new Cargo();
            data.RequestorModel = new Requestor();
            data.EquipmentModel = new Equipment();
            data.DimensionModel = new Dimension();

            // Isi daftar data dari database ke properti CombineViewModel
            data.ListCargo = db.Cargos.ToList();
            data.ListDimension = db.Dimensions.ToList();
            data.ListEquipment = db.Equipments.ToList();
            data.ListRequstor = db.Requestors.ToList();

            return View(data);
        }


        /*=====================================
          ||POST: CREATE DATA TABLE REQUESTOR||
          ===================================== 
       */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CombineViewModel data, HttpPostedFileBase file)
        {
            int maxFileSizeInBytes = 5 * 1024 * 1024;
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        if (file.ContentLength <= maxFileSizeInBytes)
                        {
                            data.EquipmentModel.Invoice = file.FileName;
                            data.EquipmentModel.InvoiceFileData = fileData;
                        }
                        else
                        {
                            ModelState.AddModelError("File", "File yang diunggah terlalu besar. Harap unggah file yang lebih kecil.");
                        }
                    }

                    //NOTICATION TO EMAIL ADDRESS
                    string emailTemplate = RenderPartialToString("EmailTamplate", data);
                    string Date = data.RequestorModel.DateRequest;
                    string Name = data.RequestorModel.Name;
                    SendEmailToAdmin(data, emailTemplate, Name, data.RequestorModel.DateRequest);

                    //CURRENT DATE REQUEST
                    DateTime currentDate = DateTime.Now;
                    string formattedDate = currentDate.ToString("dd MMMM yyyy");
                    data.RequestorModel.DateRequest = formattedDate;
                    data.RequestorModel.Date = DateTime.Now;

                    db.Cargos.Add(data.CargoModel);
                    db.Requestors.Add(data.RequestorModel);
                    db.Equipments.Add(data.EquipmentModel);
                    db.Dimensions.Add(data.DimensionModel);
                    data.EquipmentModel.Status = "Pending";

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Terjadi kesalahan saat menyimpan data: " + ex.Message);
                }
            }

            // Retrieve the lists after the database changes
            data.ListCargo = db.Cargos.ToList();
            data.ListRequstor = db.Requestors.ToList();
            data.ListDimension = db.Dimensions.ToList();
            data.ListEquipment = db.Equipments.ToList();

            return View("Create", data);
        }

        /*============================================
          ||RENDER VIEW UNTUK MANGGIL TAMPLATE EMAIL||
          ============================================ 
       */
        public string RenderPartialToString(string viewName, object model)
        {
            var controller = this;
            controller.ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        /*===========================================
          ||GET:  EDIT FOR UPDATE DATA IN DATABASE ||
          ===========================================
       */
        [HttpGet]
        [AuthorizationHandlerAttribute(Roles = "1,2")]
        public ActionResult Edit(int id)
        {
            var cargoData = db.Cargos.FirstOrDefault(c => c.Id == id); 
            var requestorData = db.Requestors.FirstOrDefault(r => r.Id == id);
            var equipmentData = db.Equipments.FirstOrDefault(e => e.Id == id);
            var dimensionData = db.Dimensions.FirstOrDefault(d => d.Id == id);
            var userData = db.Emails.ToList();

            // Pastikan data yang akan diedit ditemukan
            if (cargoData != null && requestorData != null && equipmentData != null && dimensionData != null && userData != null)
            {
                var shipment = new CombineViewModel
                {
                    ListEmail = userData,
                    CargoModel = cargoData,
                    RequestorModel = requestorData,
                    EquipmentModel = equipmentData,
                    DimensionModel = dimensionData
                };
                return View(shipment);
            }
            else
            {
                return HttpNotFound();
            }
        }
        /*============================================
          ||POST:  EDIT FOR UPDATE DATA IN DATABASE ||
          ============================================
       */
        [HttpPost]
        [AuthorizationHandlerAttribute(Roles = "1,2")]
        public ActionResult Edit(CombineViewModel model, string[] selectedEmails)
        {
            try
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine("Error: " + error.ErrorMessage);
                    }
                }

                if (ModelState.IsValid)
                {
                    // Ambil data yang akan diedit dari basis data berdasarkan ID
                    var cargoData = db.Cargos.FirstOrDefault(c => c.Id == model.CargoModel.Id);
                    var requestorData = db.Requestors.FirstOrDefault(r => r.Id == model.RequestorModel.Id);
                    var equipmentData = db.Equipments.FirstOrDefault(e => e.Id == model.EquipmentModel.Id);
                    var dimensionData = db.Dimensions.FirstOrDefault(d => d.Id == model.DimensionModel.Id);
                    var selectedEmailList = new List<string>();
                    string selectEmailsString = "";

                    if (selectedEmails != null)
                    {
                        selectedEmailList.AddRange(selectedEmails);
                        selectEmailsString = string.Join(",", selectedEmailList);
                    }

                    string emailTemplate = RenderPartialToString("EmailTamplate", model);
                    string Name = model.EquipmentModel.Company;
                    string Date = model.RequestorModel.DateRequest;

                    SendMail mailSender = new SendMail();
                    mailSender.SendMailToSuperior(emailTemplate, model.EquipmentModel.Company, model.RequestorModel.DateRequest, selectEmailsString);

                    if (cargoData != null && requestorData != null && equipmentData != null && dimensionData != null)
                    {
                        // Update properti yang sesuai dengan data yang diterima dari model

                        //equipment
                        equipmentData.Status = model.EquipmentModel.Status;
                        equipmentData.Confirm_Date = model.EquipmentModel.Confirm_Date;
                        equipmentData.Mode = model.EquipmentModel.Mode;
                        equipmentData.Po_Machine = model.EquipmentModel.Po_Machine;
                        equipmentData.Equipment_Type = model.EquipmentModel.Equipment_Type;
                        equipmentData.QTY = model.EquipmentModel.QTY;
                        equipmentData.Satuan_QTY = model.EquipmentModel.Satuan_QTY;
                        equipmentData.Equipment_Gross = model.EquipmentModel.Equipment_Gross;
                        equipmentData.Contact_Person = model.EquipmentModel.Contact_Person;
                        equipmentData.Phone = model.EquipmentModel.Phone;
                        equipmentData.Company = model.EquipmentModel.Company;
                        equipmentData.Address = model.EquipmentModel.Address;
                        equipmentData.Post_Code = model.EquipmentModel.Post_Code;
                        equipmentData.Country = model.EquipmentModel.Country;
                        equipmentData.Email_Address = model.EquipmentModel.Email_Address;
                        equipmentData.Pickup_Time = model.EquipmentModel.Pickup_Time;
                        equipmentData.Location = model.EquipmentModel.Location;

                        //requestor
                        requestorData.Name = model.RequestorModel.Name;
                        requestorData.BadgeId = model.RequestorModel.BadgeId;
                        requestorData.Email = model.RequestorModel.Email;
                        requestorData.Department = model.RequestorModel.Department;
                        requestorData.Superior = model.RequestorModel.Superior;

                        //cargo
                        cargoData.Q1 = model.CargoModel.Q1;
                        cargoData.Q2 = model.CargoModel.Q2;
                        cargoData.Q3 = model.CargoModel.Q3;
                        cargoData.Q4 = model.CargoModel.Q4;
                        cargoData.Q5 = model.CargoModel.Q5;
                        cargoData.Q6 = model.CargoModel.Q6;
                        cargoData.Q7 = model.CargoModel.Q7;
                        cargoData.Q8 = model.CargoModel.Q8;
                        cargoData.Q9 = model.CargoModel.Q9;
                        cargoData.Q10 = model.CargoModel.Q10;
                        cargoData.Q11 = model.CargoModel.Q11;
                        cargoData.Q12 = model.CargoModel.Q12;
                        cargoData.Q13 = model.CargoModel.Q13;
                        cargoData.Q15 = model.CargoModel.Q15;

                        //Dimension
                        dimensionData.Length = model.DimensionModel.Length;
                        dimensionData.Weight = model.DimensionModel.Weight;
                        dimensionData.Height = model.DimensionModel.Height;
                        dimensionData.Length_2 = model.DimensionModel.Length_2;
                        dimensionData.Weight_2 = model.DimensionModel.Weight_2;
                        dimensionData.Height_2 = model.DimensionModel.Height_2;
                        dimensionData.Length_3 = model.DimensionModel.Length_3;
                        dimensionData.Weight_3 = model.DimensionModel.Weight_3;
                        dimensionData.Height_3 = model.DimensionModel.Height_3;
                        dimensionData.Length_4 = model.DimensionModel.Length_4;
                        dimensionData.Weight_4 = model.DimensionModel.Weight_4;
                        dimensionData.Height_4 = model.DimensionModel.Height_4;
                        dimensionData.Length_5 = model.DimensionModel.Length_5;
                        dimensionData.Weight_5 = model.DimensionModel.Weight_5;
                        dimensionData.Height_5 = model.DimensionModel.Height_5;
                        dimensionData.Length_6 = model.DimensionModel.Length_6;
                        dimensionData.Weight_6 = model.DimensionModel.Weight_6;
                        dimensionData.Height_6 = model.DimensionModel.Height_6;
                        dimensionData.Length_7 = model.DimensionModel.Length_7;
                        dimensionData.Weight_7 = model.DimensionModel.Weight_7;
                        dimensionData.Height_7 = model.DimensionModel.Height_7;

                        db.SaveChanges();

                        return RedirectToAction("Index"); // Redirect ke halaman lain setelah penyimpanan berhasil
                    }
                    else
                    {
                        // Jika data yang akan diedit tidak ditemukan, kembalikan 404 Not Found
                        return HttpNotFound();
                    }
                }
                else
                {
                    // Jika ModelState tidak valid, kembalikan formulir edit dengan pesan kesalahan
                    return View(model);
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                while (innerException != null && !(innerException is SqlException))
                {
                    innerException = innerException.InnerException;
                }

                if (innerException is SqlException sqlException)
                {
                    // Handle kesalahan SQL di sini, misalnya, mencetak pesan kesalahan.
                    Console.WriteLine("SQL Error: " + sqlException.Message);
                }
                else
                {
                    // Handle kesalahan lain yang mungkin terjadi saat menyimpan perubahan.
                    Console.WriteLine("Error: " + ex.Message);
                }

                // Redirect atau tampilkan tampilan pesan kesalahan yang sesuai.
                return View("ErrorView");
            }

        }

        /*===============================
          ||POST: DELETE DATA REQUESTOR|| 
          ===============================
       */
        [HttpPost]
        [AuthorizationHandlerAttribute(Roles = "1,2")]
        public ActionResult Delete(int id)
        {
            try
            {   
                // Temukan data yang akan dihapus dari database
                var dataCargo = db.Cargos.Find(id);
                var dataRequestor = db.Requestors.Find(id);
                var dataDimension = db.Dimensions.Find(id);
                var dataEquipment = db.Equipments.Find(id);

                if (dataCargo != null)
                    db.Cargos.Remove(dataCargo);
                if (dataRequestor != null)
                    db.Requestors.Remove(dataRequestor);
                if (dataDimension != null)
                    db.Dimensions.Remove(dataDimension);
                if (dataEquipment != null)
                    db.Equipments.Remove(dataEquipment);

                db.SaveChanges();
                return RedirectToAction("Index"); // Redirect kembali ke tampilan Index setelah berhasil menghapus data
            }
            catch (Exception)
            {
                // Handle Error if this failed to deleted
                return RedirectToAction("Index"); // Anda dapat memilih tindakan yang sesuai jika terjadi kesalahan
            }
        }

        /*===================================================
          ||CONVERT DATA TABLES TO PDF, CSV AND EXCEL FILES||
          =================================================== 
       */
        public ActionResult TabelConvert(DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Set nilai default pada parameter fromDate dan toDate
            if (!fromDate.HasValue || !toDate.HasValue)
            {
                var today = DateTime.Now.Date;
                var twoMonthsAgo = today.AddMonths(0);
                fromDate = new DateTime(twoMonthsAgo.Year, twoMonthsAgo.Month, 1);
                toDate = today;
            }

            var user = db.Users.ToList();
            var cargos = db.Cargos.ToList();
            var equipment = db.Equipments.ToList();
            var requestors = db.Requestors.ToList();
            var dimensions = db.Dimensions.ToList();

            //filtering date
            if (fromDate.HasValue && toDate.HasValue)
            {
                DateTime? fromDateNullable = fromDate.HasValue ? fromDate.Value.Date : (DateTime?)null;
                DateTime? toDateNullable = toDate.HasValue ? toDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59) : (DateTime?)null;

                var filteringRequestor = requestors.Join(equipment, r => r.Id, e => e.Id, (r, e) => new { Requestor = r, Equipment = e })
                    .Where(x => x.Requestor.Date > fromDateNullable && x.Requestor.Date < toDateNullable)
                    .Select(x => x.Requestor)
                    .ToList();

                requestors = filteringRequestor;
            }

            var shipment = new CombineViewModel
            {
                ListUser = user,
                ListCargo = cargos,
                ListDimension = dimensions,
                ListRequstor = requestors,
                ListEquipment = equipment,

                UserModel = new Models.User(),
                CargoModel = new Models.Cargo(),
                DimensionModel = new Models.Dimension(),
                RequestorModel = new Models.Requestor(),
                EquipmentModel = new Models.Equipment()
            };

            // Set nilai default pada ViewBag untuk tampilan input field tanggal
            ViewBag.FromDate = fromDate.Value.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate.Value.ToString("yyyy-MM-dd");

            return View(shipment);
        }

        /*======================================
          ||BUTTON SUBMIT FOR STATUS REQUESTOR||
          ====================================== 
       */
        [AuthorizationHandlerAttribute(Roles = "1,2")]
        public ActionResult Accept(int id)
        {
            var data = db.Equipments.Find(id);
            if (data != null)
            {
                data.Status = "Complete";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        /*========================================
          ||CONTROLLER FOR DOWNLOAD FILE INVOICE||
          ========================================
       */
        public ActionResult Download(int id)
        {
            var fileModel = db.Equipments.Find(id);
            if (fileModel == null)
            {
                return HttpNotFound();
            }
            return File(fileModel.InvoiceFileData, "application/octet-stream", fileModel.Invoice);
        }

        /*================================================================
          ||TAKE THE USERNAME FROM DATABASE AT TABEL DATA WINDOWSACCOUNT||
          ================================================================
       */
        public User GetCurrentUser()
        {
            var username = HttpContext.User.Identity.Name;

            return db.Users.FirstOrDefault(x => x.WindowsAccount.ToLower() == username.ToLower());
        }

        /*==============================================
          ||GIVE SIDE BAR USER ACCORDANCE TO ROLE USER||
          ==============================================
       */
        public ActionResult NavBar()
        {
            var currentUser = GetCurrentUser()?.Role;

            if (currentUser == null)
            {
                ViewBag.role = 0;
            }
            else
            {
                ViewBag.role = Int32.Parse(currentUser);
            }

            return PartialView();
        }

        /*======================================================
          ||NOTIFICATION EMAIL TO ADMIN FROM REQUESTOR        ||
          ||"If the requestor places an order, a notification ||
          ||requesting further processing will appear to the  ||
          ||admin in the form of an email"                    ||
          ======================================================
        */
        private void SendEmailToAdmin(CombineViewModel data, string emailTamplate, string Name, string Date)
        {
            // Create a new email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("BTHGreenlineR@infineon.com");
            message.To.Add(new MailAddress("Muhammad.Ilham@infineon.com"));
            //message.To.Add(new MailAddress("Panjaitan.Hiras@infineon.com"));
            //message.To.Add(new MailAddress("Wiwiek.Purwanti@infineon.com"));
            //message.To.Add(new MailAddress("Evita.Ika@infineon.com"));
            //message.To.Add(new MailAddress("Jubasril.Jubasril@infineon.com"));
            message.Subject = "SIMULATION || FW: New Logistic Shipment Request From, " + Name;
            message.Body = emailTamplate;
            message.IsBodyHtml = true;

            // Send the email
            SmtpClient smtpClient = new SmtpClient("mailrelay-internal.infineon.com");
            smtpClient.Credentials = new NetworkCredential("OJT TEAM", "1234");
            smtpClient.Send(message);
        }
        public ActionResult EmailTemplate()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}