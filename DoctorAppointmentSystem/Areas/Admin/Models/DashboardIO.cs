using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models
{
    public class DashboardIO
    {
        private readonly DBContext _dbContext;
        private readonly IloggerService _logger;

        public DashboardIO(DBContext dbContext, IloggerService logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public int CountAllDoctor()
        {
            return _dbContext.DOCTOR.Count(d => d.DELETEDFLAG == false);
        }
        public int CountAllPatient()
        {
            return _dbContext.PATIENT.Count(d => d.DELETEDFLAG == false);
        }

        public int[] GetArrayAppointmentOnWeek()
        {
            var today = DateTime.Now;
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek + 1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);

            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false &&
                                                     a.CREATEDDATE.Value > thisWeekStart &&
                                                     a.CLOSEDDATE.Value < thisWeekEnd) 
                                        .GroupBy(a=>a.CREATEDDATE.Value.Day)
                                        .Select(g=> g.Count()).ToArray();
        }


    }
}