using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using LOGISTIK.Data;
using LOGISTIK.Models;
using LOGISTIK.Security;

namespace LOGISTIK.Controllers
{
    [AuthorizationHandler(Roles = "1,2")]
    public class UserController : Controller
    {
        private LOGISTIKContext db = new LOGISTIKContext();

        /*=============================
          ||GET: GET DATA REQUESTOR  ||
          =============================
       */
        public ActionResult Index()
        {
            var user = db.Users.ToList();
            var email = db.Emails.ToList();
            var data = new CombineViewModel
            {
                ListUser = user,
                ListEmail = email,
                UserModel = new Models.User(),
                EmailModel = new Models.Email()
            };
            return View(data);
        }

        /*==================================================
          ||GET: Edit Untuk Mengupdate Data Pada Database ||
          ==================================================
       */
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var userData = db.Users.FirstOrDefault(c => c.Id == id);

            if (userData != null)
            {
                var shipment = new CombineViewModel
                {
                    UserModel = userData,
                };
                return View(shipment);
            }
            else
            {
                return HttpNotFound();
            }
        }
        /*===================================================
          ||POST: Edit Untuk Mengupdate Data Pada Database ||
          ===================================================
       */
        [HttpPost]
        public ActionResult Edit(CombineViewModel model)
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
                    var UserData = db.Users.FirstOrDefault(c => c.Id == model.UserModel.Id);

                    if (UserData != null)
                    {
                        //USER EDIT
                        UserData.Name = model.UserModel.Name;
                        UserData.Email = model.UserModel.Email;
                        UserData.WindowsAccount = model.UserModel.WindowsAccount;
                        UserData.Role = model.UserModel.Role;
                        UserData.Type = model.UserModel.Type;
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

        /*==================================================
          ||GET: Create menyimpan data yang telah di buat ||
          ==================================================
       */
        [HttpGet]
        public ActionResult Create()
        {
            var data = new CombineViewModel();
            data.UserModel = new User();
            data.ListUser = db.Users.ToList();

            return View(data);
        }
        /*=================================
          ||POST: Create akun untuk user ||
          =================================
       */
        [HttpPost]
        public ActionResult Create(CombineViewModel model)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(model.UserModel);
                db.SaveChanges();

                TempData["alertMessage"] = "Create Account Succesfully";
                return RedirectToAction("Index");
            }
            return View("Create", model);
        }

        /*============================================================
          ||GET: CreateForwarder membuat data forwarder untuk email ||
          ============================================================
       */
        [HttpGet]
        public ActionResult CreateForwarder()
        {
            var data = new CombineViewModel();
            data.EmailModel = new Email();
            data.ListEmail = db.Emails.ToList();

            return View(data);
        }
        /*==============================================================
          ||POST: CreateForwarder membuat data forwarder untuk email  ||
          ==============================================================
       */
        [HttpPost]
        public ActionResult CreateForwarder(CombineViewModel data)
        {
            if(ModelState.IsValid)
            {
                db.Emails.Add(data.EmailModel);
                db.SaveChanges();

                TempData["alertMessage"] = "Create Account Succesfully";
                return RedirectToAction("index");
            }

            return View(data);
        }

        /*==================================================
          ||GET: Edit Untuk Mengupdate Data Pada Database ||
          ==================================================
       */
        [HttpGet]
        public ActionResult EditForwarder(int id)
        {
            var emailData = db.Emails.FirstOrDefault(c => c.Id == id);

            if (emailData != null)
            {
                var shipment = new CombineViewModel
                {
                    EmailModel = emailData,
                };
                return View(shipment);
            }
            else
            {
                return HttpNotFound();
            }
        }
        /*===================================================
          ||POST: Edit Untuk Mengupdate Data Pada Database ||
          ===================================================
       */
        [HttpPost]
        public ActionResult EditForwarder(CombineViewModel model)
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
                    var emailData = db.Emails.FirstOrDefault(c => c.Id == model.EmailModel.Id);

                    if (emailData != null)
                    {
                        //Forwarder EDIT
                        emailData.Name = model.EmailModel.Name;
                        emailData.EmailForwarder = model.EmailModel.EmailForwarder;
                         emailData.Type = model.EmailModel.Type;

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

        private void SendEmailToAdmin(CombineViewModel model, string emailTamplate, string Name)
        {
            // Create a new email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress("BTHGreenlineR@infineon.com");
            //message.To.Add(new MailAddress("Muhammad.Ilham@infineon.com"));
            //message.To.Add(new MailAddress("Panjaitan.Hiras@infineon.com"));
            //message.To.Add(new MailAddress("Wiwiek.Purwanti@infineon.com"));
            //message.To.Add(new MailAddress("Evita.Ika@infineon.com"));
            //message.To.Add(new MailAddress("Jubasril.Jubasril@infineon.com"));
            message.Subject = "Information add new user admin " + Name;
            message.Body = emailTamplate;
            message.IsBodyHtml = true;

            // Send the email
            SmtpClient smtpClient = new SmtpClient("mailrelay-internal.infineon.com");
            smtpClient.Credentials = new NetworkCredential("OJT TEAM", "1234");
            smtpClient.Send(message);
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

        /*===========================================
          ||POST: Delete untuk menghapus data user ||
          ===========================================
       */
        [AuthorizationHandler(Roles = "1")]
        public ActionResult Delete(int id)
        {
            try
            {
                // Temukan data yang akan dihapus dari database
                var dataUser = db.Users.Find(id);
                if (dataUser != null)
                    db.Users.Remove(dataUser);
                
                db.SaveChanges();
                return RedirectToAction("Index"); // Redirect kembali ke tampilan Index setelah berhasil menghapus data
            }
            catch (Exception)
            {
                // Handle Error if this failed to deleted
                return RedirectToAction("Index"); // Anda dapat memilih tindakan yang sesuai jika terjadi kesalahan
            }
        }

        /*=================================================
          ||POST: Delete untuk menghapus Email Forwarder ||
          =================================================
       */
        [AuthorizationHandler(Roles = "1")]
        public ActionResult DeleteForwarder(int id)
        {
            try
            {
                // Temukan data yang akan dihapus dari database
                var dataEmail = db.Emails.Find(id);
                if (dataEmail != null)
                    db.Emails.Remove(dataEmail);
                
                db.SaveChanges();
                return RedirectToAction("Index"); // Redirect kembali ke tampilan Index setelah berhasil menghapus data
            }
            catch (Exception)
            {
                // Handle Error if this failed to deleted
                return RedirectToAction("Index"); // Anda dapat memilih tindakan yang sesuai jika terjadi kesalahan
            }
        }
    }
}