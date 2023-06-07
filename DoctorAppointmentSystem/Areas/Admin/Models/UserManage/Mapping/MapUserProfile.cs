using AutoMapper;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.UserManage.Mapping
{
    public class MapUserProfile : Profile
    {
        public MapUserProfile()
        {
            CreateMap<USER, UserViewModel>()


                .ForMember(dest => dest.EMAIL, act => act.MapFrom(src => src.EMAIL))
                .ForMember(dest => dest.USERNAME, act => act.MapFrom(src => src.USERNAME))
                .ForMember(dest => dest.LOGINLOCKDATE, act => act.MapFrom(src => src.LOGINLOCKDATE !=null? src.LOGINLOCKDATE.Value.ToLongDateString() :""))
                .ForMember(dest => dest.LOGINRETRYCOUNT, act => act.MapFrom(src => src.LOGINRETRYCOUNT))
                .ForMember(dest => dest.CREATEDDATE, act => act.MapFrom(src => src.CREATEDDATE.Value.ToShortDateString()))
                .ForMember(dest => dest.UPDATEDDATE, act => act.MapFrom(src => src.UPDATEDDATE.Value.ToShortDateString()))
                .ForMember(dest => dest.LASTLOGIN, act => act.MapFrom(src => src.LASTLOGIN != null? src.LASTLOGIN.Value.ToShortDateString(): ""))

                .ForMember(dest => dest.FULLNAME, act => act.MapFrom(src =>
                                        (src.USERTYPE == "Admin") ? "" :
                                        (src.USERTYPE == "Doctor") ? src.DOCTOR.FirstOrDefault().DOCTORNAME :
                                        (src.USERTYPE == "Patient") ? src.PATIENT.FirstOrDefault().PATIENTNAME :
                                        ""
                                        ))
                .ForMember(dest => dest.GENDER, act => act.MapFrom(src =>
                                        (src.USERTYPE == "Admin") ? "" :
                                        (src.USERTYPE == "Doctor") ? src.DOCTOR.FirstOrDefault().DOCTORGENDER :
                                        (src.USERTYPE == "Patient") ? src.PATIENT.FirstOrDefault().PATIENTGENDER :
                                        ""
                                        ))
                 .ForMember(dest => dest.MOBILENO, act => act.MapFrom(src =>
                                        (src.USERTYPE == "Admin") ? "" :
                                        (src.USERTYPE == "Doctor") ? src.DOCTOR.FirstOrDefault().DOCTORMOBILENO :
                                        (src.USERTYPE == "Patient") ? src.PATIENT.FirstOrDefault().PATIENTMOBILENO :
                                        ""
                                        ))


            ;
        }
    }
}