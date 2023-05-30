using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Areas.Admin.Models.PatientManage;
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
    public class PatientManageController : Controller
    {
        private readonly DBContext _dbContext;

        public PatientManageController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Admin/Partient
        public ActionResult Index()
        {
            
            return View();
        }

        public async Task<ActionResult> LoadPatientData(JqueryDatatableParam param)
        {
            var mapper = MapperService.InitializeAutomapper();
            var patients = await _dbContext.PATIENT.Where(d => d.DELETEDFLAG == false).Include("DEPARTMENT").Include("USER").ToListAsync();
            IEnumerable<PatientViewModel> Patients = patients.Select(dt => mapper.Map<PATIENT, PatientViewModel>(dt)).ToList();


            if (!string.IsNullOrEmpty(param.sSearch)) //tìm kiếm
            {
                Patients = Patients.Where(x => x.PATIENTNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.PATIENTID.ToString().Contains(param.sSearch.ToLower())
                                              || x.PATIENTGENDER.ToLower().Contains(param.sSearch.ToLower())
                                              || x.PATIENTDATEOFBIRTH.ToLower().Contains(param.sSearch.ToLower())
                                              || x.PATIENTMOBILENO.ToString().Contains(param.sSearch.ToLower())
                                              || x.PATIENTADDRESS.ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
            }

            var sortColumnIndex = param.iSortCol_0;// Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
            var sortDirection = param.sSortDir_0; // HttpContext.Request.QueryString["sSortDir_0"];



            if (sortColumnIndex == 1)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.PATIENTID) : Patients.OrderByDescending(c => c.PATIENTID);
            }
            else if (sortColumnIndex == 5)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.PATIENTDATEOFBIRTH) : Patients.OrderByDescending(c => c.PATIENTDATEOFBIRTH);
            }
            else if (sortColumnIndex == 11)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.LOGINRETRYCOUNT) : Patients.OrderByDescending(c => c.LOGINRETRYCOUNT);
            }
            else if (sortColumnIndex == 10)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.LOGINLOCKDATE) : Patients.OrderByDescending(c => c.LOGINLOCKDATE);
            }
            else if (sortColumnIndex == 13)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.CREATEDDATE) : Patients.OrderByDescending(c => c.CREATEDDATE);
            }
            else if (sortColumnIndex == 15)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.UPDATEDDATE) : Patients.OrderByDescending(c => c.CREATEDDATE);
            }
            else
            {
                Func<PatientViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.PATIENTNAME :
                                                           sortColumnIndex == 3 ? e.USERNAME :
                                                           sortColumnIndex == 4 ? e.PATIENTGENDER :
                                                           sortColumnIndex == 6 ? e.PATIENTNATIONALID :
                                                           sortColumnIndex == 7 ? e.PATIENTMOBILENO :
                                                           sortColumnIndex == 8 ? e.EMAIL :
                                                           sortColumnIndex == 9 ? e.PATIENTADDRESS :
                                                           sortColumnIndex == 12 ? e.CREATEDBY :
                                                           e.UPDATEDBY;//11

                Patients = sortDirection == "asc" ? Patients.OrderBy(orderingFunction) : Patients.OrderByDescending(orderingFunction);
                //asc tăng dần  
            }


            var displayResult = Patients.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = Patients.Count();


            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

        }

    }
}