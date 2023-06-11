using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.Appointment;
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
        // GET: Appointment
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

        public ActionResult GetData()
        {
            var doctorList = new AppointmentIO().GetData();
            return Json(new { data = doctorList }, JsonRequestBehavior.AllowGet);
        }
    }
}