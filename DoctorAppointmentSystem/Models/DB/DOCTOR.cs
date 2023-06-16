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
    
    public partial class DOCTOR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCTOR()
        {
            this.SCHEDULE = new HashSet<SCHEDULE>();
        }
    
        public int DOCTORID { get; set; }
        public int USERID { get; set; }
        public int DEPARTMENTID { get; set; }
        public string DOCTORNAME { get; set; }
        public string DOCTORNATIONALID { get; set; }
        public int DOCTORGENDER { get; set; }
        public System.DateTime DOCTORDATEOFBIRTH { get; set; }
        public string DOCTORMOBILENO { get; set; }
        public string DOCTORADDRESS { get; set; }
        public string SPECIALITY { get; set; }
        public System.DateTime WORKINGSTARTDATE { get; set; }
        public System.DateTime WORKINGENDDATE { get; set; }
        public string CREATEDBY { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public string UPDATEDBY { get; set; }
        public Nullable<System.DateTime> UPDATEDDATE { get; set; }
        public bool DELETEDFLAG { get; set; }
    
        public virtual DEPARTMENT DEPARTMENT { get; set; }
        public virtual USER USER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SCHEDULE> SCHEDULE { get; set; }
    }
}
