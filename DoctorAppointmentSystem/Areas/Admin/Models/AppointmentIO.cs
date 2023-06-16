using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models
{
    public class AppointmentIO
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;
        private readonly IloggerService _logger;

        public AppointmentIO(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper, IloggerService logger)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
            _logger = logger;
        }

        public List<APPOINTMENT> GetAllAppointment()
        {
            return _dbContext.APPOINTMENT.Where(a=>a.DELETEDFLAG == false ).Include("PATIENT").Include("SCHEDULE").Include("SCHEDULE.DOCTOR").ToList();
        }

    }
}