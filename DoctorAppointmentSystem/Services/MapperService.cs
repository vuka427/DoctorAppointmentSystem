using AutoMapper;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage.Mapping;
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
                cfg.AddProfile( new MapDoctorProfile());
            });

            var mapper = new Mapper(config);
            return mapper;

        }
    }          
}