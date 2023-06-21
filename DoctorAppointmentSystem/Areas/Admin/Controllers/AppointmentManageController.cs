﻿using DoctorAppointmentSystem.Areas.Admin.Models;
using DoctorAppointmentSystem.Areas.Admin.Models.AdminUser;
using DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage;
using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.Validation;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    public class AppointmentManageController : Controller
    {

        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;
        private readonly IloggerService _logger;
        private readonly AppointmentIO _appointment;

        public AppointmentManageController(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper, IloggerService logger, AppointmentIO appointment)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
            _logger = logger;
            _appointment = appointment;
        }




        // GET: Admin/Appointment
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Appointments");
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            var user = GetCurrentUser();
            ViewBag.Name = user != null ? user.USERNAME : "";

            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");

            return View();
        }

        public JsonResult LoadAppointmentData(JqueryDatatableParam param) {

            var apmts = _appointment.GetAllAppointment();
            IEnumerable<ApponitmentViewModel> Apointments;
            try
            {


                Apointments = apmts.Select(dt => _mapper.GetMapper().Map<APPOINTMENT, ApponitmentViewModel>(dt)).ToList();
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to mapping APPOINTMENT to ApponitmentViewModel";
                string sEventSrc = nameof(LoadAppointmentData);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                Apointments = new List<ApponitmentViewModel>();
            }

            if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Apointments = Apointments.Where(x => x.PATIENID.ToString().ToLower().Contains(param.sSearch.ToLower())
                                              || x.PATIENTNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORID.ToString().ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORNAME.ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
            }
            var sortColumnIndex = param.iSortCol_0;
            var sortDirection = param.sSortDir_0; 

            if (sortColumnIndex == 1)
            {
                Apointments = (sortDirection == "asc" ? Apointments.OrderBy(c => c.APPOINTMENTID) : Apointments.OrderByDescending(c => c.APPOINTMENTID));
            }
            else if(sortColumnIndex == 2)
            {
                Apointments = (sortDirection == "asc" ? Apointments.OrderBy(c => c.PATIENID) : Apointments.OrderByDescending(c => c.PATIENID));
            }
            else if (sortColumnIndex == 4)
            {
                Apointments = (sortDirection == "asc" ? Apointments.OrderBy(c => c.DOCTORID) : Apointments.OrderByDescending(c => c.DOCTORID));
            }
            else if (sortColumnIndex == 13)
            {
                Apointments = sortDirection == "asc" ? Apointments.OrderBy(c => c.CREATEDDATE) : Apointments.OrderByDescending(c => c.CREATEDDATE);
            }
            else if (sortColumnIndex == 15)
            {
                Apointments = sortDirection == "asc" ? Apointments.OrderBy(c => c.UPDATEDDATE) : Apointments.OrderByDescending(c => c.CREATEDDATE);
            }else
            {
                Func<ApponitmentViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.PATIENTNAME :
                                                           sortColumnIndex == 3 ? e.DOCTORNAME :
                                                           sortColumnIndex == 6 ? e.APPOINTMENTDATE :
                                                           sortColumnIndex == 7 ? e.APPOINTMENTTIME :
                                                           sortColumnIndex == 8 ? e.APPOINTMENTDAY :
                                                           sortColumnIndex == 9 ? e.APPOIMENTSTATUS:
                                                           sortColumnIndex == 10 ? e.CONSULTANTTIME :
                                                           sortColumnIndex == 12 ? e.CREATEDBY :
                                                           e.UPDATEDBY
                                                           ;

                Apointments = (sortDirection == "asc" ? Apointments.OrderBy(orderingFunction) : Apointments.OrderByDescending(orderingFunction));

            }

            var displayResult = Apointments.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = Apointments.Count();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult DeleteAppointment(int AppointmentId)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return Json(new {error =1, msg =" " });
            }
            ValidationResult result = _appointment.DeleteAppointment(AppointmentId,user.USERNAME);
            if (result.Success)
            {
                return Json(new { error = 0, msg = "ok" });
            }
            else
            {
                return Json(new {error =1 , msg = result.ErrorMessage});
            }
        }

        public JsonResult AppointViewDetail(int appointmentID)
        {
            var apm = _appointment.getAppointmentInfo(appointmentID, GetCurrentUser().USERNAME);

            return Json(new { error = 1, msg = "ok", data= apm }, JsonRequestBehavior.AllowGet);
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