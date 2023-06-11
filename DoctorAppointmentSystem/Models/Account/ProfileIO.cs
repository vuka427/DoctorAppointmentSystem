using DoctorAppointmentSystem.Models.Account.Profile;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account
{
    public class ProfileIO
    {
        private readonly DBContext dBContext;
        public ProfileIO()
        {
            dBContext = new DBContext();
        }

        public ProfileViewModel GetProfile(string username)
        {
            ProfileViewModel profile = new ProfileViewModel();

            USER user = dBContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
            if(user != null)
            {
                if (user.USERTYPE.ToLower().Equals("patient"))
                {
                    PATIENT patient = dBContext.PATIENT.Where(p => p.USERID.Equals(user.USERID)).FirstOrDefault();
                    if(patient != null)
                    {
                        profile.fullName = patient.PATIENTNAME.Trim();
                        profile.email = user.EMAIL.Trim();
                        profile.nationalID = patient.PATIENTNATIONALID.Trim();
                        profile.dateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("yyyy-dd-MM");
                        
                        int paraID;
                        Int32.TryParse(patient.PATIENTGENDER, out paraID);
                        SYSTEM_PARA para = dBContext.SYSTEM_PARA.Where(p => p.ID.Equals(paraID)).FirstOrDefault();

                        profile.gender = para.PARAVAL.Trim();
                        profile.phoneNumber = patient.PATIENTMOBILENO.Trim();
                        profile.address = patient.PATIENTADDRESS.Trim();
                    }
                } else if (user.USERTYPE.ToLower().Equals("doctor"))
                {
                    DOCTOR doctor = dBContext.DOCTOR.Where(d => d.USERID.Equals(user.USERID)).FirstOrDefault();
                    if (doctor != null)
                    {
                        profile.fullName = doctor.DOCTORNAME.Trim();
                        profile.email = user.EMAIL.Trim();
                        profile.nationalID = doctor.DOCTORNATIONALID.Trim();
                        profile.dateOfBirth = doctor.DOCTORDATEOFBIRTH.ToString("dd - MM - yyyy");

                        int paraID;
                        Int32.TryParse(doctor.DOCTORGENDER, out paraID);
                        SYSTEM_PARA para = dBContext.SYSTEM_PARA.Where(p => p.ID.Equals(paraID)).FirstOrDefault();

                        profile.gender = para.PARAVAL.Trim();
                        profile.phoneNumber = doctor.DOCTORMOBILENO.Trim();
                        profile.address = doctor.DOCTORADDRESS.Trim();
                    }
                }
                else
                {

                }
            }

            return profile;
        }
    }
}