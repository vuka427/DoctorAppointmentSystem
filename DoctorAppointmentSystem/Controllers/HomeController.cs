using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.menu = RenderMenu.RenderPatientMenu("Home");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public ActionResult Intro()
        {
            return View();
        }
    }
}