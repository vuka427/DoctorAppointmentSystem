﻿using DoctorAppointmentSystem.Areas.Admin.Models;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
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
        private readonly DashboardIO _dashboardIO;

        public ManageController(DBContext dbContext, DashboardIO dashboardIO)
        {
            _dbContext = dbContext;
            _dashboardIO = dashboardIO;
        }



        // GET: Admin/Manage
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Home");
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            var user = GetCurrentUser();
            ViewBag.Name = user != null ? user.USERNAME : "";

            ViewBag.DoctorCount = _dashboardIO.CountAllDoctor();
            ViewBag.PatientCount = _dashboardIO.CountAllPatient();

            return View();
        }
        [HttpGet]
        public JsonResult GetChartApm()
        {
            int[] apmData = _dashboardIO.GetArrayAppointmentOnWeek();

            return Json(new { error = 0, datachart = apmData }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private USER GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var userName = User.Identity.Name;
                if (userName != null)
                {
                    var currentUser = _dbContext.USER.Where(u => u.USERNAME == userName && u.DELETEDFLAG == false).FirstOrDefault();
                    return currentUser;
                }
            }

            return null;
        }

    }
}