using AutoMapper;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage.Mapping
{
    public class MapAppointmentProfile : Profile
    {
        public MapAppointmentProfile(ISystemParamService sysParam) {
            CreateMap<APPOINTMENT,ApponitmentViewModel>()
                .ForMember(dest => dest.PATIENTNAME, act => 
                                                     act.MapFrom(src => src.PATIENT !=null? src.PATIENT.PATIENTNAME : "" ))
                .ForMember(dest => dest.DOCTORNAME, act =>
                                                    act.MapFrom(src => src.SCHEDULE == null ? "" :  
                                                                       src.SCHEDULE.DOCTOR != null? src.SCHEDULE.DOCTOR.DOCTORNAME :""))
                .ForMember(dest => dest.CONSULTANTTIME, act =>
                                                    act.MapFrom(src => src.SCHEDULE == null ? "":
                                                         (sysParam.GetAllParam().Where(p => p.ID == src.SCHEDULE.CONSULTANTTIME)) == null ? "" :
                                                         sysParam.GetAllParam().Where(p => p.ID == src.SCHEDULE.CONSULTANTTIME).FirstOrDefault().NOTE 
                                                    ))
                ;
        }
    }
}