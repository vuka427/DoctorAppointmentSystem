using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    public class DoctorManageController : Controller
    {
        private readonly DBContext _dbContext;

        public DoctorManageController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }



        // GET: Admin/DoctorManage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadDoctorData(JqueryDatatableParam param)
        {
            var mapper = MapperService.InitializeAutomapper();
            var doctor = _dbContext.DOCTORs.Where(d => d.DELETEDFLAG == false).Include("DEPARTMENT").ToList();
            IEnumerable<DoctorViewModel> Doctors = doctor.Select(dt => mapper.Map<DOCTOR, DoctorViewModel>(dt)).ToList();


            if (!string.IsNullOrEmpty(param.sSearch)) //tìm kiếm
            {
                Doctors = Doctors.Where(x => x.DOCTORNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORID.ToString().Contains(param.sSearch.ToLower())
                                              || x.DOCTORGENDER.ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORDATEOFBIRTH.ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORMOBILENO.ToString().Contains(param.sSearch.ToLower())
                                              || x.DOCTORADDRESS.ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
            }

            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
            var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];

           

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
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.CREATEDATE) : Doctors.OrderByDescending(c => c.CREATEDATE);
            }
            else if (sortColumnIndex == 20)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.UPDATEDATE) : Doctors.OrderByDescending(c => c.CREATEDATE);
            }
            else
            {
                Func<DoctorViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.DOCTORNAME :
                                                           sortColumnIndex == 3 ? e.DOCTORGENDER :
                                                           sortColumnIndex == 5 ? e.DOCTORMOBILENO :
                                                           sortColumnIndex == 7 ? e.DOCTORADDRESS :
                                                           sortColumnIndex == 8 ? e.DEPARTMENT :
                                                           sortColumnIndex == 9 ? e.CREATEBY :
                                                           sortColumnIndex == 10 ? e.CREATEBY :
                                                           sortColumnIndex == 11 ? e.CREATEBY :
                                                           sortColumnIndex == 12 ? e.CREATEBY :
                                                           sortColumnIndex == 15 ? e.CREATEBY :
                                                           sortColumnIndex == 17 ? e.CREATEBY :
                                                           e.UPDATEBY;//11

                Doctors = sortDirection == "asc" ? Doctors.OrderBy(orderingFunction) : Doctors.OrderByDescending(orderingFunction);
                //asc tăng dần  
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
            }, JsonRequestBehavior.AllowGet);

        }



    }
}