//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoctorAppointmentSystem.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class APPOINTMENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public APPOINTMENT()
        {
            this.APPOINTMENT_NOTE = new HashSet<APPOINTMENT_NOTE>();
        }
    
        public int APPOINTMENTID { get; set; }
        public int PATIENTID { get; set; }
        public int DOCTORID { get; set; }
        public int SCHEDULEID { get; set; }
        public int CONSULTANTTYPE { get; set; }
        public int MODEOFCONSULTANT { get; set; }
        public System.DateTime DATEOFCONSULTATION { get; set; }
        public Nullable<System.DateTime> APPOINTMENTDATE { get; set; }
        public string APPOIMENTSTATUS { get; set; }
        public Nullable<System.DateTime> CLOSEDDATE { get; set; }
        public string CLOSEDBY { get; set; }
        public string SYMTOMS { get; set; }
        public string EXISTINGILLNESS { get; set; }
        public string DRUGALLERGIES { get; set; }
        public string NOTE { get; set; }
        public string CASENOTE { get; set; }
        public string DIAGNOSIS { get; set; }
        public string ADVICETOPATIENT { get; set; }
        public string LABTESTS { get; set; }
        public string CREATEDBY { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public string UPDATEDBY { get; set; }
        public Nullable<System.DateTime> UPDATEDDATE { get; set; }
        public bool DELETEDFLAG { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APPOINTMENT_NOTE> APPOINTMENT_NOTE { get; set; }
        public virtual PATIENT PATIENT { get; set; }
        public virtual SCHEDULE SCHEDULE { get; set; }
    }
}
