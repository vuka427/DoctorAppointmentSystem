using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    public class HomeController : Controller
    {
        // GET: Doctor/Home
        public ActionResult Index()
        {
            ViewBag.menu = RenderMenu.RenderDoctorMenu("Dashboard");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }
    }
}