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
        [Authorize]
        public ActionResult Index()
        {
            RenderNavbar("Home");
            return View();
        }

        public ActionResult Intro()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RenderNavbar(string activePage)
        {
            RenderPatientMenu menu = new RenderPatientMenu();
            ViewBag.menu = menu.RenderMenu(activePage);
            return PartialView("_MenuView");
        }
    }
}