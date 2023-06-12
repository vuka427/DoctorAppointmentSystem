using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Areas.Admin.Models.PatientManage;
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
    public class PatientManageController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISystemParamService _sysParam;

        public PatientManageController(DBContext dbContext, IMapper mapper, ISystemParamService sysParam)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _sysParam = sysParam;
        }

        // GET: Admin/Partient
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Patient management");
            var sysParam = _sysParam.GetAllParam();
            ViewBag.genders = ViewBag.genders = new SelectList(sysParam.Where(c => c.GROUPID.Equals("Gender")).ToList(), "ID", "PARAVAL");
            

            return View();
        }

        public async Task<ActionResult> LoadPatientData(JqueryDatatableParam param)
        {
            
            var patients = await _dbContext.PATIENT.Where(d => d.DELETEDFLAG == false).Include("USER").ToListAsync();
            IEnumerable<PatientViewModel> Patients = patients.Select(dt => _mapper.GetMapper().Map<PATIENT, PatientViewModel>(dt)).ToList();

            if (!string.IsNullOrEmpty(param.sSearch)) //search
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

        // Create patient
        [HttpPost]
        public JsonResult CreatePatient(PatientCreateModel model)
        {
            model.PATIENTNAME = model.PATIENTNAME != null ? model.PATIENTNAME.Trim() : model.PATIENTNAME;
            model.PATIENTNATIONALID = model.PATIENTNATIONALID != null ? model.PATIENTNATIONALID.Trim() : model.PATIENTNATIONALID;
            model.PATIENTADDRESS = model.PATIENTADDRESS != null ? model.PATIENTADDRESS.Trim() : model.PATIENTADDRESS;

            string formatString = @"^[\p{L}\p{N}\s]*$"; // re!   \|!#$%&/()=?»«@£§€{}.-;'<>_,
            string patternMobile = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
            string patternUsername = @"^[a-z0-9-]*$";

            // validation
            // check PATIENTNAME
            if (String.IsNullOrEmpty(model.PATIENTNAME))
            {
                return Json(new { error = 1, msg = "Patient name is not null !" });
            }

            Match strname = Regex.Match(model.PATIENTNAME, formatString, RegexOptions.IgnoreCase);
            if (!strname.Success)
            {
                return Json(new { error = 1, msg = $"Patient name does not contain any special characters !" });
            }

            if (model.PATIENTNAME.Length >= 50)
            {
                return Json(new { error = 1, msg = "Patient name charater max lenght is 50!" });
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
            if (usermatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Username already exists!" });
            }
            // check PASSWORD

            if (String.IsNullOrEmpty(model.PASSWORD))
            {

                return Json(new { error = 1, msg = "Password is not null!" });
            }

            // check PATIENTGENDER
            var sysParam = _sysParam.GetAllParam();
            if (model.PATIENTGENDER <= 0)
            {
                return Json(new { error = 1, msg = "Gender not match!" });
            }
            else
            {
                var result = sysParam.Where(pr => pr.GROUPID == "Gender" && pr.ID == model.PATIENTGENDER).FirstOrDefault();
                if (result == null)
                {
                    return Json(new { error = 1, msg = "Gender not match!" });
                }
            }

            // check DOCTORNATIONALID
            if (String.IsNullOrEmpty(model.PATIENTNATIONALID))
            {
                return Json(new { error = 1, msg = "National ID is not null!" });
            }
            if (model.PATIENTNAME.Length >= 20)
            {
                return Json(new { error = 1, msg = "National ID charater max lenght is 20!" });
            }
            var nationidmatch = _dbContext.DOCTOR.Where(u => u.DOCTORNATIONALID == model.PATIENTNATIONALID).ToList();
            if (nationidmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Nation ID already exists!" });
            }
            //check DOCTORDATEOFBIRTH
            if (model.PATIENTDATEOFBIRTH > DateTime.Now)
            {
                return Json(new { error = 1, msg = "Date of birth smaller than current date !" });
            }
            //check DOCTORMOBILENO
            if (!String.IsNullOrEmpty(model.PATIENTMOBILENO))
            {
                Match m = Regex.Match(model.PATIENTMOBILENO, patternMobile, RegexOptions.IgnoreCase);

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

            // check PATIENTADDRESS
            if (String.IsNullOrEmpty(model.PATIENTADDRESS))
            {

                return Json(new { error = 1, msg = "Address is not null !" });
            }
            if (model.PATIENTADDRESS.Length >= 265)
            {
                return Json(new { error = 1, msg = "Patient address charater max lenght is 256 !" });
            }
           
            //map model bind to doctor
            
            PATIENT Patient = _mapper.GetMapper().Map<PatientCreateModel, PATIENT>(model);

            var date = DateTime.Now;
            Patient.CREATEDBY = "Admin";
            Patient.CREATEDDATE = date;
            Patient.UPDATEDBY = "Admin";
            Patient.UPDATEDDATE = date;

            //check role exists
            var role = _dbContext.ROLE.Where(r => r.ROLENAME == "Patient").FirstOrDefault();
            if (role == null)
            {
                role = _dbContext.ROLE.Add(new ROLE()
                {
                    ROLENAME = "Patient",
                    CREATEDBY = "Admin",
                    UPDATEDBY = "Admin",
                    CREATEDDATE = date,
                    UPDATEDDATE = date,
                    DELETEDFLAG = false,
                });
                try { 
                    _dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    //write error log
                }


            }

            var hashcode = PasswordHelper.HashPassword(model.PASSWORD);

            USER user = new USER()
            {

                USERNAME = model.USERNAME,
                ROLEID = role.ROLEID,
                PASSWORDHASH = hashcode,
                EMAIL = model.EMAIL,
                USERTYPE = "Patient",//Patient, Doctor , Admin 
                LOGINRETRYCOUNT = 0,
                STATUS = true,
                CREATEDBY = "Admin",
                UPDATEDBY = "Admin",
                CREATEDDATE = date,
                UPDATEDDATE = date,
                DELETEDFLAG = false,
            };

            var newuser = _dbContext.USER.Add(user);
           

            Patient.USERID = newuser.USERID;

            _dbContext.PATIENT.Add(Patient);
            try { 
                _dbContext.SaveChanges(); 
            }
            catch (Exception ex)
            {
                //write error log
            }



            return Json(new { error = 0, msg = "ok" });
        }


        [HttpPost]
        //load patient data for update 
        public JsonResult LoadPatient(int PatientId)
        {
            if (PatientId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not find patient !" });
            }
            var patient = _dbContext.PATIENT.Where(p=>p.PATIENTID == PatientId).Include("USER").FirstOrDefault();
            if (patient == null)
            {
                return Json(new { error = 1, msg = "Error! do not find patient !" });
            }

           
            PatientViewEditModel dt = _mapper.GetMapper().Map<PATIENT, PatientViewEditModel>(patient);

            return Json(new { error = 0, msg = "ok", patient = dt });
        }


        // Create patient 
        [HttpPost]
        public JsonResult UpdatePatient(PatientEditModel model)
        {
            var oldPatient = _dbContext.PATIENT.Where(p=>p.PATIENTID == model.PATIENTID).Include("USER").FirstOrDefault();
            USER oldUser = null;

            if (oldPatient == null)
            {
                return Json(new { error = 1, msg = "Error ! Can`t find Doctor !" });
            }
            else
            {
                if (oldPatient.USER == null)
                {
                    return Json(new { error = 1, msg = "Error ! Can`t find Doctor !" });
                }
                oldUser = oldPatient.USER;
            }

            model.PATIENTNAME = model.PATIENTNAME != null ? model.PATIENTNAME.Trim() : model.PATIENTNAME;
            model.PATIENTNATIONALID = model.PATIENTNATIONALID != null ? model.PATIENTNATIONALID.Trim() : model.PATIENTNATIONALID;
            model.PATIENTADDRESS = model.PATIENTADDRESS != null ? model.PATIENTADDRESS.Trim() : model.PATIENTADDRESS;

            string formatString = @"^[\p{L}\p{N}\s]*$"; // re!   \|!#$%&/()=?»«@£§€{}.-;'<>_,
            string patternMobile = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
            string patternUsername = @"^[a-z0-9-]*$";

            // validation
            // check PATIENTNAME
            if (String.IsNullOrEmpty(model.PATIENTNAME))
            {
                return Json(new { error = 1, msg = "Patient name is not null !" });
            }

            Match strname = Regex.Match(model.PATIENTNAME, formatString, RegexOptions.IgnoreCase);
            if (!strname.Success)
            {
                return Json(new { error = 1, msg = $"Patient name does not contain any special characters !" });
            }

            if (model.PATIENTNAME.Length >= 50)
            {
                return Json(new { error = 1, msg = "Patient name charater max lenght is 50!" });
            }

            // check PATIENTGENDER
            var sysParam = _sysParam.GetAllParam();
            if (model.PATIENTGENDER <= 0)
            {
                return Json(new { error = 1, msg = "Gender not match!" });
            }
            else
            {
                var result = sysParam.Where(pr => pr.GROUPID == "Gender" && pr.ID == model.PATIENTGENDER).FirstOrDefault();
                if (result == null)
                {
                    return Json(new { error = 1, msg = "Gender not match!" });
                }
            }

            // check DOCTORNATIONALID
            if (String.IsNullOrEmpty(model.PATIENTNATIONALID))
            {
                return Json(new { error = 1, msg = "National ID is not null!" });
            }
            if (model.PATIENTNAME.Length >= 20)
            {
                return Json(new { error = 1, msg = "National ID charater max lenght is 20!" });
            }
            var nationidmatch = _dbContext.PATIENT.Where(u => u.PATIENTNATIONALID == model.PATIENTNATIONALID && u.PATIENTID != oldPatient.PATIENTID ).ToList();
            if (nationidmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Nation ID already exists!" });
            }
            //check DOCTORDATEOFBIRTH
            if (model.PATIENTDATEOFBIRTH > DateTime.Now)
            {
                return Json(new { error = 1, msg = "Date of birth smaller than current date !" });
            }
            //check DOCTORMOBILENO
            if (!String.IsNullOrEmpty(model.PATIENTMOBILENO))
            {
                Match m = Regex.Match(model.PATIENTMOBILENO, patternMobile, RegexOptions.IgnoreCase);

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
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL && u.USERID != oldUser.USERID).ToList();
            if (emailmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Email already exists!" });
            }

            // check PATIENTADDRESS
            if (String.IsNullOrEmpty(model.PATIENTADDRESS))
            {

                return Json(new { error = 1, msg = "Address is not null !" });
            }
            if (model.PATIENTADDRESS.Length >= 265)
            {
                return Json(new { error = 1, msg = "Patient address charater max lenght is 256 !" });
            }

            //map model bind to doctor
            
            PATIENT newPatient = _mapper.GetMapper().Map<PatientEditModel, PATIENT>(model);

            //update old doctor info to new doctor  
            var date = DateTime.Now;
            newPatient.USERID = oldPatient.USERID;
            newPatient.CREATEDBY = oldPatient.CREATEDBY;
            newPatient.CREATEDDATE =oldPatient.CREATEDDATE;
            newPatient.UPDATEDBY = "Admin";
            newPatient.UPDATEDDATE = date;
            newPatient.DELETEDFLAG = oldPatient.DELETEDFLAG;

            //update user info
            oldUser.EMAIL = model.EMAIL;
            oldUser.UPDATEDBY = "Admin";
            oldUser.UPDATEDDATE = date;
           

            _dbContext.USER.AddOrUpdate(oldUser);
            _dbContext.PATIENT.AddOrUpdate(newPatient);
            _dbContext.SaveChanges();


            return Json(new { error = 0, msg = "ok" });
        }
        //delete patient
        [HttpPost]
        public JsonResult DeletePatient(int PatientId)
        {
            if (PatientId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not delete patient !" });
            }
            var patient = _dbContext.PATIENT.Where(p=>p.PATIENTID == PatientId).Include("USER").FirstOrDefault();
            if (patient == null)
            {
                return Json(new { error = 1, msg = "Error! do not find patienr !" });
            }

            patient.DELETEDFLAG = true;
            patient.USER.DELETEDFLAG = true;

            _dbContext.PATIENT.AddOrUpdate(patient);
            _dbContext.USER.AddOrUpdate(patient.USER);
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