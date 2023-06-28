﻿using DoctorAppointmentSystem.Areas.Doctor.Models.Registration;
using DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    [Authorize]
    public class RegistrationController : Controller
    {
        private readonly RegistrationIO registrationIO;

        public RegistrationController()
        {
            registrationIO = new RegistrationIO();
        }

        // GET: Doctor/Registration
        public ActionResult Index()
        {
            var consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.consultantType = new SelectList(consultantType, "id", "paraval");
            var modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.modeOfConsultant = new SelectList(modeOfConsultant, "id", "paraval");
            ViewBag.menu = RenderMenu.RenderDoctorMenu("Registration");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            var genders = SystemParaHelper.GenerateGender();
            ViewBag.genders = new SelectList(genders, "id", "paraval");
            return View();
        }

        public ActionResult SearchMembers(string nationalID, string name)
        {
            try
            {
                var data = registrationIO.SearchMembers(nationalID, name);
                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(SearchMembers);
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { success = false, message = "No matches found." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CreateNewMember(CreateMemberViewModel member)
        {
            string message;
            bool success = registrationIO.CreateNewMember(member, out message);
            return Json(new { success = success, message=message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MakeAppointment(CreateMemberViewModel member)
        {
            bool success = true;
            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }
    }
}