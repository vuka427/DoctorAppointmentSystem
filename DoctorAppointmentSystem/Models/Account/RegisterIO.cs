
using DoctorAppointmentSystem.Models.Account.Register;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account
{
    public class RegisterIO
    {
        private readonly AuthQuestionViewModel authQuestion;
        private readonly PatientViewModel patient;
        private readonly UserViewModel user;
        private readonly GenderViewModel gender;
        private readonly DBContext dbContext;

        public RegisterIO()
        {
            this.authQuestion = new AuthQuestionViewModel();
            this.patient = new PatientViewModel();
            this.user = new UserViewModel();
            this.gender = new GenderViewModel();
            this.dbContext = new DBContext();
        }

        //Get authencation question to generate it on register form
        public List<AuthQuestionViewModel> GenerateAuthQuestion()
        {
            List<SYSTEM_PARA> system_paraList = dbContext.SYSTEM_PARA.Where(c => c.GROUPID.Equals("authQuestion")).ToList();
            List<AuthQuestionViewModel> questionList = new List<AuthQuestionViewModel>();
            foreach(SYSTEM_PARA item in system_paraList)
            {
                AuthQuestionViewModel question = new AuthQuestionViewModel(item.ID, item.PARAID, item.GROUPID, item.PARAVAL);
                questionList.Add(question);
            }
            return questionList;
        }

        //Get gender to generate it on register form
        public List<GenderViewModel> GenerateGender()
        {
            List<SYSTEM_PARA> system_paraList = dbContext.SYSTEM_PARA.Where(c => c.GROUPID.Equals("Gender")).ToList();
            List<GenderViewModel> genderList = new List<GenderViewModel>();
            foreach(SYSTEM_PARA item in system_paraList)
            {
                GenderViewModel gender = new GenderViewModel(item.ID, item.PARAID, item.GROUPID, item.PARAVAL);
                genderList.Add(gender);
            }
            return genderList;
        }

        public bool CreateNewUser(UserViewModel user, out string message)
        {
            bool success = false;
            message = "";
            if (String.IsNullOrEmpty(user.username.Trim()))
            {
                message = "Please enter your username!";
                return success;
            }
            if (String.IsNullOrEmpty(user.password.Trim()))
            {
                message = "Please enter your password!";
                return success;
            }
            return success;
        }
    }
}