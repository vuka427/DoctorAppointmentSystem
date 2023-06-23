using DoctorAppointmentSystem.Areas.Doctor.Models.ComfirmedAppt;
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
    public class ConfirmedApptController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.menu = RenderMenu.RenderDoctorMenu("Appointments Confirmed");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public ActionResult LoadData(string status)
        {
            string username = User.Identity.Name;
            try
            {
                int doctorID = GetInfo.GetDoctorID(username);

                if(doctorID == 0)
                {
                    throw new Exception("Doctor not found.");
                }

                List<AppointmentViewModel> data = new ConfirmedApptIO().GetConfirmedAppts(doctorID, status);

                return Json(new { success = true, data = data}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string sEventCatg = GetInfo.GetUserType(username).ToUpper();
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "LoadData";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MemberDetails(int id)
        {
            MemberDetailViewModel data = new ConfirmedApptIO().GetMemberDetails(id);

            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.menu = RenderMenu.RenderDoctorMenu("Appointments Confirmed");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View(data);
        }


    }
}