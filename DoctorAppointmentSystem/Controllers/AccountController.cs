using DoctorAppointmentSystem.Models;
using DoctorAppointmentSystem.Models.Account;
using DoctorAppointmentSystem.Models.Account.Login;
using DoctorAppointmentSystem.Models.Account.Register;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DoctorAppointmentSystem.HelperClasses;
using System.Web.Security;
using System.IO;
using System.Threading.Tasks;

namespace DoctorAppointmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly RegisterIO registerIO;
        private readonly LoginIO loginIO;

        public AccountController()
        {
            this.registerIO = new RegisterIO();
            this.loginIO = new LoginIO();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            USER user = loginIO.GetUser(model.Username);
            if (user != null && PasswordHelper.VerifyPassword(model.Password, user.PASSWORDHASH))
            {
                if (user.STATUS)
                {
                    if (user.LASTLOGIN != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Username, false);
                        string action = "";
                        string controller = "";
                        loginIO.UserRedirects(user, out action, out controller);
                        return Json(new { success = true, message = "", url = "/" + controller + "/" + action });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Please check your email to activate your account before logging in." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Account have been locked." });
                }
            }
            return Json(new { success = false, message = "Username or password is incorrect." });
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            var questions = SystemParaHelper.GenerateAuthQuestion();
            ViewBag.questions = new SelectList(questions, "id", "paraval");
            var genders = SystemParaHelper.GenerateGender();
            ViewBag.genders = new SelectList(genders, "id", "paraval");
            return View();
        }


        [HttpPost]
        public ActionResult RegisterAPI(UserViewModel user, PatientViewModel patient)
        {
            string message;
            if (!registerIO.VerifyUserInfo(user, out message))
            {
                return Json(new { success = false, message }, JsonRequestBehavior.AllowGet);
            }
            if (!registerIO.VerifyPatientInfo(patient, out message))
            {
                return Json(new { success = false, message }, JsonRequestBehavior.AllowGet);
            }

            registerIO.CreateNewAccount(user, patient, out message);

            // Send activation mail asynchronously
            string activationToken = EmailSender.GenerateActivationToken(user.username);
            string activationLink = EmailSender.GenerateActivationLink(activationToken);

            EmailSender emailSender = new EmailSender();
            _ = Task.Run(() => emailSender.SendActivationEmailAsync(user.email, activationLink));

            return Json(new { success = true, message }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult UploadFiles(string username)
        {
            username = String.IsNullOrEmpty(username.Trim()) ? User.Identity.Name : username;
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string imgFormat = file.FileName.Substring(file.FileName.LastIndexOf("."));
                        string fileName = username + "_avatar" + imgFormat;
                        using (DBContext dbContext = new DBContext())
                        {
                            if (!String.IsNullOrEmpty(User.Identity.Name))
                            {
                                USER user = dbContext.USER.Where(u => u.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
                                if (user != null)
                                {
                                    user.AVATARURL = fileName;
                                    dbContext.SaveChanges();
                                }
                            }
                        }
                        fileName = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                        file.SaveAs(fileName);
                    }

                    return Json(new { success = true, message = " Good Job." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = true, message = "Error occurred. Error details: " + ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = " No files selected." });
            }
        }

        public ActionResult Activate(string token, string expires)
        {
            string username = "";
            try
            {
                if (token == null)
                {
                    ViewBag.message = "Token not found.";
                    throw new ArgumentNullException();
                }
                else if(registerIO.IsValidToken(token, expires, ref username))
                {
                    registerIO.ActivateAccount(username);
                    ViewBag.message = "Congratulations! Your account has been successfully activated.";
                }
                else
                {
                    throw new Exception("Account activation failed! Incorrect or expired link. Please check your email again.");
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "Activate";
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Intro", "Home");
        }
    }
}