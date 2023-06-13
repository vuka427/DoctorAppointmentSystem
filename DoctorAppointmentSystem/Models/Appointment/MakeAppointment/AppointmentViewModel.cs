using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Appointment.MakeAppointment
{
    public class AppointmentViewModel
    {
        public int doctorID { get; set; }
        public int patientID { get; set; }
        public int schuduleID { get; set; }
        public string doctorName { get; set; }
        public string doctorGender { get; set; }
        public string doctorSpeciality { get; set; }
        public string patientName { get; set; }
        public string patientGender { get; set; }
        public string patientDateOfBirth { get; set; }
        public string consultantTime { get; set; }
        public string workingDay { get; set; }
        public string symtoms { get; set; }
        public string existingIllness { get; set; }
        public string drugAlergies { get; set; }

    }
}