using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Appointment.MakeAppointment
{
    public class ScheduleViewModel
    {
        public int scheduleID { get; set; }
        public int doctorID { get; set; }
        public string workingDay { get; set; }
        public string doctorName { get; set; }
        public string speciality { get; set; }
        public string gender { get; set; }
        public string availableTime { get; set; }
        public string consultantTime { get; set; }
    }
}