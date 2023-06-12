using DoctorAppointmentSystem.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    public class ManageController : Controller
    {
        // GET: Admin/Manage
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Home");
            return View();
        }
    }
}