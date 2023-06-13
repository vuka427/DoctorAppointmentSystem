using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.Appointment;
using DoctorAppointmentSystem.Models.Appointment.MakeAppointment;
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
            ViewBag.menu = RenderMenu.RenderPatientMenu("Make Appointment");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public ActionResult MakeAppointment()
        {
            ViewBag.menu = RenderMenu.RenderPatientMenu("Make Appointment");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public ActionResult History()
        {
            ViewBag.menu = RenderMenu.RenderPatientMenu("Appointment History");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public ActionResult LoadSchedules()
        {
            var schedules = appointmentIO.LoadSchedules();
            return Json(new { data = schedules }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadAppointment(string selectedDoctorID, string selectedScheduleID)
        {
            AppointmentViewModel data = appointmentIO.LoadAppointment(Convert.ToInt32(selectedDoctorID), User.Identity.Name, Convert.ToInt32(selectedScheduleID));
            return Json(new { data = data}, JsonRequestBehavior.AllowGet);
        }
    }
}