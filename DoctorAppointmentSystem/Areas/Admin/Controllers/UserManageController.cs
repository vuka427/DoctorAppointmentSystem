using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Areas.Admin.Models.UserManage;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
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
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("User management");
            return View();
        }

        //load data user to jquery datatable
        public async Task<ActionResult> LoadUserData(JqueryDatatableParam param)
        {
            var users = await _dbContext.USER.Where(d => d.DELETEDFLAG == false).Include("DOCTOR").Include("PATIENT").ToListAsync();

            IEnumerable<UserViewModel> Users =  users.Select(dt => _mapper.GetMapper().Map<USER, UserViewModel>(dt)).ToList();

            

            if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Users = Users.Where(x => x.EMAIL.ToLower().Contains(param.sSearch.ToLower())
                                              || x.USERID.ToString().Contains(param.sSearch.ToLower())
                                              || x.FULLNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.USERNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.USERTYPE.ToLower().Contains(param.sSearch.ToLower())
                                              || x.EMAIL.ToLower().Contains(param.sSearch.ToLower())
                                              || x.MOBILENO.ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
            }
            var sortColumnIndex = param.iSortCol_0;// Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
            var sortDirection = param.sSortDir_0; // HttpContext.Request.QueryString["sSortDir_0"];

            if (sortColumnIndex == 1)
            {
                Users = (sortDirection == "asc" ? Users.OrderBy(c => c.USERID) : Users.OrderByDescending(c => c.USERID));
            }
            else
            {
                Func<UserViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.FULLNAME :
                                                           sortColumnIndex == 3 ? e.USERNAME :
                                                           sortColumnIndex == 4 ? e.USERTYPE :
                                                           sortColumnIndex == 5 ? e.GENDER :
                                                           sortColumnIndex == 6 ? e.MOBILENO :
                                                           sortColumnIndex == 7 ? e.EMAIL:
                                                           sortColumnIndex == 8 ? e.LASTLOGIN :
                                                           e.CREATEDBY
                                                           ;

                Users = (sortDirection == "asc" ? Users.OrderBy(orderingFunction) : Users.OrderByDescending(orderingFunction));

            }


            var displayResult = Users.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = Users.Count();


            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

            
        }



        //delete user
        [HttpPost]
        public JsonResult DeleteUser(int USERID)
        {
            if (USERID == 0)
            {
                return Json(new { error = 1, msg = "Failed ! do not delete user !" });
            }
            var user = _dbContext.USER.Where(d => d.USERID == USERID).Include("DOCTOR").Include("PATIENT").FirstOrDefault();
            if (user == null)
            {
                return Json(new { error = 1, msg = "Failed ! do not delete user !" });
            }
            
            user.DELETEDFLAG = true; 
            _dbContext.USER.AddOrUpdate(user);

            if (user.USERTYPE == "Doctor")
            {
                if(user.DOCTOR.FirstOrDefault() != null)
                {
                    var doctor = user.DOCTOR.FirstOrDefault();
                    doctor.DELETEDFLAG = true;
                    _dbContext.DOCTOR.AddOrUpdate(doctor);
                }
            }
            if (user.USERTYPE == "Patient")
            {
                if (user.PATIENT.FirstOrDefault() != null)
                {
                    var patient = user.PATIENT.FirstOrDefault();
                    patient.DELETEDFLAG = true;
                    _dbContext.PATIENT.AddOrUpdate(patient);
                }
            }
           
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //write error log
                return Json(new { error = 1, msg = ex.ToString() });
            }

            return Json(new { error = 0, msg = "ok" });
        }

        //reset password user
        [HttpPost]
        public JsonResult ResetPassword(ResetPasswordModel model)
        {
            string patternPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$";

            if (model.USERID == 0)
            {
                return Json(new { error = 1, msg = "Can't reset password for user !" });
            }
            var user = _dbContext.USER.Where(d => d.USERID == model.USERID).FirstOrDefault();
            if (user == null)
            {
                return Json(new { error = 1, msg = "Can't reset password for user! do not find user !" });
            }
           
            if (String.IsNullOrEmpty(model.PASSWORD))
            {

                return Json(new { error = 1, msg = "Password is required!" });
            }

            Match strpawd = Regex.Match(model.PASSWORD, patternPassword, RegexOptions.IgnoreCase);
            if (!strpawd.Success)
            {
                return Json(new { error = 1, msg = @"Password charater at least one uppercase letter, one lowercase letter, one number and one special character: [a - z],[A - Z],[0 - 9],[@$!%*?&]" });
            }

            var hashcode = PasswordHelper.HashPassword(model.PASSWORD);

            user.PASSWORDHASH = hashcode;

            _dbContext.USER.AddOrUpdate(user);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { error = 1, msg = ex.ToString() });
                //write error log
            }


            return Json(new { error = 0, msg = "ok" });
        }

    }
}