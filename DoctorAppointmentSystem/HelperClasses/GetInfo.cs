using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.HelperClasses
{
    public class GetInfo
    {
        
        public static string GetFullName(string username)
        {
            string fullName = "";
            DBContext dbContext = new DBContext();
            USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
            if(user.USERTYPE.ToLower() == "patient")
            {
                PATIENT patient = dbContext.PATIENT.Where(p => p.USERID.Equals(user.USERID)).FirstOrDefault();
                fullName = patient.PATIENTNAME;
            }
            else if(user.USERTYPE.ToLower() == "doctor")
            {
                DOCTOR doctor = dbContext.DOCTOR.Where(d => d.USERID.Equals(user.USERID)).FirstOrDefault();
                fullName = doctor.DOCTORNAME;
            }
            else
            {
                fullName = "Admin";
            }
            return fullName;
        }

        public static string GetImgPath(string username)
        {
            string imgName = "default-user-image.png";
            DBContext dbContext = new DBContext();
            USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
            if (!String.IsNullOrEmpty(user.AVATARURL))
            {
                imgName = user.AVATARURL;
            }
            string imgPath = "/Uploads/" + imgName;
            return imgPath;
        }
    }
}