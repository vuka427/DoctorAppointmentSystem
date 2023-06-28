﻿using DoctorAppointmentSystem.Areas.Doctor.Models.Appointments;
using DoctorAppointmentSystem.Areas.Doctor.Models.Home;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    [AppAuthorize("Doctor")]
    public class HomeController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly calendarIO _calendarIO;
        private readonly AppointmentsDoctorIO _appointments;

        public HomeController(DBContext dbContext, ISystemParamService sysParam, calendarIO calendarIO, AppointmentsDoctorIO appointments)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _calendarIO = calendarIO;
            _appointments = appointments;
        }



        // GET: Doctor/Home
        public ActionResult Index()
        {
            ViewBag.menu = RenderMenu.RenderDoctorMenu("Dashboard");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            ViewBag.daynow = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            


            return View();

        }


        

        [HttpGet]
        public JsonResult GetDoctorSchedule(DateTime start, DateTime end)
        {
            var currentUser = GetCurrentUser();
            int doctorId = currentUser.DOCTOR !=null? currentUser.DOCTOR.FirstOrDefault().DOCTORID : 0;

            var events = _calendarIO.GetDoctorSchedule(start, end, doctorId);

            return Json(events, JsonRequestBehavior.AllowGet);
        }




        [NonAction]
        private USER GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var userName = User.Identity.Name;
                if (userName != null)
                {
                    var currentUser = _dbContext.USER.Where(u => u.USERNAME == userName && u.DELETEDFLAG == false).Include("DOCTOR").FirstOrDefault();
                    return currentUser;
                }
            }
            return new USER();
        }
    }
}