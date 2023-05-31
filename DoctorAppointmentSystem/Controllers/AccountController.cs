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
                if(user.STATUS)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Json(new { success = false, message = "Account have been locked!" });
                }
            }
            return Json(new { success = false, message = "Login failed! Username or password is incorrect!" });
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            var questions = registerIO.GenerateAuthQuestion();
            ViewBag.questions = new SelectList(questions, "id", "paraval");
            var genders = registerIO.GenerateGender();
            ViewBag.genders = new SelectList(genders, "id", "paraval");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAPI(UserViewModel user, PatientViewModel patient)
        {
            string message;
            if(!registerIO.VerifyUserInfo(user, out message))
            {
                return Json(new { success = false, message }, JsonRequestBehavior.AllowGet);
            }
            if (!registerIO.VerifyPatientInfo(patient, out message))
            {
                return Json(new { success = false, message }, JsonRequestBehavior.AllowGet);
            }

            registerIO.CreateNewAccount(user, patient, out message);
            return Json(new { success = true, message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}