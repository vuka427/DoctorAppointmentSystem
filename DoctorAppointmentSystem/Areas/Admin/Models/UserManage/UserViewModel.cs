using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.UserManage
{
    public class UserViewModel
    {
        public int USERID { get; set; } //1
        public string FULLNAME { get; set; } //2
        public string USERNAME { get; set; } //3
        public string USERTYPE { get; set; } //4
        public string GENDER { get; set; }//5
        public string DATEOFBIRTH { get; set; }//6
        public string NATIONALID { get; set; }//7
        public string MOBILENO { get; set; }//8
        public string EMAIL { get; set; }//9
        public string ADDRESS { get; set; }//10
        public string LOGINLOCKDATE { get; set; } //11
        public int LOGINRETRYCOUNT { get; set; } //12
        public string CREATEDBY { get; set; }//13
        public string CREATEDDATE { get; set; }//14
        public string UPDATEDBY { get; set; }//15
        public string UPDATEDDATE { get; set; }//16
        public string AVATARURL { get; set; }//17

    }
}