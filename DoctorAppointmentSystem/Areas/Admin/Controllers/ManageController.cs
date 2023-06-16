using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    [AppAuthorize("Admin")]
    public class ManageController : Controller
    {
        private readonly DBContext _dbContext;

        public ManageController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Admin/Manage
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Home");

            ViewBag.DoctorCount = _dbContext.DOCTOR.Count(d => d.DELETEDFLAG == false);
            ViewBag.PatientCount = _dbContext.PATIENT.Count(d => d.DELETEDFLAG == false);
            
            return View();
        }

        
    }
}