
using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Doctor.Models.Appointments;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly AppointmentsDoctorIO _appointments;
        private readonly IMapperService _mapper;

        public AppointmentsController(DBContext dbContext, AppointmentsDoctorIO appointments, IMapperService mapper)
        {
            _dbContext = dbContext;
            _appointments = appointments;
            _mapper = mapper;
        }



        // GET: Doctor/Appointments
        public ActionResult Index()
        {
            ViewBag.menu = RenderMenu.RenderDoctorMenu("Appointments");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public JsonResult LoadAppointmentData(JqueryDatatableParam param)
        {

            var apmts = _appointments.GetAllAppointment(GetCurrentUser().USERID);
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
            else if (sortColumnIndex == 2)
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
            }
            else
            {
                Func<ApponitmentViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.PATIENTNAME :
                                                           sortColumnIndex == 3 ? e.DOCTORNAME :
                                                           sortColumnIndex == 6 ? e.APPOINTMENTDATE :
                                                           sortColumnIndex == 7 ? e.APPOINTMENTTIME :
                                                           sortColumnIndex == 8 ? e.APPOINTMENTDAY :
                                                           sortColumnIndex == 9 ? e.APPOIMENTSTATUS :
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