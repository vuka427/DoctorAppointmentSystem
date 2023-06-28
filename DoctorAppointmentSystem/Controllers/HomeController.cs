using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.Account;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    [AppAuthorize("Patient")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.menu = RenderMenu.RenderPatientMenu("Home");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            using (DBContext dbContext = new DBContext())
            {
                int userID = 0;
                USER user = dbContext.USER.Where(u => u.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
                if(user != null)
                {
                    userID = user.USERID;
                }
                if(userID != 0)
                {
                    PATIENT patient = dbContext.PATIENT.Where(p => p.USERID.Equals(userID)).FirstOrDefault();
                    if(patient != null)
                    {
                        int patientID = patient.PATIENTID;
                        ViewBag.allDoctors = dbContext.DOCTOR.Count();
                        ViewBag.allBooking = dbContext.APPOINTMENT.Where(a => a.PATIENTID.Equals(patientID)).Count();
                        ViewBag.newBooking = dbContext.APPOINTMENT.Where(a => a.PATIENTID.Equals(patientID) && (a.APPOIMENTSTATUS.Equals("Booked") || a.APPOIMENTSTATUS.Equals("In process"))).Count();

                        DateTime today = DateTime.Now.Date;
                        ViewBag.todaySessions = dbContext.APPOINTMENT.Where(a => a.PATIENTID.Equals(patientID) && a.APPOINTMENTDATE == today).Count();
                    }
                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Intro()
        {
            RegisterIO registerIO = new RegisterIO();
            
            if (User.Identity.IsAuthenticated)
            {   
                var user = registerIO.GetUserByUserName(User.Identity.Name);
                if (user != null)
                {
                    if(user.USERTYPE == "Patient")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (user.USERTYPE.Equals("Doctor"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Doctor" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage", new { area = "Admin" });

                    }
                }
            }
            
                return View();
        }
    }
}