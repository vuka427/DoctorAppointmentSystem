using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Doctor/Profile
        public ActionResult Index()
        {
            return View();
        }
    }
}