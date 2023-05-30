using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    public class DoctorManageController : Controller
    {
        private const string SATL = "dungvu";
        private readonly DBContext _dbContext;

        public DoctorManageController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Admin/DoctorManage
        public ActionResult Index()
        {
            
            ViewBag.department = new SelectList( _dbContext.DEPARTMENT.ToList(), "DEPARTMENTID", "DEPARTMENTNAME");
            return View();
        }

        //load data doctors  to jquery datatable
        public async  Task<ActionResult> LoadDoctorData( JqueryDatatableParam param)
        {
            var mapper = MapperService.InitializeAutomapper();
            var doctor = await _dbContext.DOCTOR.Where(d => d.DELETEDFLAG == false).Include("DEPARTMENT").Include("USER").ToListAsync();
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

        // Create doctor
        [HttpPost]
        public JsonResult CreateDoctor(DoctorCreateModel model)
        {
           

            model.DOCTORNAME = model.DOCTORNAME != null ? model.DOCTORNAME.Trim() : model.DOCTORNAME;
            model.DOCTORNATIONALID = model.DOCTORNATIONALID != null ? model.DOCTORNATIONALID.Trim() : model.DOCTORNATIONALID;
            model.DOCTORADDRESS = model.DOCTORADDRESS != null ? model.DOCTORADDRESS.Trim() : model.DOCTORADDRESS;

            string formatString = @"^[\p{L}\p{N}\s]*$"; // re!   \|!#$%&/()=?»«@£§€{}.-;'<>_,
            string patternMobile = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
            string patternUsername = @"^[a-z0-9-]*$"; 

            // validation
            // check DOCTORNAME
            if (String.IsNullOrEmpty(model.DOCTORNAME))
            {
                return Json(new { error = 1, msg = "Doctor name is not null !" });
            }

            Match strname = Regex.Match(model.DOCTORNAME, formatString, RegexOptions.IgnoreCase);
            if (!strname.Success)
            {
                return Json(new { error = 1, msg = $"Doctor name does not contain any special characters !" });
            }

            if (model.DOCTORNAME.Length >= 50)
            {
                return Json(new { error = 1, msg = "Doctor name charater max lenght is 50!" });
            }
            // check USERNAME
            if (String.IsNullOrEmpty(model.USERNAME))
            {
                return Json(new { error = 1, msg = "Username is not null !" });
            }

            Match strusername = Regex.Match(model.USERNAME, patternUsername, RegexOptions.IgnoreCase);
            if (!strusername.Success)
            {
                return Json(new { error = 1, msg = $"Username does not contain any special characters !" });
            }

            if (model.USERNAME.Length >= 50 || model.USERNAME.Length < 3)
            {
                return Json(new { error = 1, msg = "Username charater lenght is 3 to 50!" });
            }
            var usermatch = _dbContext.USER.Where(u => u.USERNAME == model.USERNAME).ToList();
            if(usermatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Username already exists!" });
            }
            // check DOCTORGENDER
            if (String.IsNullOrEmpty(model.DOCTORGENDER) && (model.DOCTORGENDER != "Male" || model.DOCTORGENDER != "Female"))
            {
                return Json(new { error = 1, msg = "Gender not match!" });
            }

            // check PASSWORD

            if (String.IsNullOrEmpty(model.PASSWORD) )
            {

                return Json(new { error = 1, msg = "Password is not null!" });
            }
            // check DOCTORNATIONALID
            if (String.IsNullOrEmpty(model.DOCTORNATIONALID))
            {
                return Json(new { error = 1, msg = "National ID is not null!" });
            }
            if (model.DOCTORNAME.Length >= 20)
            {
                return Json(new { error = 1, msg = "National ID charater max lenght is 20!" });
            }
            var nationidmatch = _dbContext.DOCTOR.Where(u => u.DOCTORNATIONALID == model.DOCTORNATIONALID).ToList();
            if (nationidmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Nation ID already exists!" });
            }
            //check DOCTORDATEOFBIRTH
            if (model.DOCTORDATEOFBIRTH > DateTime.Now)
            {
                return Json(new { error = 1, msg = "Date of birth smaller than current date !" });
            }
            //check DOCTORMOBILENO
            if (!String.IsNullOrEmpty(model.DOCTORMOBILENO))
            {
                Match m = Regex.Match(model.DOCTORMOBILENO, patternMobile, RegexOptions.IgnoreCase);

                if (!m.Success) //mobile
                {
                    return Json(new { error = 1, msg = $"Mobile No error !" });
                }
            }
            else
            {
                return Json(new { error = 1, msg = "Mobile No is not null !" });

            }
            // check EMAIL
            if (String.IsNullOrEmpty(model.EMAIL))
            {
                return Json(new { error = 1, msg = "Email is not null!" });
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL).ToList();
            if (emailmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Email already exists!" });
            }

            // check DOCTORADDRESS
            if (String.IsNullOrEmpty(model.DOCTORADDRESS))
            {

                return Json(new { error = 1, msg = "Address is not null !" });
            }
            if (model.DOCTORADDRESS.Length >= 265)
            {
                return Json(new { error = 1, msg = "Doctor address charater max lenght is 256 !" });
            }
            // check SPECIALITY
            if (String.IsNullOrEmpty(model.SPECIALITY))
            {
                return Json(new { error = 1, msg = "Specialy is not null!" });
            }
           
            
            // check WORKINGSTARTDATE , WORKINGENDDATE
            if (model.WORKINGSTARTDATE >= model.WORKINGENDDATE)
            {
                return Json(new { error = 1, msg = "Working start date smaller than Working end date !" });
            }

            //map model bind to doctor
            var mapper = MapperService.InitializeAutomapper();
            DOCTOR Doctors = mapper.Map<DoctorCreateModel, DOCTOR>(model);

            var department = _dbContext.DEPARTMENT.Find(Doctors.DEPARTMENTID);
            if (department == null) { return Json(new { error = 1, msg = "Error ! Can`t find Department !" }); }
            Doctors.DEPARTMENT = department;

            var date = DateTime.Now;
            Doctors.CREATEDBY = "Admin";
            Doctors.CREATEDDATE = date;
            Doctors.UPDATEDBY = "Admin";
            Doctors.UPDATEDDATE = date;

            //check role exists
            var role = _dbContext.ROLE.Where(r => r.ROLENAME == "Doctor").FirstOrDefault();
            if (role== null)
            {
              role = _dbContext.ROLE.Add(new ROLE()
                {
                    ROLENAME = "Doctor",
                    CREATEDBY = "Admin",
                    UPDATEDBY = "Admin",
                    CREATEDDATE = date,
                    UPDATEDDATE = date,
                    DELETEDFLAG = false,
                });
                _dbContext.SaveChanges();
               
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
                CREATEDBY = "Admin",
                UPDATEDBY = "Admin",
                CREATEDDATE = date,
                UPDATEDDATE = date,
                DELETEDFLAG = false,
            };

            var newuser = _dbContext.USER.Add(user);
            _dbContext.SaveChanges();



            Doctors.USERID = newuser.USERID;

            _dbContext.DOCTOR.Add(Doctors);
            _dbContext.SaveChanges();


            return Json(new { error = 0, msg = "ok" });
        }

        [HttpPost]
        //load doctor data for update 
        public JsonResult LoadDoctor(int DoctorId)
        {
            if (DoctorId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not delete doctor !" });
            }
            var doctor = _dbContext.DOCTOR.Where(d=>d.DOCTORID == DoctorId).Include("USER").FirstOrDefault();
            if (doctor == null)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor!" });
            }

            var mapper = MapperService.InitializeAutomapper();
            DoctorViewEditModel dt = mapper.Map<DOCTOR, DoctorViewEditModel>(doctor);

            return Json(new { error = 0, msg = "ok", doctor = dt });
        }

        //update doctor
        public JsonResult UpdateDoctor(DoctorEditModel model)
        {
            var oldDoctor = _dbContext.DOCTOR.Where(d => d.DOCTORID == model.DOCTORID).Include("USER").FirstOrDefault();
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

            string formatString = @"^[\p{L}\p{N}\s]*$"; // re!   \|!#$%&/()=?»«@£§€{}.-;'<>_,
            string patternMobile = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
            //string patternUsername = @"^[a-z0-9-]*$";

            // validation
            // check DOCTORNAME
            if (String.IsNullOrEmpty(model.DOCTORNAME))
            {
                return Json(new { error = 1, msg = "Doctor name is not null !" });
            }

            Match strname = Regex.Match(model.DOCTORNAME, formatString, RegexOptions.IgnoreCase);
            if (!strname.Success)
            {
                return Json(new { error = 1, msg = $"Doctor name does not contain any special characters !" });
            }

            if (model.DOCTORNAME.Length >= 50)
            {
                return Json(new { error = 1, msg = "Doctor name charater max lenght is 50!" });
            }
            // check DOCTORGENDER
            if (String.IsNullOrEmpty(model.DOCTORGENDER) && (model.DOCTORGENDER != "Male" || model.DOCTORGENDER != "Female"))
            {
                return Json(new { error = 1, msg = "Gender not match!" });
            }


            // check DOCTORNATIONALID
            if (String.IsNullOrEmpty(model.DOCTORNATIONALID))
            {
                return Json(new { error = 1, msg = "National ID is not null!" });
            }
            if (model.DOCTORNAME.Length >= 20)
            {
                return Json(new { error = 1, msg = "National ID charater max lenght is 20!" });
            }
            var nationidmatch = _dbContext.DOCTOR.Where(u => u.DOCTORNATIONALID == model.DOCTORNATIONALID && u.DOCTORNATIONALID != oldDoctor.DOCTORNATIONALID).ToList();
            if (nationidmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Nation ID already exists!" });
            }
            //check DOCTORDATEOFBIRTH
            if (model.DOCTORDATEOFBIRTH > DateTime.Now)
            {
                return Json(new { error = 1, msg = "Date of birth smaller than current date !" });
            }
            //check DOCTORMOBILENO
            if (!String.IsNullOrEmpty(model.DOCTORMOBILENO))
            {
                Match m = Regex.Match(model.DOCTORMOBILENO, patternMobile, RegexOptions.IgnoreCase);

                if (!m.Success) //mobile
                {
                    return Json(new { error = 1, msg = $"Mobile No error !" });
                }
            }
            else
            {
                return Json(new { error = 1, msg = "Mobile No is not null !" });

            }
            // check EMAIL
            if (String.IsNullOrEmpty(model.EMAIL))
            {
                return Json(new { error = 1, msg = "Email is not null!" });
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL && u.EMAIL != oldUser.EMAIL).ToList();
            if (emailmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Email already exists!" });
            }

            // check DOCTORADDRESS
            if (String.IsNullOrEmpty(model.DOCTORADDRESS))
            {

                return Json(new { error = 1, msg = "Address is not null !" });
            }
            if (model.DOCTORADDRESS.Length >= 265)
            {
                return Json(new { error = 1, msg = "Doctor address charater max lenght is 256 !" });
            }
            // check SPECIALITY
            if (String.IsNullOrEmpty(model.SPECIALITY))
            {
                return Json(new { error = 1, msg = "Specialy is not null!" });
            }
           

            // check WORKINGSTARTDATE , WORKINGENDDATE
            if (model.WORKINGSTARTDATE >= model.WORKINGENDDATE)
            {
                return Json(new { error = 1, msg = "Working start date smaller than Working end date !" });
            }
            //map model bind to doctor
            var mapper = MapperService.InitializeAutomapper();
            DOCTOR newDoctors = mapper.Map<DoctorEditModel, DOCTOR>(model);

            var department = _dbContext.DEPARTMENT.Find(newDoctors.DEPARTMENTID);
            if (department == null) { return Json(new { error = 1, msg = "Error ! Can`t find Department !" }); }
            newDoctors.DEPARTMENT = department;
            var date = DateTime.Now;
            
            //update old doctor info to new doctor  
            newDoctors.DOCTORID = oldDoctor.DOCTORID;
            newDoctors.USERID = oldUser.USERID;
            newDoctors.CREATEDBY = oldDoctor.CREATEDBY;
            newDoctors.CREATEDDATE = oldDoctor.CREATEDDATE;
            newDoctors.CREATEDDATE = oldDoctor.CREATEDDATE;
            newDoctors.UPDATEDBY = "Admin";
            newDoctors.UPDATEDDATE = date;
            newDoctors.DELETEDFLAG = false;

            //update user info
            oldUser.EMAIL = model.EMAIL;
            oldUser.UPDATEDDATE = date;
            oldUser.UPDATEDBY = "Admin";

            _dbContext.USER.AddOrUpdate(oldUser);
            _dbContext.DOCTOR.AddOrUpdate(newDoctors);
            _dbContext.SaveChanges();

            return Json(new { error = 0, msg = "ok" });

        }

        [HttpPost]
        public JsonResult DeleteDoctor(int DoctorId)
        {
            if (DoctorId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not delete doctor !" });
            }
            var doctor = _dbContext.DOCTOR.Find(DoctorId);
            if (doctor == null)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor !" });
            }

            doctor.DELETEDFLAG = true;

            _dbContext.DOCTOR.AddOrUpdate(doctor);
            _dbContext.SaveChanges();

            return Json(new { error = 0, msg = "ok" });
        }

        // chưa cần 
        public JsonResult ResetPassword(int DoctorId, string Password )
        {
            if (DoctorId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not delete doctor !" });
            }
            var doctor = _dbContext.DOCTOR.Where( d => d.DOCTORID == DoctorId ).Include("USER").FirstOrDefault();
            if (doctor == null && doctor.USER == null)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor !" });
            }

            if (String.IsNullOrEmpty(Password))
            {
                return Json(new { error = 1, msg = "Password is not null!" });
            }

            var hashcode = PasswordHelper.HashPassword(Password); 

            doctor.USER.PASSWORDHASH = hashcode;

            _dbContext.USER.AddOrUpdate(doctor.USER);
           // _dbContext.SaveChanges();

            return Json( new {error = 0, msg ="ok"}  );
        }
        

    }
}