using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    public class CancelledApptController : Controller
    {
        // GET: Doctor/CancelledAppt
        public ActionResult Index()
        {
            return View();
        }
    }
}