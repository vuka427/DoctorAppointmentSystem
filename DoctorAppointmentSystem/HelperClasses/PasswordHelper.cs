using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DoctorAppointmentSystem.HelperClasses
{
    public class PasswordHelper
    {
        public string HashPassword(string password)
        {
            Config.Salt = "doctorappointmentsystem";
            using (var sha256 = SHA256.Create())
            {
                password = password + Config.Salt;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "");

                return hashedPassword;
            }
        }

        public bool VerifyPassword(string password, string hashedpassword)
        {
            bool verified = false;
            if (HashPassword(password).Equals(hashedpassword))
            {
                verified = true;
            }
            return verified;
        }
    }
}