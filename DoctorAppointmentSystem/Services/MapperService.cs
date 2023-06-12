using AutoMapper;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage.Mapping;
using DoctorAppointmentSystem.Areas.Admin.Models.PatientManage.Mapping;
using DoctorAppointmentSystem.Areas.Admin.Models.UserManage.Mapping;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Services
{
    public interface IMapper
    {
        Mapper GetMapper();
    }

    public class MapperService : IMapper
    {
        private readonly ISystemParamService _sysParam;
        private Mapper _mapper;

        public MapperService(ISystemParamService sysParam)
        {
            _sysParam = sysParam;
            InitAutomapper();
        }

        private void InitAutomapper()
        {

            var config = new MapperConfiguration(cfg =>
            {
                //register mapper for doctor model
                cfg.AddProfile( new MapDoctorProfile(_sysParam));
                //register mapper for patient model
                cfg.AddProfile(new MapPatientProfile());
                //register mapper for user model
                cfg.AddProfile(new MapUserProfile());
            });

            _mapper = new Mapper(config);
            

        }

        public Mapper GetMapper()
        {
            if (_mapper == null)
                InitAutomapper();
            return _mapper;
        }
    }          
}