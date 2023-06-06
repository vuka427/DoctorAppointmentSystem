using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Services
{
    public interface ISystemParamService
    {
        List<SYSTEM_PARA> GetAllParam();
        
    }

    public class SystemParamService : ISystemParamService
    {
        private readonly DBContext _dbContext;
        private List<SYSTEM_PARA> _syprama;
        public SystemParamService(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<SYSTEM_PARA> GetAllParam() {

            if(_syprama == null)
                _syprama = _dbContext.SYSTEM_PARA.ToList();
            if(_syprama == null) 
                return new List<SYSTEM_PARA>();
            return _syprama;
        }

    }
}