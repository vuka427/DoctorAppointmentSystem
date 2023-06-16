﻿using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Profile
{
    public class ProfileIO
    {
        private readonly DBContext dbContext;
        public ProfileIO()
        {
            dbContext = new DBContext();
        }

        public ProfileViewModel GetProfile(string username)
        {
            ProfileViewModel profile = new ProfileViewModel();

            USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
            if (user != null)
            {
                if (user.USERTYPE.ToLower().Equals("patient"))
                {
                    PATIENT patient = dbContext.PATIENT.Where(p => p.USERID.Equals(user.USERID)).FirstOrDefault();
                    if (patient != null)
                    {
                        profile.fullName = patient.PATIENTNAME.Trim();
                        profile.email = user.EMAIL.Trim();
                        profile.nationalID = patient.PATIENTNATIONALID.Trim();
                        profile.dateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("yyyy-MM-dd");
                        profile.gender = patient.PATIENTGENDER;
                        profile.phoneNumber = patient.PATIENTMOBILENO.Trim();
                        profile.address = patient.PATIENTADDRESS.Trim();
                        profile.profilePicture = user.AVATARURL != null ? user.AVATARURL.Trim() : "default-user-image.png";
                    }
                }
                else if (user.USERTYPE.ToLower().Equals("doctor"))
                {
                    DOCTOR doctor = dbContext.DOCTOR.Where(d => d.USERID.Equals(user.USERID)).FirstOrDefault();
                    if (doctor != null)
                    {
                        profile.fullName = doctor.DOCTORNAME.Trim();
                        profile.email = user.EMAIL.Trim();
                        profile.nationalID = doctor.DOCTORNATIONALID.Trim();
                        profile.dateOfBirth = doctor.DOCTORDATEOFBIRTH.ToString("yyyy-MM-dd");
                        profile.gender = doctor.DOCTORGENDER;
                        profile.phoneNumber = doctor.DOCTORMOBILENO.Trim();
                        profile.address = doctor.DOCTORADDRESS.Trim();
                        profile.profilePicture = user.AVATARURL != null ? user.AVATARURL.Trim() : "default-user-image.png";
                    }
                }
                else
                {

                }
            }

            return profile;
        }

        public bool VerifiedData(ProfileViewModel profileData, string username, out string message)
        {
            USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
            PATIENT patient = dbContext.PATIENT.Where(p => p.USERID.Equals(user.USERID)).FirstOrDefault();

            bool success = false;

            bool email_exists = dbContext.USER.Where(u => u.EMAIL.Equals(profileData.email)).Count() == 0 ? false : true;
            bool nationalID_exists = dbContext.PATIENT.Where(p => p.PATIENTNATIONALID.Equals(profileData.nationalID)).Count() == 0 ? false : true;

            if (user != null)
            {
                if(profileData.fullName.Trim() == null)
                {
                    message = "Please enter your full name!";
                }
                else if(profileData.email.Trim() == null)
                {
                    message = "Please enter your username!";
                }
                else if(email_exists && !profileData.email.Trim().Equals(user.EMAIL.Trim()))
                {
                    message = "Email address already exists! Please choose another!";
                }
                else if (profileData.nationalID.Trim() == null)
                {
                    message = "Please enter your National ID!";
                }
                else if(nationalID_exists && !profileData.nationalID.Trim().Equals(patient.PATIENTNATIONALID.Trim()))
                {
                    message = "National ID already exists! Please choose another!";
                }
                else if (String.IsNullOrEmpty(profileData.dateOfBirth))
                {
                    message = "Please enter your date of birth!";
                }
                else if (profileData.gender.ToString().Trim() == "0")
                {
                    message = "Please select gender!";
                }
                else if (String.IsNullOrEmpty(profileData.phoneNumber))
                {
                    message = "Please enter your phone number!";
                }
                else if (String.IsNullOrEmpty(profileData.address))
                {
                    message = "Please enter your address!";
                }
                else
                {
                    message = "Updated successfully profile!";
                    success = true;
                }

            }
            else
            {
                message = "404! Not found User!";
            }

            return success;
        }

        public void UpdateProfile(ProfileViewModel profileData, string username)
        {
            USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
            PATIENT patient = dbContext.PATIENT.Where(p => p.USERID.Equals(user.USERID)).FirstOrDefault();

            user.EMAIL = profileData.email.Trim();
            patient.PATIENTNAME = profileData.fullName.Trim();
            patient.PATIENTNATIONALID = profileData.nationalID.Trim();
            patient.PATIENTDATEOFBIRTH = DateTime.Parse(profileData.dateOfBirth);
            patient.PATIENTGENDER = profileData.gender;
            patient.PATIENTMOBILENO = profileData.phoneNumber.Trim();
            patient.PATIENTADDRESS = profileData.address.Trim();

            dbContext.SaveChanges();
        }
    }
}