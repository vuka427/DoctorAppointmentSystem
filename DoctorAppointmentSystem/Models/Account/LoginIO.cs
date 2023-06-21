using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.Account.Login;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account
{
    public class LoginIO
    {
        private readonly LoginViewModel model;
        private readonly DBContext dbContext;

        public LoginIO()
        {
            this.model = new LoginViewModel();
            this.dbContext = new DBContext();
        }

        public USER GetUser(string username)
        {
            return dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
        }

        public void UserRedirects(USER user, out string area, out string controller, out string action)
        {
            string userType = user.USERTYPE.ToLower();
            if (userType.Equals("patient"))
            {
                area = "";
                action = "Index";
                controller = "Home";
            }
            else if (userType.Equals("doctor"))
            {
                area = "Doctor";
                action = "Index";
                controller = "Home";
            }
            else
            {
                area = "Admin";
                action = "Index";
                controller = "Manage";
            }

            try
            {
                user.LASTLOGIN = DateTime.Now;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "UserRedirects";
                string sEventType = "U";
                string sInsBy = user.USERNAME;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
        }
    }
}