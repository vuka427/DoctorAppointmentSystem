﻿using DoctorAppointmentSystem.Models;
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
using DoctorAppointmentSystem.Models.Account.Profile;

namespace DoctorAppointmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly RegisterIO registerIO;
        private readonly LoginIO loginIO;
        private readonly ProfileIO profileIO;

        public AccountController()
        {
            this.registerIO = new RegisterIO();
            this.loginIO = new LoginIO();
            this.profileIO = new ProfileIO();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            USER user = loginIO.GetUser(model.Username);
            if (user != null && PasswordHelper.VerifyPassword(model.Password, user.PASSWORDHASH))
            {
                if (user.STATUS)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    string action = "";
                    string controller = "";
                    loginIO.UserRedirects(user, out action, out controller);
                    return Json(new { success = true, message = "", url = "/" + controller + "/" + action });
                }
                else
                {
                    return Json(new { success = false, message = "Account have been locked!" });
                }
            }
            return Json(new { success = false, message = "Username or password is incorrect!" });
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


        // Register without profile picture
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
            return Json(new { success = true, message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFiles(string username)
        {
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
                        fileName = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                        file.SaveAs(fileName);
                    }

                    return Json(new { success = true, message = " Good Job!"});
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

        public ActionResult EditProfile()
        {
            return View();
        }

        public ActionResult EditProfileAPI()
        {
            ProfileViewModel profile = profileIO.GetProfile(User.Identity.Name);
            return Json(new { data = profile }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Intro", "Home");
        }
    }
}