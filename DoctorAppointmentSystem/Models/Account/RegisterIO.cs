
using DoctorAppointmentSystem.HelperClasses;
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

        public bool VerifyUserInfo(UserViewModel user, out string message)
        {
            user.passwordRecoveryAns1 = user.passwordRecoveryAns1 != null ? user.passwordRecoveryAns1.Trim() : user.passwordRecoveryAns1;
            user.passwordRecoveryAns2 = user.passwordRecoveryAns2 != null ? user.passwordRecoveryAns2.Trim() : user.passwordRecoveryAns2;
            user.passwordRecoveryAns3 = user.passwordRecoveryAns3 != null ? user.passwordRecoveryAns3.Trim() : user.passwordRecoveryAns3;

            bool success = false;
            message = "";

            // Check if the username is already in use
            bool username_exists = dbContext.USER.Where(u => u.USERNAME.Equals(user.username)).FirstOrDefault() == null ? false : true;

            //Check if the email is already in use
            bool email_exists = dbContext.USER.Where(u => u.EMAIL.Equals(user.email)).FirstOrDefault() == null ? false : true;

            if (String.IsNullOrEmpty(user.username))
            {
                message = "Please enter your username!";
            }
            else if (username_exists)
            {
                message = "The name already exists! Please choose another!";
            }
            else if (String.IsNullOrEmpty(user.password))
            {
                message = "Please enter your password!";
            }
            else if (String.IsNullOrEmpty(user.email))
            {
                message = "Please enter your email!";
            }
            else if (email_exists)
            {
                message = "The email address already exists! Please choose another!";
            }
            else if (user.passwordRecoveryQue1.ToString().Trim() == "0" || user.passwordRecoveryQue2.ToString().Trim() == "0"|| user.passwordRecoveryQue3.ToString().Trim() == "0")
            {
                message = "Please select all authentication questions!";
            }
            else if (String.IsNullOrEmpty(user.passwordRecoveryAns1)|| String.IsNullOrEmpty(user.passwordRecoveryAns2)|| String.IsNullOrEmpty(user.passwordRecoveryAns3))
            {
                message = "Please answer all authentication questions!";
            }
            else
            {
                success = true;
            }
            return success;
        }

        public bool VerifyPatientInfo(PatientViewModel patient, out string message)
        {
            patient.fullName = patient.fullName != null ? patient.fullName.Trim() : patient.fullName;
            patient.address = patient.address != null ? patient.address.Trim() : patient.address;

            bool success = false;
            message = "";

            //Check if the email is already in use
            bool nationalID_exists = dbContext.PATIENT.Where(p => p.PATIENTNATIONALID.Equals(patient.nationalID)).FirstOrDefault() == null ? false : true;

            if (String.IsNullOrEmpty(patient.fullName))
            {
                message = "Please enter your full name!";
            }
            else if (String.IsNullOrEmpty(patient.nationalID))
            {
                message = "Please enter your National ID!";
            }
            else if (nationalID_exists)
            {
                message = "The national ID already exists! Please choose another!";
            }
            else if (String.IsNullOrEmpty(patient.dateOfBirth))
            {
                message = "Please enter your date of birth!";
            }
            else if (patient.gender.ToString().Trim() == "0")
            {
                message = "Please select gender!";
            }
            else if (String.IsNullOrEmpty(patient.phoneNumber))
            {
                message = "Please enter your phone number!";
            }
            else if (String.IsNullOrEmpty(patient.address))
            {
                message = "Please enter your address!";
            }
            else
            {
                success = true;
            }
            return success;
        }

        int GetUserID(string username)
        {
            return dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault().USERID;
        }

        void CreateNewUser(UserViewModel uvm)
        {
            USER user = new USER();

            user.ROLEID = 1303;
            user.USERNAME = uvm.username;
            user.PASSWORDHASH = PasswordHelper.HashPassword(uvm.password);
            user.EMAIL = uvm.email;
            user.LASTLOGIN = DateTime.Now;
            user.USERTYPE = "patient";
            user.PASSWORDRECOVERYQUE1 = uvm.passwordRecoveryQue1;
            user.PASSWORDRECOVERYANS1 = uvm.passwordRecoveryAns1;
            user.PASSWORDRECOVERYQUE2 = uvm.passwordRecoveryQue2;
            user.PASSWORDRECOVERYANS2 = uvm.passwordRecoveryAns2;
            user.PASSWORDRECOVERYQUE3 = uvm.passwordRecoveryQue3;
            user.PASSWORDRECOVERYANS3 = uvm.passwordRecoveryAns3;
            user.STATUS = true;
            user.LOGINRETRYCOUNT = 0;
            user.LOGINLOCKDATE = null;
            user.CREATEDBY = uvm.username;
            user.CREATEDDATE = DateTime.Now;
            user.UPDATEDBY = uvm.username;
            user.UPDATEDDATE = DateTime.Now;
            user.DELETEDFLAG = false;
            user.AVATARURL = null;

            dbContext.USER.Add(user);
            dbContext.SaveChanges();
        }

        void CreateNewPatient(PatientViewModel pvm, string username)
        {
            PATIENT patient = new PATIENT();

            patient.PATIENTNAME = pvm.fullName;
            patient.USERID = GetUserID(username);
            patient.PATIENTNATIONALID = pvm.nationalID;
            patient.PATIENTGENDER = pvm.gender;
            patient.PATIENTMOBILENO = pvm.phoneNumber;
            patient.PATIENTADDRESS = pvm.address;
            patient.PATIENTDATEOFBIRTH = DateTime.Parse(pvm.dateOfBirth);
            patient.CREATEDBY = username;
            patient.CREATEDDATE = DateTime.Now;
            patient.UPDATEDBY = username;
            patient.UPDATEDDATE = DateTime.Now;
            patient.DELETEDFLAG = false;

            dbContext.PATIENT.Add(patient);
            dbContext.SaveChanges();
        }

        public void CreateNewAccount(UserViewModel user, PatientViewModel patient , out string message)
        {
            message = "Account successfully created!";
            CreateNewUser(user);
            CreateNewPatient(patient, user.username);
        }
    }
}