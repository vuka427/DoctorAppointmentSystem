using DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage;
using DoctorAppointmentSystem.HelperClasses;
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

        public List<APPOINTMENT> GetAllPendingAppt(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Pending").Include("PATIENT").Include("SCHEDULE").ToList();
        }
        public List<APPOINTMENT> GetAllCancelledAppt(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Cancel").Include("PATIENT").Include("SCHEDULE").ToList();
        }
        public List<APPOINTMENT> GetAllCompleteAppt(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Completed").Include("PATIENT").Include("SCHEDULE").ToList();
        }

        public CompletedApptViewDetailsModel getAppointmentInfo(int appointmentID, string username)
        {
            CompletedApptViewDetailsModel avm = new CompletedApptViewDetailsModel();
            try
            {
                var apm = _dbContext.APPOINTMENT.Where(a => a.APPOINTMENTID == appointmentID)
                                                .Include("PATIENT")
                                                .Include("SCHEDULE")
                                                .Include("SCHEDULE.DOCTOR")
                                                .FirstOrDefault();

                avm = _mapper.GetMapper().Map<APPOINTMENT, CompletedApptViewDetailsModel>(apm);
            }
            catch
            {
                string sEventCatg = "DOCTOR PORTAL";
                string sEventMsg = "Exception: Failed to load detail appointment";
                string sEventSrc = nameof(getAppointmentInfo);
                string sEventType = "L";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
            return avm;

        }

        public int CountApptPending()
        {
            return _dbContext.APPOINTMENT.Where(a=>a.DELETEDFLAG == false && a.APPOIMENTSTATUS == "Pending").Count();
        }
        public int CountApptToDay()
        {
            var date = DateTime.Now;
            date = date.Date.AddHours(1).AddMinutes(0).AddSeconds(0);

            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && 
                                                    a.APPOIMENTSTATUS == "Confirm" && 
           
                                                    a.DATEOFCONSULTATION.ToShortDateString() ==  date.ToShortDateString()).Count();
        }

    }
}