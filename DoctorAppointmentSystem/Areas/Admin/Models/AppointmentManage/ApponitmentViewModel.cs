using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage
{
    public class ApponitmentViewModel
    {
        public int APPOINTMENTID { get; set; } //1
        public int PATIENID { get; set; } //2
        public string PATIENTNAME { get; set; } //3
        public int DOCTORID { get; set; }//4
        public string DOCTORNAME { get; set; }//5
        public string APPOINTMENTDATE { get; set; }//6
        public string APPOINTMENTTIME { get; set; }//7
        public string APPOINTMENTDAY { get; set; }//8
        public string APPOIMENTSTATUS { get; set; }//9
        public string CONSULTANTTIME { get; set; } //10
        public string CLOSEDDATE { get; set; }//11
        public string CLOSEDBY { get; set; }//12
        public string CREATEDBY { get; set; }//13
        public string CREATEDDATE { get; set; }//14
        public string UPDATEDBY { get; set; }//15
        public string UPDATEDDATE { get; set; }//16
       
    }
}