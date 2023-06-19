using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.Appointment;
using DoctorAppointmentSystem.Models.Appointment.MakeAppointment;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly AppointmentIO appointmentIO;
        public AppointmentController()
        {
            appointmentIO = new AppointmentIO();
        }

        public ActionResult Index()
        {
            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.menu = RenderMenu.RenderPatientMenu("Schedule of Doctors");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public ActionResult MakeAppointment()
        {
            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.availableDoctors = appointmentIO.GetAvailableDoctors();
            ViewBag.menu = RenderMenu.RenderPatientMenu("Make Appointment");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public ActionResult History()
        {
            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.menu = RenderMenu.RenderPatientMenu("Appointment History");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public ActionResult ViewHistory()
        {
            var data = appointmentIO.GetAllAppointment(User.Identity.Name);
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }


        // // Get a list of doctors with corresponding work schedules
        public ActionResult GetDoctors(DateTime dateOfConsultation, TimeSpan time)
        {
            List<DoctorViewModel> doctors = appointmentIO.GetDoctors(dateOfConsultation, time);

            return Json(new { data = doctors }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadAppointment(string selectedDoctorID, string selectedScheduleID)
        {
            AppointmentViewModel data = appointmentIO.LoadAppointment(Convert.ToInt32(selectedDoctorID), User.Identity.Name, Convert.ToInt32(selectedScheduleID));
            return Json(new { data = data}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewAppointment(int appointmentID)
        {
            AppointmentViewModel data =  appointmentIO.ViewAppointment(appointmentID);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult MakeAppointmentAPI(AppointmentViewModel data)
        {
            bool success = appointmentIO.MakeAppointment(data, User.Identity.Name);
            if (success)
            {
                return Json(new { success = true, message = "Appointmented successfully! Please wait for confirmation from the doctor." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "You have an upcoming appointment. Please check your schedule." }, JsonRequestBehavior.AllowGet);
        }
    }
}