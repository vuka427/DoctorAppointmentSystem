using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    public class ConfirmedApptController : Controller
    {
        // GET: Doctor/ConfirmedAppt
        public ActionResult Index()
        {
            return View();
        }
    }
}