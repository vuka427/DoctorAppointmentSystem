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
            using (DBContext dbContext = new DBContext())
            {
                string fullName = "";

                USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
                if (user.USERTYPE.ToLower() == "patient")
                {
                    PATIENT patient = dbContext.PATIENT.Where(p => p.USERID.Equals(user.USERID)).FirstOrDefault();
                    fullName = patient.PATIENTNAME;
                }
                else if (user.USERTYPE.ToLower() == "doctor")
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
        }

        public static int GetUserID(string username)
        {
            using (DBContext dbContext = new DBContext())
            {
                USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
                if(user != null)
                {
                    return user.USERID;
                }
                return 0;
            }
        }

        public static int GetPatientID(int userID)
        {
            using (DBContext dbContext = new DBContext())
            {
                PATIENT patient = dbContext.PATIENT.Where(p => p.USERID.Equals(userID)).FirstOrDefault();
                if (patient != null)
                {
                    return patient.PATIENTID;
                }
                return 0;
            }
        }

        public static string GetImgPath(string username)
        {
            using (DBContext dbContext = new DBContext())
            {
                string imgName = "default-user-image.png";
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
}