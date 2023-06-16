using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage
{
    public class AppointmentViewDetailsModel
    {
        public int PATIENID { get; set; }
        public string PATIENTNAME { get; set; }
        public int DOCTORID { get; set; }
        public string DOCTORNAME { get; set; }
        public string DATEOFCONSULTATION { get; set; }
        public string APPOINTMENTDATE { get; set; }
        public string APPOIMENTSTATUS { get; set; }
        public string CLOSEDDATE { get; set; }
        public string CLOSEDBY { get; set; }
        public string SYMTOMS { get; set; }
        public string EXISTINGILLNESS { get; set; }
        public string DRUGALLERGIES { get; set; }
        public string DIAGNOSIS { get; set; }
        public string ADVICETOPATIENT { get; set; }
        public string LABTESTS { get; set; }
        public string CREATEDBY { get; set; }
        public string CREATEDDATE { get; set; }
        public string UPDATEDBY { get; set; }
        public string UPDATEDDATE { get; set; }
    }
}