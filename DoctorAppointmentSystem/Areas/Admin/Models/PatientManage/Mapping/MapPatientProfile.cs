using AutoMapper;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.PatientManage.Mapping
{
    public class MapPatientProfile : Profile
    {
        public MapPatientProfile() {

            CreateMap<PATIENT, PatientViewModel>()
               
                .ForMember(dest => dest.PATIENTDATEOFBIRTH, act => act.MapFrom(src => src.PATIENTDATEOFBIRTH.ToShortDateString()))
                .ForMember(dest => dest.CREATEDDATE, act => act.MapFrom(src => src.CREATEDDATE.Value.ToShortDateString()))
                .ForMember(dest => dest.UPDATEDDATE, act => act.MapFrom(src => src.UPDATEDDATE.Value.ToShortDateString()))
                .ForMember(dest => dest.EMAIL, act => act.MapFrom(src => src.USER != null ? src.USER.EMAIL : "none"))
                .ForMember(dest => dest.USERNAME, act => act.MapFrom(src => src.USER != null ? src.USER.USERNAME : "none"))
                .ForMember(dest => dest.LOGINLOCKDATE, act => act.MapFrom(src => (src.USER != null) ? ((src.USER.LOGINLOCKDATE != null) ? src.USER.LOGINLOCKDATE.Value.ToShortDateString() : "none") : "none"))
                .ForMember(dest => dest.LOGINRETRYCOUNT, act => act.MapFrom(src => (src.USER != null) ? ((src.USER.LOGINRETRYCOUNT != null) ? src.USER.LOGINRETRYCOUNT.Value : 0) : 0))
                ;
        }
    }
}