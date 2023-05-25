using AutoMapper;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage.Mapping
{
    public class MapDoctorProfile : Profile
    {
        public MapDoctorProfile() {

            CreateMap<DOCTOR, DoctorViewModel>()
                
                .ForMember(dest => dest.DEPARTMENT, act => act.MapFrom(src => src.DEPARTMENT != null ? src.DEPARTMENT.DEPARTMENTNAME : "không có"))
                .ForMember(dest => dest.DOCTORDATEOFBIRTH, act => act.MapFrom(src => src.DOCTORDATEOFBIRTH.ToShortDateString()))
                .ForMember(dest => dest.WORKINGENDDATE, act => act.MapFrom(src => src.WORKINGENDDATE.ToShortDateString()))
                .ForMember(dest => dest.WORKINGSTARTDATE, act => act.MapFrom(src => src.WORKINGSTARTDATE.ToShortDateString()))
                .ForMember(dest => dest.CREATEDATE, act => act.MapFrom(src => src.CREATEDDATE.Value.ToShortDateString()))
                .ForMember(dest => dest.UPDATEDATE, act => act.MapFrom(src => src.UPDATEDDATE.Value.ToShortDateString()))
                  ;
        }

    }
}