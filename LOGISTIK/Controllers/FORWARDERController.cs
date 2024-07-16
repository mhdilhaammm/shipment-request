using System;
using System.IO;
using System.Linq;
using System.Web.Security;
using LOGISTIK.Data;
using LOGISTIK.Models;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using MailMessage = System.Net.Mail.MailMessage;

namespace LOGISTIK.Controllers
{
    public class FORWARDERController : Controller
    {
        private LOGISTIKContext db = new LOGISTIKContext();

        /*======================================
          ||GET: Index untuk menampilkan tabel||
          ====================================== 
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

            var cargos = db.Cargos.ToList();
            var requestors = db.Requestors.ToList();
            var equipment = db.Equipments.ToList();
            var dimensions = db.Dimensions.ToList();
            var user = db.Users.ToList();

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

        /*====================================
          ||POST: Index untuk Authentication||
          ||(pembagian halaman melalui form)||
          ====================================
        */
        [HttpPost]
        public ActionResult Index(CombineViewModel data)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(m => m.WindowsAccount == data.UserModel.WindowsAccount);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie( user.WindowsAccount, false);
                    if (user.Type == "LOGISTIK")
                    {
                        return RedirectToAction("Index", "LOGISTIK");
                    } 
                    else if (user.Type == "FORWARDER")
                    {
                        return RedirectToAction("Index", "FORWARDER");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Login failed. Please check your username and password.");
                }
            }
            return RedirectToAction("Index");
        }

        /*=======================================
          || GET: CREATE DATA FORWARDER        ||   
          || (form validasi untuk perequestan) ||
          ======================================= 
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

        /*=======================================
          || POST: CREATE DATA TABLE REQUESTOR ||
          ======================================= 
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

        /*=====================================================
          || POST: DETAILS DATA FORWARDER                    ||
          || (penampilan data untuk requestor dengan details)||
          ===================================================== 
        */
        [HttpGet]
        public ActionResult Details(int id, CombineViewModel objj)
        {
            var cargoData = db.Cargos.FirstOrDefault(c => c.Id == id); // Mengambil data Cargo sesuai dengan ID
            var requestorData = db.Requestors.FirstOrDefault(r => r.Id == id); // Mengambil data Requestor sesuai dengan ID
            var equipmentData = db.Equipments.FirstOrDefault(e => e.Id == id); // Mengambil data Equipment sesuai dengan ID
            var dimensionData = db.Dimensions.FirstOrDefault(d => d.Id == id); // Mengambil data Dimension sesuai dengan ID

            if (cargoData != null && requestorData != null && equipmentData != null && dimensionData != null)
            {
                var shipment = new CombineViewModel
                {
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

        /*===============================
          ||DASHBOARD FROM THIS WEBSITE||
          ===============================
       */
        public ActionResult Dashboard()
        {
            return View();
        }

        /*=============================================
          ||CONTROLLER UNTUK MENDOWNLOAD FILE INVOICE||
          ============================================= 
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

        public ActionResult AccessDenied()
        {
            return View();
        }

        /*=======================================
           ||TAKE NICKNAME FROM WINDOWS ACCOUNT||
          =======================================
        */
        public User GetCurrentUser()
        {
            var username = HttpContext.User.Identity.Name;

            return db.Users.FirstOrDefault(x => x.WindowsAccount.ToLower() == username.ToLower());
        }

        /*===================================
          ||TAKE THE SIDEBAR BASED ON ROLE ||
          ===================================
        */
        public ActionResult Sidebar()
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

        public ActionResult Breadcrumb()
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

    }
}