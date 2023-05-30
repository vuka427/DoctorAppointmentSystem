using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.HelperClasses
{
    public class Config
    {
        public static string Salt
        {
            get
            {
                return Salt == null ? string.Empty : Salt;
            }
            set
            {
                Salt = value;
            }
        }
    }
}