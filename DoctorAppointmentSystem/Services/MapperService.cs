using AutoMapper;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage.Mapping;
using DoctorAppointmentSystem.Areas.Admin.Models.PatientManage.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Services
{
    public class MapperService
    {
        public static Mapper InitializeAutomapper()
        {

            var config = new MapperConfiguration(cfg =>
            {
                //register mapper for doctor model
                cfg.AddProfile( new MapDoctorProfile());
                //register mapper for patient model
                cfg.AddProfile(new MapPatientProfile());
            });

            var mapper = new Mapper(config);
            return mapper;

        }
    }          
}