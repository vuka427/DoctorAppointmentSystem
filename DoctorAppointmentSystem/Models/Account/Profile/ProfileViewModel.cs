using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account.Profile
{
    public class ProfileViewModel
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string nationalID { get; set; }
        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
    }
}