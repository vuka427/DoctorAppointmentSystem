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
    
    public partial class SCHEDULE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SCHEDULE()
        {
            this.APPOINTMENT = new HashSet<APPOINTMENT>();
        }
    
        public int SCHEDULEID { get; set; }
        public int DOCTORID { get; set; }
        public System.DateTime WORKINGDAY { get; set; }
        public System.TimeSpan SHIFTTIME { get; set; }
        public System.TimeSpan BREAKTIME { get; set; }
        public int CONSULTANTTIME { get; set; }
        public string CREATEDBY { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public string UPDATEDBY { get; set; }
        public Nullable<System.DateTime> UPDATEDDATE { get; set; }
        public bool DELETEDFLAG { get; set; }
    
        public virtual DOCTOR DOCTOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APPOINTMENT> APPOINTMENT { get; set; }
    }
}
