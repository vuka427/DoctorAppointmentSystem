using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account.ChangePassword
{
    public class ChangePasswordViewModel
    {
        public string username { get; set; }
        public string passwordRecoveryAns1 { get; set; }
        public string passwordRecoveryAns2 { get; set; }
        public string passwordRecoveryAns3 { get; set; }
        public string currentPassword { get; set; }
        public string newPassword { get; set; }

    }
}