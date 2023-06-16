using Antlr.Runtime;
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
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    
    [AppAuthorize("Admin")]
    public class DoctorManageController : Controller
    {
        
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;

        public DoctorManageController(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
        }

        // GET: Admin/DoctorManage
      
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Doctor management");

            var sysParam = _sysParam.GetAllParam();

            ViewBag.genders = ViewBag.genders = new SelectList(sysParam.Where(c => c.GROUPID.Equals("Gender") && c.DELETEDFLAG == false).ToList(), "ID", "PARAVAL");
            ViewBag.department = new SelectList( _dbContext.DEPARTMENT.ToList(), "DEPARTMENTID", "DEPARTMENTNAME");
            return View();
        }

        //load data doctors to jquery datatable
        public async  Task<ActionResult> LoadDoctorData( JqueryDatatableParam param)
        {
            var doctor = await _dbContext.DOCTOR.Where(d => d.DELETEDFLAG == false).Include("DEPARTMENT").Include("USER").ToListAsync();
            IEnumerable<DoctorViewModel> Doctors = doctor.Select(dt => _mapper.GetMapper().Map<DOCTOR, DoctorViewModel>(dt)).ToList();
            
            if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Doctors = Doctors.Where(x => x.DOCTORNAME.ToLower().Contains(param.sSearch.ToLower())
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
            else if (sortColumnIndex == 15)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.LOGINRETRYCOUNT) : Doctors.OrderByDescending(c => c.LOGINRETRYCOUNT);
            }
            else if (sortColumnIndex == 12)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.WORKINGSTARTDATE) : Doctors.OrderByDescending(c => c.WORKINGSTARTDATE);
            }
            else if (sortColumnIndex == 13)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.WORKINGENDDATE) : Doctors.OrderByDescending(c => c.WORKINGENDDATE);
            }
            else if (sortColumnIndex == 17)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.CREATEDDATE) : Doctors.OrderByDescending(c => c.CREATEDDATE);
            }
            else if (sortColumnIndex == 19)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.UPDATEDDATE) : Doctors.OrderByDescending(c => c.CREATEDDATE);
            }
            else
            {
                Func<DoctorViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.DOCTORNAME :
                                                           sortColumnIndex == 3 ? e.USERNAME :
                                                           sortColumnIndex == 4 ? e.DEPARTMENT:
                                                           sortColumnIndex == 5 ? e.DOCTORGENDER :
                                                           sortColumnIndex == 8 ? e.DOCTORMOBILENO :
                                                           sortColumnIndex == 10 ? e.DOCTORADDRESS :
                                                           sortColumnIndex == 9 ? e.EMAIL :
                                                           sortColumnIndex == 11 ? e.SPECIALITY :
                                                           sortColumnIndex == 16 ? e.CREATEDBY :
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
            }, JsonRequestBehavior.AllowGet);

        }

        // Create doctor
        [HttpPost]
        public JsonResult CreateDoctor(DoctorCreateModel model)
        {
            model.DOCTORNAME = model.DOCTORNAME != null ? model.DOCTORNAME.Trim() : model.DOCTORNAME;
            model.DOCTORNATIONALID = model.DOCTORNATIONALID != null ? model.DOCTORNATIONALID.Trim() : model.DOCTORNATIONALID;
            model.DOCTORADDRESS = model.DOCTORADDRESS != null ? model.DOCTORADDRESS.Trim() : model.DOCTORADDRESS;

            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }

            // validation
            // check DOCTORNAME
            ValidationResult NameValidResult = ValidationInput.NameIsValid(model.DOCTORNAME,"Doctor name");
            if (!NameValidResult.Success)
            {
                return Json(new { error = 1, msg = NameValidResult.ErrorMessage });
            }

            // check USERNAME
            ValidationResult UNValidResult = ValidationInput.UserNameIsValid(model.USERNAME, "Username");
            if (!UNValidResult.Success)
            {
                return Json(new { error = 1, msg = UNValidResult.ErrorMessage });
            }

            var usermatch = _dbContext.USER.Where(u => u.USERNAME == model.USERNAME).ToList();
            if(usermatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Username already exists!" });
            }

            // check DOCTORGENDER
            var sysParam = _sysParam.GetAllParam();
            ValidationResult GenderValidResult = ValidationInput.GenderIsValid(model.DOCTORGENDER, sysParam);
            if (!GenderValidResult.Success)
            {
                return Json(new { error = 1, msg = GenderValidResult.ErrorMessage });
            }

            // check PASSWORD
            ValidationResult PaswdValidResult = ValidationInput.PasswordIsValid(model.PASSWORD);
            if (!PaswdValidResult.Success)
            {
                return Json(new { error = 1, msg = PaswdValidResult.ErrorMessage });
            }
            // check DOCTORNATIONALID
            ValidationResult NationalValidResult = ValidationInput.NationalIsValid(model.DOCTORNATIONALID);
            if(!NationalValidResult.Success){
                return Json(new { error = 1, msg = NationalValidResult.ErrorMessage });
            }
            var nationidmatch = _dbContext.DOCTOR.Where(u => u.DOCTORNATIONALID == model.DOCTORNATIONALID).ToList();
            if (nationidmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Nation ID already exists !" });
            }

            //check DOCTORDATEOFBIRTH
            ValidationResult DOBValidResult = ValidationInput.DateOfBirthIsValid(model.DOCTORDATEOFBIRTH);
            if (!DOBValidResult.Success)
            {
                return Json(new { error = 1, msg = DOBValidResult.ErrorMessage });
            }

            //check DOCTORMOBILENO
            ValidationResult MobileValidResult = ValidationInput.MobileIsValid(model.DOCTORMOBILENO);
            if (!MobileValidResult.Success)
            {
                return Json(new { error = 1, msg = MobileValidResult.ErrorMessage });
            }
            var mobilematch = _dbContext.DOCTOR.Where(u => u.DOCTORMOBILENO == model.DOCTORMOBILENO).ToList();
            if (mobilematch.Count > 0)
            {
                return Json(new { error = 1, msg = "Mobile number already exists !" });
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

            // check DOCTORADDRESS
            ValidationResult AddressValidResult = ValidationInput.AddressIsValid(model.DOCTORADDRESS);
            if (!AddressValidResult.Success)
            {
                return Json(new { error = 1, msg = AddressValidResult.ErrorMessage });
            }
            // check SPECIALITY
            ValidationResult SpecialityValidResult = ValidationInput.SpecialityIsValid(model.SPECIALITY);
            if (!SpecialityValidResult.Success)
            {
                return Json(new { error = 1, msg = SpecialityValidResult.ErrorMessage });
            }

            // check WORKINGSTARTDATE , WORKINGENDDATE
            ValidationResult WorkingValidResult = ValidationInput.WorkingIsValid(model.WORKINGSTARTDATE, model.WORKINGENDDATE);
            if (!WorkingValidResult.Success)
            {
                return Json(new { error = 1, msg = WorkingValidResult.ErrorMessage });
            }

            //map model bind to doctor

            DOCTOR Doctors = _mapper.GetMapper().Map<DoctorCreateModel, DOCTOR>(model);

            var department = _dbContext.DEPARTMENT.Find(Doctors.DEPARTMENTID);
            if (department == null) { return Json(new { error = 1, msg = "Error ! Can`t find Department !" }); }
            Doctors.DEPARTMENT = department;

            var date = DateTime.Now;
            Doctors.CREATEDBY = CurentUser.USERNAME;
            Doctors.CREATEDDATE = date;
            Doctors.UPDATEDBY = CurentUser.USERNAME;
            Doctors.UPDATEDDATE = date;

            //check role exists
            var role = _dbContext.ROLE.Where(r => r.ROLENAME == "Doctor").FirstOrDefault();
            if (role== null)
            {
              role = _dbContext.ROLE.Add(new ROLE()
                {
                    ROLENAME = "Doctor",
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
                catch (Exception ex)
                {
                    //write error log
                    return Json(new { error = 1, msg = ex.ToString() });
                }
            }

            var hashcode = PasswordHelper.HashPassword(model.PASSWORD);

            USER user = new USER() {
                
                USERNAME = model.USERNAME,
                ROLEID = role.ROLEID,
                PASSWORDHASH = hashcode,
                EMAIL = model.EMAIL,
                USERTYPE = "Doctor",//Partient , Admin
                LOGINRETRYCOUNT = 0 ,
                STATUS = true,
                CREATEDBY = CurentUser.USERNAME,
                UPDATEDBY = CurentUser.USERNAME,
                CREATEDDATE = date,
                UPDATEDDATE = date,
                DELETEDFLAG = false,
            };

            var newuser = _dbContext.USER.Add(user);
            Doctors.USERID = newuser.USERID;
            _dbContext.DOCTOR.Add(Doctors);

            try
            { 
               
                _dbContext.SaveChanges();

            }catch(Exception ex)
            {
                //write error log
                return Json(new { error = 1, msg = ex.ToString() });
            }

          


            return Json(new { error = 0, msg = "ok" });
        }


        [HttpPost]
        //load doctor data for update 
        public JsonResult LoadDoctorInfo(int DoctorId)
        {
            if (DoctorId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor !" });
            }
            var doctor = _dbContext.DOCTOR.Where(d=>d.DOCTORID == DoctorId && d.DELETEDFLAG == false).Include("USER").FirstOrDefault();
            if (doctor == null)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor!" });
            }

           
            DoctorViewEditModel dt = _mapper.GetMapper().Map<DOCTOR, DoctorViewEditModel>(doctor);

            return Json(new { error = 0, msg = "ok", doctor = dt });
        }

        [HttpPost]
        //update doctor
        public JsonResult UpdateDoctor(DoctorEditModel model)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }

            var oldDoctor = _dbContext.DOCTOR.Where(d => d.DOCTORID == model.DOCTORID && d.DELETEDFLAG == false).Include("USER").FirstOrDefault();
            USER oldUser = null;

            if (oldDoctor == null)
            {
                return Json(new { error = 1, msg = "Error ! Can`t find Doctor !" });
            }
            else
            {
                if (oldDoctor.USER == null)
                {
                    return Json(new { error = 1, msg = "Error ! Can`t find Doctor !" });
                }
                oldUser = oldDoctor.USER;
            }

            model.DOCTORNAME = model.DOCTORNAME != null ? model.DOCTORNAME.Trim() : model.DOCTORNAME;
            model.DOCTORNATIONALID = model.DOCTORNATIONALID != null ? model.DOCTORNATIONALID.Trim() : model.DOCTORNATIONALID;
            model.DOCTORADDRESS = model.DOCTORADDRESS != null ? model.DOCTORADDRESS.Trim() : model.DOCTORADDRESS;
;
            // validation
            // check DOCTORNAME
            ValidationResult NameValidResult = ValidationInput.NameIsValid(model.DOCTORNAME, "Doctor name");
            if (!NameValidResult.Success)
            {
                return Json(new { error = 1, msg = NameValidResult.ErrorMessage });
            }

            // check DOCTORGENDER
            var sysParam = _sysParam.GetAllParam();
            ValidationResult GenderValidResult = ValidationInput.GenderIsValid(model.DOCTORGENDER, sysParam);
            if (!GenderValidResult.Success)
            {
                return Json(new { error = 1, msg = GenderValidResult.ErrorMessage });
            }

            // check DOCTORNATIONALID
            ValidationResult NationalValidResult = ValidationInput.NationalIsValid(model.DOCTORNATIONALID);
            if (!NationalValidResult.Success)
            {
                return Json(new { error = 1, msg = NationalValidResult.ErrorMessage });
            }
            var nationidmatch = _dbContext.DOCTOR.Where(u => u.DOCTORNATIONALID == model.DOCTORNATIONALID && 
                                                             u.DOCTORNATIONALID != oldDoctor.DOCTORNATIONALID ).ToList();
            if (nationidmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Nation ID already exists !" });
            }

            //check DOCTORDATEOFBIRTH
            ValidationResult DOBValidResult = ValidationInput.DateOfBirthIsValid(model.DOCTORDATEOFBIRTH);
            if (!DOBValidResult.Success)
            {
                return Json(new { error = 1, msg = DOBValidResult.ErrorMessage });
            }

            //check DOCTORMOBILENO
            ValidationResult MobileValidResult = ValidationInput.MobileIsValid(model.DOCTORMOBILENO);
            if (!MobileValidResult.Success)
            {
                return Json(new { error = 1, msg = MobileValidResult.ErrorMessage });
            }
            var mobilematch = _dbContext.DOCTOR.Where(u => u.DOCTORMOBILENO == model.DOCTORMOBILENO && 
                                                           u.DOCTORMOBILENO != oldDoctor.DOCTORMOBILENO ).ToList();
            if (mobilematch.Count > 0)
            {
                return Json(new { error = 1, msg = "Mobile number already exists !" });
            }

            // check EMAIL
            ValidationResult EmailValidResult = ValidationInput.EmailIsValid(model.EMAIL);
            if (!EmailValidResult.Success)
            {
                return Json(new { error = 1, msg = EmailValidResult.ErrorMessage });
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL && 
                                                        u.EMAIL != oldDoctor.USER.EMAIL).ToList();
            if (emailmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Email already exists!" });
            }

            // check DOCTORADDRESS
            ValidationResult AddressValidResult = ValidationInput.AddressIsValid(model.DOCTORADDRESS);
            if (!AddressValidResult.Success)
            {
                return Json(new { error = 1, msg = AddressValidResult.ErrorMessage });
            }
            // check SPECIALITY
            ValidationResult SpecialityValidResult = ValidationInput.SpecialityIsValid(model.SPECIALITY);
            if (!SpecialityValidResult.Success)
            {
                return Json(new { error = 1, msg = SpecialityValidResult.ErrorMessage });
            }

            // check WORKINGSTARTDATE , WORKINGENDDATE
            ValidationResult WorkingValidResult = ValidationInput.WorkingIsValid(model.WORKINGSTARTDATE, model.WORKINGENDDATE);
            if (!WorkingValidResult.Success)
            {
                return Json(new { error = 1, msg = WorkingValidResult.ErrorMessage });
            }
            //map model bind to doctor

            DOCTOR newDoctors = _mapper.GetMapper().Map<DoctorEditModel, DOCTOR>(model);

            var department = _dbContext.DEPARTMENT.Find(newDoctors.DEPARTMENTID);
            if (department == null) { return Json(new { error = 1, msg = "Error ! Can`t find Department !" }); }
            newDoctors.DEPARTMENT = department;
            var date = DateTime.Now;
            
            //update old doctor info to new doctor  
            newDoctors.DOCTORID = oldDoctor.DOCTORID;
            newDoctors.USERID = oldUser.USERID;
            newDoctors.CREATEDBY = oldDoctor.CREATEDBY;
            newDoctors.CREATEDDATE = oldDoctor.CREATEDDATE;
            newDoctors.UPDATEDBY = CurentUser.USERNAME;
            newDoctors.UPDATEDDATE = date;
            newDoctors.DELETEDFLAG = false;

            //update user info
            oldUser.EMAIL = model.EMAIL;
            oldUser.UPDATEDDATE = date;
            oldUser.UPDATEDBY = CurentUser.USERNAME;

            _dbContext.USER.AddOrUpdate(oldUser);
            _dbContext.DOCTOR.AddOrUpdate(newDoctors);
            try
            {
                _dbContext.SaveChanges();
            } catch(Exception ex)
            {
                //write error log
                return Json(new { error = 1, msg = ex.ToString() });
            }
            

            return Json(new { error = 0, msg = "ok" });

        }

        //delete doctor
        [HttpPost]
        public JsonResult DeleteDoctor(int DoctorId)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }
            if (DoctorId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not delete doctor !" });
            }
            var doctor = _dbContext.DOCTOR.Where(d => d.DOCTORID == DoctorId && d.DELETEDFLAG == false).FirstOrDefault();
            if (doctor == null)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor !" });
            }

            doctor.UPDATEDDATE = DateTime.Now;
            doctor.UPDATEDBY = CurentUser.USERNAME;
            doctor.DELETEDFLAG = true;
            doctor.USER.UPDATEDDATE = DateTime.Now;
            doctor.USER.DELETEDFLAG = true;
            doctor.USER.UPDATEDBY = CurentUser.USERNAME;
            

            _dbContext.DOCTOR.AddOrUpdate(doctor);
            _dbContext.USER.AddOrUpdate(doctor.USER);
            try {
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
            if(User.Identity.IsAuthenticated == true)
            {
                var userName = User.Identity.Name;
                if(userName != null)
                {
                    var currentUser = _dbContext.USER.Where(u => u.USERNAME == userName && u.DELETEDFLAG == false).FirstOrDefault();
                    return currentUser;
                }
            }
            return null;
        }

    }
}