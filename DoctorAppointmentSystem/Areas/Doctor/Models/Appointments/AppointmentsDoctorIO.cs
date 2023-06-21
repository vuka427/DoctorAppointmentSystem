using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.Appointments
{

    public class AppointmentsDoctorIO
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;

        public AppointmentsDoctorIO(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
        }

        public List<APPOINTMENT> GetAllAppointment(int userid)
        {



            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false).Include("PATIENT").Include("SCHEDULE").Include("SCHEDULE.DOCTOR").ToList();
        }
    }
}