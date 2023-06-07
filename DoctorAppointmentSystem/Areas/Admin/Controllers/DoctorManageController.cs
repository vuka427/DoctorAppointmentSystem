using Antlr.Runtime;
using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    public class DoctorManageController : Controller
    {
        
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapper _mapper;

        public DoctorManageController(DBContext dbContext, ISystemParamService sysParam, IMapper mapper)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
        }



        // GET: Admin/DoctorManage
        public ActionResult Index()
        {
            RenderAdminMenu menu = new RenderAdminMenu();
            ViewBag.menu = menu.RenderMenu("Doctor management");
            var sysParam = _sysParam.GetAllParam();

            ViewBag.genders = ViewBag.genders = new SelectList(sysParam.Where(c => c.GROUPID.Equals("Gender")).ToList(), "ID", "PARAVAL");
            ViewBag.department = new SelectList( _dbContext.DEPARTMENT.ToList(), "DEPARTMENTID", "DEPARTMENTNAME");
            return View();
        }

        //load data doctors  to jquery datatable
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
            }, JsonRequestBehavior.AllowGet);

        }

        // Create doctor
        [HttpPost]
        public JsonResult CreateDoctor(DoctorCreateModel model)
        {
            model.DOCTORNAME = model.DOCTORNAME != null ? model.DOCTORNAME.Trim() : model.DOCTORNAME;
            model.DOCTORNATIONALID = model.DOCTORNATIONALID != null ? model.DOCTORNATIONALID.Trim() : model.DOCTORNATIONALID;
            model.DOCTORADDRESS = model.DOCTORADDRESS != null ? model.DOCTORADDRESS.Trim() : model.DOCTORADDRESS;

            string patternName = @"^[\p{L}\p{N}\s]*$"; // re!   \|!#$%&/()=?»«@£§€{}.-;'<>_,
            string patternMobile = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
            string patternUsername = @"^[a-z0-9-]*$";
            string patternPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$";

            // validation
            // check DOCTORNAME
            if (String.IsNullOrEmpty(model.DOCTORNAME))
            {
                return Json(new { error = 1, msg = "Doctor name is required !" });
            }

            Match strname = Regex.Match(model.DOCTORNAME, patternName, RegexOptions.IgnoreCase);
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
                return Json(new { error = 1, msg = "Username is required !" });
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

            var sysParam = _sysParam.GetAllParam();
            if (model.DOCTORGENDER<=0)
            {
                return Json(new { error = 1, msg = "Gender not match!" });
            }
            else
            {
                var result = sysParam.Where(pr => pr.GROUPID == "Gender" && pr.ID == model.DOCTORGENDER).FirstOrDefault();
                if (result == null)
                {
                    return Json(new { error = 1, msg = "Gender not match!" });
                }
            }

            // check PASSWORD

            if (String.IsNullOrEmpty(model.PASSWORD) )
            {

                return Json(new { error = 1, msg = "Password is required!" });
            }

            Match strpawd = Regex.Match(model.PASSWORD, patternPassword, RegexOptions.IgnoreCase);
            if (!strpawd.Success)
            {
                return Json(new { error = 1, msg = @"Password charater at least one uppercase letter, one lowercase letter, one number and one special character: [a - z],[A - Z],[0 - 9],[@$!%*?&]" });
            }
            // check DOCTORNATIONALID
            if (String.IsNullOrEmpty(model.DOCTORNATIONALID))
            {
                return Json(new { error = 1, msg = "National ID is required!" });
            }
            if (model.DOCTORNATIONALID.Length >= 20)
            {
                return Json(new { error = 1, msg = "National ID charater max lenght is 20!" });
            }
            var nationidmatch = _dbContext.DOCTOR.Where(u => u.DOCTORNATIONALID == model.DOCTORNATIONALID).ToList();
            if (nationidmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Nation ID already exists !" });
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
                    return Json(new { error = 1, msg = $"Mobile number is required !" });
                }
            }
            else
            {
                return Json(new { error = 1, msg = "Mobile number is required !" });

            }
            // check EMAIL
            if (String.IsNullOrEmpty(model.EMAIL))
            {
                return Json(new { error = 1, msg = "Email is required!"});
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL).ToList();
            if (emailmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Email already exists!" });
            }

            // check DOCTORADDRESS
            if (String.IsNullOrEmpty(model.DOCTORADDRESS))
            {

                return Json(new { error = 1, msg = "Address is required !" });
            }
            if (model.DOCTORADDRESS.Length >= 265)
            {
                return Json(new { error = 1, msg = "Doctor address charater max lenght is 256 !" });
            }
            // check SPECIALITY
            if (String.IsNullOrEmpty(model.SPECIALITY))
            {
                return Json(new { error = 1, msg = "Specialy is required!" });
            }
            if (model.SPECIALITY.Length >= 265)
            {
                return Json(new { error = 1, msg = "Specialy charater max lenght is 256 !" });
            }

            // check WORKINGSTARTDATE , WORKINGENDDATE
            if (model.WORKINGSTARTDATE >= model.WORKINGENDDATE)
            {
                return Json(new { error = 1, msg = "Working start date smaller than Working end date !" });
            }

            //map model bind to doctor
           
            DOCTOR Doctors = _mapper.GetMapper().Map<DoctorCreateModel, DOCTOR>(model);

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

                try
                {
                    _dbContext.SaveChanges();

                }
                catch (Exception ex)
                {
                    //write error log
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
                CREATEDBY = "Admin",
                UPDATEDBY = "Admin",
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
            }

          


            return Json(new { error = 0, msg = "ok" });
        }


        [HttpPost]
        //load doctor data for update 
        public JsonResult LoadDoctor(int DoctorId)
        {
            if (DoctorId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor !" });
            }
            var doctor = _dbContext.DOCTOR.Where(d=>d.DOCTORID == DoctorId).Include("USER").FirstOrDefault();
            if (doctor == null)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor!" });
            }

           
            DoctorViewEditModel dt = _mapper.GetMapper().Map<DOCTOR, DoctorViewEditModel>(doctor);

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
                return Json(new { error = 1, msg = "Doctor name is required !" });
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

            var sysParam = _sysParam.GetAllParam();
            if (model.DOCTORGENDER <= 0)
            {
                return Json(new { error = 1, msg = "Gender not match!" });
            }
            else
            {
                var result = sysParam.Where(pr => pr.GROUPID == "Gender" && pr.ID == model.DOCTORGENDER).FirstOrDefault();
                if (result == null)
                {
                    return Json(new { error = 1, msg = "Gender not match!" });
                }
            }


            // check DOCTORNATIONALID
            if (String.IsNullOrEmpty(model.DOCTORNATIONALID))
            {
                return Json(new { error = 1, msg = "National ID is required!" });
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
                return Json(new { error = 1, msg = "Mobile No is required !" });

            }
            // check EMAIL
            if (String.IsNullOrEmpty(model.EMAIL))
            {
                return Json(new { error = 1, msg = "Email is required !" });
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL && u.EMAIL != oldUser.EMAIL).ToList();
            if (emailmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Email already exists!" });
            }

            // check DOCTORADDRESS
            if (String.IsNullOrEmpty(model.DOCTORADDRESS))
            {

                return Json(new { error = 1, msg = "Address is required !" });
            }
            if (model.DOCTORADDRESS.Length >= 265)
            {
                return Json(new { error = 1, msg = "Doctor address charater max lenght is 256 !" });
            }
            // check SPECIALITY
            if (String.IsNullOrEmpty(model.SPECIALITY))
            {
                return Json(new { error = 1, msg = "Specialy is required !" });
            }
           
            // check WORKINGSTARTDATE , WORKINGENDDATE
            if (model.WORKINGSTARTDATE >= model.WORKINGENDDATE)
            {
                return Json(new { error = 1, msg = "Working start date smaller than Working end date !" });
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
            newDoctors.UPDATEDBY = "Admin";
            newDoctors.UPDATEDDATE = date;
            newDoctors.DELETEDFLAG = false;

            //update user info
            oldUser.EMAIL = model.EMAIL;
            oldUser.UPDATEDDATE = date;
            oldUser.UPDATEDBY = "Admin";

            _dbContext.USER.AddOrUpdate(oldUser);
            _dbContext.DOCTOR.AddOrUpdate(newDoctors);
            try
            {
                _dbContext.SaveChanges();
            } catch(Exception ex)
            {
                //write error log
            }
            

            return Json(new { error = 0, msg = "ok" });

        }

        //delete doctor
        [HttpPost]
        public JsonResult DeleteDoctor(int DoctorId)
        {
            if (DoctorId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not delete doctor !" });
            }
            var doctor = _dbContext.DOCTOR.Where(d => d.DOCTORID == DoctorId).FirstOrDefault();
            if (doctor == null)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor !" });
            }

            doctor.DELETEDFLAG = true;
            doctor.USER.DELETEDFLAG = true;

            _dbContext.DOCTOR.AddOrUpdate(doctor);
            _dbContext.USER.AddOrUpdate(doctor.USER);
            try {
                _dbContext.SaveChanges(); 
            }
            catch (Exception ex)
            {
                //write error log
            }


            return Json(new { error = 0, msg = "ok" });
        }



    }
}