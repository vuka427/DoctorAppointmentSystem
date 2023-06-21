using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    public class AppointmentsController : Controller
    {
        // GET: Doctor/Appointments
        public ActionResult Index()
        {
            return View();
        }
    }
}