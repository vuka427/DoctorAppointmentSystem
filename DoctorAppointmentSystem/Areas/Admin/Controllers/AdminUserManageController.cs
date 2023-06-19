using DoctorAppointmentSystem.Areas.Admin.Models.AdminUser;
using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Areas.Admin.Models.Validation;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    [AppAuthorize("Admin")]
    public class AdminUserManageController : Controller
    {

        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;
        private readonly IloggerService _logger;

        public AdminUserManageController(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper, IloggerService logger)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
            _logger = logger;

        }

        // GET: Admin/UserAdminManage
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Admin management");
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            var user = GetCurrentUser();
            ViewBag.Name = user != null ? user.USERNAME : "";

            return View();
        }

        //load data user to jquery datatable
        public async Task<ActionResult> LoadAdminData(JqueryDatatableParam param)
        {
            var users = await _dbContext.USER.Where(d => d.DELETEDFLAG == false && d.USERTYPE == "Admin").ToListAsync();

            IEnumerable<AdminUserViewModel> Users = users.Select(dt => _mapper.GetMapper().Map<USER, AdminUserViewModel>(dt)).ToList();

            if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Users = Users.Where(x => x.EMAIL.ToLower().Contains(param.sSearch.ToLower())
                                              || x.USERID.ToString().Contains(param.sSearch.ToLower())
                                              || x.USERNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.EMAIL.ToLower().Contains(param.sSearch.ToLower())
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
                Func<AdminUserViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.USERNAME :
                                                           sortColumnIndex == 3 ? e.EMAIL :
                                                           sortColumnIndex == 4 ? e.LASTLOGIN :
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

        // Create admin
        [HttpPost]
        public JsonResult CreateAdmin (AdminUserCreateModel model)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
               return Json(new { error = 1, msg = "Can't find current user !" });
            }

            // validation
            // check USERNAME
            ValidationResult UNValidResult = ValidationInput.UserNameIsValid(model.USERNAME, "Username");
            if (!UNValidResult.Success)
            {
                return Json(new { error = 1, msg = UNValidResult.ErrorMessage });
            }
            var usermatch = _dbContext.USER.Where(u => u.USERNAME == model.USERNAME).ToList();
            if (usermatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Username already exists!" });
            }

            // check PASSWORD
            ValidationResult PaswdValidResult = ValidationInput.PasswordIsValid(model.PASSWORD);
            if (!PaswdValidResult.Success)
            {
                return Json(new { error = 1, msg = PaswdValidResult.ErrorMessage }); 
            }

            // check EMAIL
            ValidationResult EmailValidResult = ValidationInput.EmailIsValid(model.EMAIL);
            if (!EmailValidResult.Success)
            {
                return Json(new { error = 1, msg = EmailValidResult.ErrorMessage }); 
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL).ToList();
            if (emailmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Email already exists!" }); 
            }

            var date = DateTime.Now;
          
            //check role exists
            var role = _dbContext.ROLE.Where(r => r.ROLENAME == "Admin").FirstOrDefault(); 
            if (role == null)
            {
                role = _dbContext.ROLE.Add(new ROLE()
                {
                    ROLENAME = "Admin",
                    CREATEDBY = CurentUser.USERNAME,
                    UPDATEDBY = CurentUser.USERNAME,
                    CREATEDDATE = date,
                    UPDATEDDATE = date,
                    DELETEDFLAG = false,
                });

                try
                {
                    _dbContext.SaveChanges();

                }
                catch 
                {
                    //write error log
                    _logger.InsertLog("Admin","create admin is failed",nameof(CreateAdmin),"I",CurentUser.USERNAME);
                    return Json(new { error = 1, msg = "create admin is failed !" });
                }
            }

            var hashcode = PasswordHelper.HashPassword(model.PASSWORD);

            USER userAdmin = new USER()
            {
               
                USERNAME = model.USERNAME,
                ROLEID = role.ROLEID,
                PASSWORDHASH = hashcode,
                EMAIL = model.EMAIL,
                USERTYPE = "Admin",//Partient , Doctor
                LOGINRETRYCOUNT = 0,
                STATUS = true,
                CREATEDBY = CurentUser.USERNAME,
                UPDATEDBY = CurentUser.USERNAME,
                CREATEDDATE = date,
                UPDATEDDATE = date,
                DELETEDFLAG = false,
            };

            _dbContext.USER.Add(userAdmin);

            try
            {
                _dbContext.SaveChanges();
            }
            catch 
            {
                //write error log
                _logger.InsertLog("Admin", "create admin failed", nameof(CreateAdmin), "I", CurentUser.USERNAME);
                return Json(new { error = 1, msg = "Create admin is failed !" });
            }

            return Json(new { error = 0, msg = "ok" });
        }

        [HttpPost]
        //load admin data for update 
        public JsonResult LoadAdminInfo(int USERID)
        {
            if (USERID == 0)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor !" });
            }
            var userAdmin = _dbContext.USER.Where(d => d.USERID == USERID && d.USERTYPE == "Admin" && d.DELETEDFLAG == false).FirstOrDefault();
            if (userAdmin == null)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor!" });
            }

            var adminInfo = _mapper.GetMapper().Map<USER, AdminUserViewEditModel>(userAdmin);
          
            return Json(new { error = 0, msg = "ok", admin = adminInfo });
        }

        [HttpPost]
        //update admin
        public JsonResult UpdateAdmin(AdminUserEditModel model)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }

            var oldAdmin = _dbContext.USER.Where(d => d.USERID == model.USERID && d.USERTYPE == "Admin" && d.DELETEDFLAG == false).FirstOrDefault();

            // check EMAIL
            ValidationResult EmailValidResult = ValidationInput.EmailIsValid(model.EMAIL);
            if (!EmailValidResult.Success)
            {
                return Json(new { error = 1, msg = EmailValidResult.ErrorMessage });
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL && u.EMAIL != oldAdmin.EMAIL).ToList();
            if (emailmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Email already exists!" });
            }

            oldAdmin.EMAIL = model.EMAIL;
            oldAdmin.UPDATEDBY = CurentUser.USERNAME;
            oldAdmin.UPDATEDDATE = DateTime.Now;
            _dbContext.USER.AddOrUpdate(oldAdmin);

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

        //delete admin user
        [HttpPost]
        public JsonResult DeleteAdmin(int USERID)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }
            if (USERID == 0)
            {
                return Json(new { error = 1, msg = "Failed ! do not delete user !" });
            }
            var user = _dbContext.USER.Where(d => d.USERID == USERID && d.USERTYPE == "Admin").FirstOrDefault();
            if (user == null)
            {
                return Json(new { error = 1, msg = "Failed ! do not delete user !" });
            }
            user.UPDATEDDATE = DateTime.Now;
            user.DELETEDFLAG = true;
            user.UPDATEDBY = CurentUser.USERNAME;
            _dbContext.USER.AddOrUpdate(user);
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