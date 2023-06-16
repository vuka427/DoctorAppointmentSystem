using DoctorAppointmentSystem.Areas.Admin.Models.AdminUser;
using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorSchedule;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorScheduleManage;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    public class DoctorScheduleManageController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;
        private readonly IloggerService _logger;

        public DoctorScheduleManageController(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper, IloggerService logger)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: Admin/DoctorSchedule
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Doctor Schedule");
            var listDoctor = _dbContext.DOCTOR.Where(d => d.DELETEDFLAG == false).ToList();
            var ListConsultantTime = _sysParam.GetAllParam().Where(p => p.GROUPID == "ConsultantTime").ToList();
            ViewBag.doctor = new SelectList(listDoctor, "DOCTORID", "DOCTORNAME");
            ViewBag.consultanttime = new SelectList(ListConsultantTime, "ID", "NOTE");
            return View();
        }

        //load data doctor schedule to jquery datatable
        public async Task<ActionResult> LoadDoctorScheduleData(JqueryDatatableParam param)
        {
            var schedule = await _dbContext.SCHEDULE.Where(d => d.DELETEDFLAG == false ).ToListAsync();

            IEnumerable<DoctorScheduleViewModel> Schedules = schedule.Select(dt => _mapper.GetMapper().Map<SCHEDULE, DoctorScheduleViewModel>(dt)).ToList();

            if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Schedules = Schedules.Where(x => x.DOCTORID.ToString().ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.WORKINGDAY.ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
            }
            var sortColumnIndex = param.iSortCol_0;
            var sortDirection = param.sSortDir_0;

            if (sortColumnIndex == 1)
            {
                Schedules = (sortDirection == "asc" ? Schedules.OrderBy(c => c.SCHEDULEID) : Schedules.OrderByDescending(c => c.SCHEDULEID));
            }
            else if (sortColumnIndex == 2)
            {
                Schedules = (sortDirection == "asc" ? Schedules.OrderBy(c => c.DOCTORID) : Schedules.OrderByDescending(c => c.DOCTORID));
            }
            else if (sortColumnIndex == 3)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.DOCTORNAME) : Schedules.OrderByDescending(c => c.DOCTORNAME);
            }
            else if (sortColumnIndex == 4)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.WORKINGDAY) : Schedules.OrderByDescending(c => c.WORKINGDAY);
            }
            else if (sortColumnIndex == 5)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.SHIFTTIME) : Schedules.OrderByDescending(c => c.SHIFTTIME);
            }
            else if (sortColumnIndex == 6)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.BREAKTIME) : Schedules.OrderByDescending(c => c.BREAKTIME);
            }
            else if (sortColumnIndex == 7)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.CONSULTANTTIME) : Schedules.OrderByDescending(c => c.CONSULTANTTIME);
            }
            else if (sortColumnIndex == 8)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.APPOINTMENTNUM) : Schedules.OrderByDescending(c => c.APPOINTMENTNUM);
            }
            else if (sortColumnIndex == 9)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.CREATEDBY) : Schedules.OrderByDescending(c => c.APPOINTMENTNUM);
            }
            else if(sortColumnIndex == 10)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.CREATEDDATE) : Schedules.OrderByDescending(c => c.APPOINTMENTNUM);
            }
            else if(sortColumnIndex == 12)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.UPDATEDBY) : Schedules.OrderByDescending(c => c.APPOINTMENTNUM);
            }
            else 
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.UPDATEDDATE) : Schedules.OrderByDescending(c => c.APPOINTMENTNUM);
            }

             var displayResult = Schedules.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = Schedules.Count();


            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

        }

        //create doctor schedule
        public JsonResult CreateDoctorSchedule(DoctorScheduleCreateModel model)
        {

            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }

            var datenow = DateTime.Now;

            // check doctor
            if(model.DOCTORID <= 0)
            {
                return Json(new { error = 1, msg = "Doctor is repuired" });
            }
            var doctor = _dbContext.DOCTOR.Where(d => d.DOCTORID == model.DOCTORID && d.DELETEDFLAG == false).FirstOrDefault();
            if(doctor == null)
            {
                return Json(new { error = 1, msg = "Can't find doctor !" });
            }

            //check working day
            if(model.WORKINGDAY == null)
            {
                return Json(new { error = 1, msg = "Schedule date is repuired" });
            }
            if(model.WORKINGDAY < datenow)
            {
                return Json(new { error = 1, msg = "Schedule date greater than current date" });
            }
            //check shift time
            if(model.SHIFTTIME == null)
            {
                return Json(new { error = 1, msg = "Start time is repuired" });
            }
            //check break time
            if (model.BREAKTIME == null)
            {
                return Json(new { error = 1, msg = "End time is repuired" });
            }
            if (model.BREAKTIME <= model.SHIFTTIME )
            {
                return Json(new { error = 1, msg = "Start time smaller than end time" });
            }
            //check consutant time
            if (model.CONSULTANTTIME <= 0)
            {
                return Json(new { error = 1, msg = "Consutant time is required" });
            }

            var schedule = new SCHEDULE()
            {
                DOCTORID = model.DOCTORID,
                WORKINGDAY = model.WORKINGDAY,
                SHIFTTIME = model.SHIFTTIME,
                BREAKTIME = model.BREAKTIME,
                CONSULTANTTIME = model.CONSULTANTTIME,
                CREATEDBY= CurentUser.USERNAME,
                CREATEDDATE = datenow,
                UPDATEDBY = CurentUser.USERNAME,
                UPDATEDDATE = datenow,
                DELETEDFLAG = false,
            };

            try
            {
                _dbContext.SCHEDULE.Add(schedule);
                _dbContext.SaveChanges();
            }
            catch 
            {
                return Json(new { error = 1, msg = "Create doctor is failed!" });
            }


            return Json(new {error = 0, msg = "ok"});
        }

        //load Schedule data 
        public JsonResult LoadDoctorScheduleInfo(int scheduleid)
        {

            var schedule = _dbContext.SCHEDULE.Where(s => s.SCHEDULEID == scheduleid && s.DELETEDFLAG == false).FirstOrDefault();
            var scheduleView = _mapper.GetMapper().Map<SCHEDULE, DoctorScheduleViewEditModel>(schedule);

            return Json(new {error = 0, msg ="ok", schedule = scheduleView });
        }

        //update doctor schedule
        public JsonResult UpdateDoctorSchedule(DoctorScheduleCreateModel model)
        {

            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }

            var datenow = DateTime.Now;

            // check doctor
            if (model.DOCTORID <= 0)
            {
                return Json(new { error = 1, msg = "Doctor is repuired" });
            }
            var doctor = _dbContext.DOCTOR.Where(d => d.DOCTORID == model.DOCTORID && d.DELETEDFLAG == false).FirstOrDefault();
            if (doctor == null)
            {
                return Json(new { error = 1, msg = "Can't find doctor !" });
            }

            //check working day
            if (model.WORKINGDAY == null)
            {
                return Json(new { error = 1, msg = "Schedule date is repuired" });
            }
            if (model.WORKINGDAY < datenow)
            {
                return Json(new { error = 1, msg = "Schedule date greater than current date" });
            }
            //check shift time
            if (model.SHIFTTIME == null)
            {
                return Json(new { error = 1, msg = "Start time is repuired" });
            }
            //check break time
            if (model.BREAKTIME == null)
            {
                return Json(new { error = 1, msg = "End time is repuired" });
            }
            if (model.BREAKTIME <= model.SHIFTTIME)
            {
                return Json(new { error = 1, msg = "Start time smaller than end time" });
            }
            //check consutant time
            if (model.CONSULTANTTIME <= 0)
            {
                return Json(new { error = 1, msg = "Consutant time is required" });
            }

            var schedule = new SCHEDULE()
            {
                DOCTORID = model.DOCTORID,
                WORKINGDAY = model.WORKINGDAY,
                SHIFTTIME = model.SHIFTTIME,
                BREAKTIME = model.BREAKTIME,
                CONSULTANTTIME = model.CONSULTANTTIME,
                CREATEDBY = CurentUser.USERNAME,
                CREATEDDATE = datenow,
                UPDATEDBY = CurentUser.USERNAME,
                UPDATEDDATE = datenow,
                DELETEDFLAG = false,
            };

            try
            {
               // _dbContext.SCHEDULE.Add(schedule);
               // _dbContext.SaveChanges();
            }
            catch
            {
                return Json(new { error = 1, msg = "Update doctor is failed!" });
            }

            return Json(new { error = 0, msg = "ok" });
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