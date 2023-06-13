using AutoMapper;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorScheduleManage;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.DoctorSchedule.Mapping
{
    public class MappDoctorScheduleProfile : Profile
    {
        public MappDoctorScheduleProfile()
        {
            CreateMap<SCHEDULE, DoctorScheduleViewModel>()
                .ForMember(dest => dest.DOCTORID, act => act.MapFrom(src => src.DOCTORID))
                .ForMember(dest => dest.DOCTORNAME, act => act.MapFrom(src => (src.DOCTOR!=null)? src.DOCTOR.DOCTORNAME: ""))
                .ForMember(dest => dest.WORKINGDAY, act => act.MapFrom(src => src.WORKINGDAY.ToShortDateString()))
                .ForMember(dest => dest.SHIFTTIME, act => act.MapFrom(src => src.SHIFTTIME.ToString()))
                .ForMember(dest => dest.BREAKTIME, act => act.MapFrom(src => src.BREAKTIME.ToString()))
                .ForMember(dest => dest.CREATEDDATE, act => act.MapFrom(src => src.CREATEDDATE.Value.ToShortDateString()))
                .ForMember(dest => dest.UPDATEDDATE, act => act.MapFrom(src => src.UPDATEDDATE.Value.ToShortDateString()))
                ;
            CreateMap<SCHEDULE,DoctorScheduleViewEditModel>()
                .ForMember(dest => dest.WORKINGDAY, act => act.MapFrom(src => src.WORKINGDAY.ToShortDateString()))
                .ForMember(dest => dest.SHIFTTIME, act => act.MapFrom(src => src.SHIFTTIME.ToString()))
                .ForMember(dest => dest.BREAKTIME, act => act.MapFrom(src => src.BREAKTIME.ToString()))
                ;
        }
    }
}