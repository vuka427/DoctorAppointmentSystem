﻿using AutoMapper;
using DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.Appointments.Mapping
{
    public class AppointmentDoctorMappingProfile : Profile
    {

        public AppointmentDoctorMappingProfile(ISystemParamService sysParam)
        {
            CreateMap<APPOINTMENT, AppointmentViewModel>()
                .ForMember(dest => dest.PATIENTNAME, act =>
                                                    act.MapFrom(src => src.PATIENT != null ? src.PATIENT.PATIENTNAME : ""))
               .ForMember(dest => dest.PATIENTNAME, act =>
                                                    act.MapFrom(src => src.PATIENT != null ? src.PATIENT.PATIENTNAME : ""))
               
               .ForMember(dest => dest.CONSULTANTTIME, act =>
                                                   act.MapFrom(src => src.SCHEDULE == null ? "" :
                                                        (sysParam.GetAllParam().Where(p => p.ID == src.SCHEDULE.CONSULTANTTIME)) == null ? "" :
                                                        sysParam.GetAllParam().Where(p => p.ID == src.SCHEDULE.CONSULTANTTIME).FirstOrDefault().NOTE
                                                   ))
               .ForMember(dest => dest.DATEOFCONSULTANT, act => act.MapFrom(src => src.DATEOFCONSULTATION!= null ? src.DATEOFCONSULTATION.ToShortDateString() : ""))
               .ForMember(dest => dest.DATEOFCONSULTANTTIME, act => act.MapFrom(src => src.DATEOFCONSULTATION != null ? src.DATEOFCONSULTATION.TimeOfDay.ToString(@"hh\:mm") : ""))
               .ForMember(dest => dest.DATEOFCONSULTANTDAY, act => act.MapFrom(src => src.DATEOFCONSULTATION != null ? src.DATEOFCONSULTATION.DayOfWeek.ToString() : ""))
               ;

            CreateMap<APPOINTMENT, CancelledApptViewModel>()
               .ForMember(dest => dest.PATIENTNAME, act =>
                                                   act.MapFrom(src => src.PATIENT != null ? src.PATIENT.PATIENTNAME : ""))
              .ForMember(dest => dest.PATIENTNAME, act =>
                                                   act.MapFrom(src => src.PATIENT != null ? src.PATIENT.PATIENTNAME : ""))

              .ForMember(dest => dest.CONSULTANTTIME, act =>
                                                  act.MapFrom(src => src.SCHEDULE == null ? "" :
                                                       (sysParam.GetAllParam().Where(p => p.ID == src.SCHEDULE.CONSULTANTTIME)) == null ? "" :
                                                       sysParam.GetAllParam().Where(p => p.ID == src.SCHEDULE.CONSULTANTTIME).FirstOrDefault().NOTE
                                                  ))
              .ForMember(dest => dest.DATEOFCONSULTANT, act => act.MapFrom(src => src.DATEOFCONSULTATION != null ? src.DATEOFCONSULTATION.ToShortDateString() : ""))
              .ForMember(dest => dest.DATEOFCONSULTANTTIME, act => act.MapFrom(src => src.DATEOFCONSULTATION != null ? src.DATEOFCONSULTATION.TimeOfDay.ToString(@"hh\:mm") : ""))
              .ForMember(dest => dest.DATEOFCONSULTANTDAY, act => act.MapFrom(src => src.DATEOFCONSULTATION != null ? src.DATEOFCONSULTATION.DayOfWeek.ToString() : ""))
              ;
            CreateMap<APPOINTMENT, CompletedApptViewModel>()
              .ForMember(dest => dest.PATIENTNAME, act => act.MapFrom(src => src.PATIENT != null ? src.PATIENT.PATIENTNAME : ""))
             .ForMember(dest => dest.PATIENTNAME, act => act.MapFrom(src => src.PATIENT != null ? src.PATIENT.PATIENTNAME : ""))

             .ForMember(dest => dest.CONSULTANTTIME, act =>
                                                 act.MapFrom(src => src.SCHEDULE == null ? "" :
                                                      (sysParam.GetAllParam().Where(p => p.ID == src.SCHEDULE.CONSULTANTTIME)) == null ? "" :
                                                      sysParam.GetAllParam().Where(p => p.ID == src.SCHEDULE.CONSULTANTTIME).FirstOrDefault().NOTE
                                                 ))
             .ForMember(dest => dest.DATEOFCONSULTANT, act => act.MapFrom(src => src.DATEOFCONSULTATION != null ? src.DATEOFCONSULTATION.ToShortDateString() : ""))
             .ForMember(dest => dest.DATEOFCONSULTANTTIME, act => act.MapFrom(src => src.DATEOFCONSULTATION != null ? src.DATEOFCONSULTATION.TimeOfDay.ToString(@"hh\:mm") : ""))
             .ForMember(dest => dest.DATEOFCONSULTANTDAY, act => act.MapFrom(src => src.DATEOFCONSULTATION != null ? src.DATEOFCONSULTATION.DayOfWeek.ToString() : ""))
             .ForMember(dest => dest.CLOSEDBY, act =>act.MapFrom(src => src.CLOSEDBY != null ? src.CLOSEDBY : ""))
             .ForMember(dest => dest.CLOSEDDATE, act => act.MapFrom(src => src.CLOSEDDATE != null ? src.CLOSEDDATE.Value.ToShortDateString(): ""))

             ;


        }
    }
}