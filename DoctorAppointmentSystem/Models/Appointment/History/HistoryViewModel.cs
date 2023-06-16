using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Appointment.History
{
    public class HistoryViewModel
    {
        public int appointmentID { get; set; }
        public string doctorName { get; set; }
        public string appointmentDay { get; set; }
        public string appointmentDate { get; set; }
        public string appointmentStatus { get; set; }
        public string appointmentTime { get; set; }
    }
}