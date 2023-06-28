using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.Home
{
    public class calendarIO
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;

        public calendarIO(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
        }

       public List<EventViewModel> GetDoctorSchedule(DateTime start, DateTime end, int doctorId)
        {
            List<EventViewModel> schedule;
            
            try {

                var la = _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false &&
                                                    a.DATEOFCONSULTATION > start && 
                                                    a.DATEOFCONSULTATION < end &&
                                                    a.DOCTORID == doctorId && 
                                                    a.APPOIMENTSTATUS == "Confirm"
                                                    )
                                                    .Include("PATIENT").ToList();

                schedule = la.Select(s => new EventViewModel
                {
                    id = s.APPOINTMENTID,
                    title = s.PATIENT.PATIENTNAME,
                    start = s.DATEOFCONSULTATION.ToString("yyyy-MM-ddTHH:mm:ss"),
                    description = "",
                    url = "#1",
                    allDay = false,
                    color = s.APPOIMENTSTATUS == "Pending" ? "#ffc107" :
                            s.APPOIMENTSTATUS == "Completed" ? "#00ff21" :
                            s.APPOIMENTSTATUS == "Cancel" ? "#dc3545" : "#007bff"
                             ,

                }).ToList();
            }
            catch
            {
                schedule = new List<EventViewModel>();
            }
            return schedule;
        }

    }
}