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



    }
}