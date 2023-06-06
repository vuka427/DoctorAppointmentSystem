using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    public class UserManageController : Controller
    {

        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapper _mapper;

        public UserManageController(DBContext dbContext, ISystemParamService sysParam, IMapper mapper)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
        }

        // GET: Admin/UserManage
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> LoadUserData(JqueryDatatableParam param)
        {
            var Doctors = _dbContext.USER.Where(d => d.DELETEDFLAG == false).Include("DOCTOR").Include("PATIENT");



            /*if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Doctors = Doctors.Where(x => x.EMAIL.ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORID.ToString().Contains(param.sSearch.ToLower())
                                              || x.DOCTORGENDER.ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORDATEOFBIRTH.ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORMOBILENO.ToString().Contains(param.sSearch.ToLower())
                                              || x.DOCTORADDRESS.ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
            }
            var sortColumnIndex = param.iSortCol_0;// Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
            var sortDirection = param.sSortDir_0; // HttpContext.Request.QueryString["sSortDir_0"];



            if (sortColumnIndex == 1)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.DOCTORID) : Doctors.OrderByDescending(c => c.DOCTORID);
            }
            else if (sortColumnIndex == 6)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.DOCTORDATEOFBIRTH) : Doctors.OrderByDescending(c => c.DOCTORDATEOFBIRTH);
            }
            else if (sortColumnIndex == 16)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.LOGINRETRYCOUNT) : Doctors.OrderByDescending(c => c.LOGINRETRYCOUNT);
            }
            else if (sortColumnIndex == 13)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.WORKINGSTARTDATE) : Doctors.OrderByDescending(c => c.WORKINGSTARTDATE);
            }
            else if (sortColumnIndex == 14)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.WORKINGENDDATE) : Doctors.OrderByDescending(c => c.WORKINGENDDATE);
            }
            else if (sortColumnIndex == 18)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.CREATEDDATE) : Doctors.OrderByDescending(c => c.CREATEDDATE);
            }
            else if (sortColumnIndex == 20)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.UPDATEDDATE) : Doctors.OrderByDescending(c => c.CREATEDDATE);
            }
            else
            {
                Func<DoctorViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.DOCTORNAME :
                                                           sortColumnIndex == 3 ? e.DOCTORGENDER :
                                                           sortColumnIndex == 5 ? e.DOCTORMOBILENO :
                                                           sortColumnIndex == 7 ? e.DOCTORADDRESS :
                                                           sortColumnIndex == 8 ? e.DEPARTMENT :
                                                           sortColumnIndex == 9 ? e.EMAIL :
                                                           sortColumnIndex == 10 ? e.DOCTORADDRESS :
                                                           sortColumnIndex == 11 ? e.SPECIALITY :
                                                           sortColumnIndex == 12 ? e.QUALIFICATION :
                                                           sortColumnIndex == 17 ? e.CREATEDBY :
                                                           e.UPDATEDBY;//20

                Doctors = sortDirection == "asc" ? Doctors.OrderBy(orderingFunction) : Doctors.OrderByDescending(orderingFunction);

            }


            var displayResult = Doctors.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = Doctors.Count();


            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);*/

            return Json(new { });
        }
    }
}