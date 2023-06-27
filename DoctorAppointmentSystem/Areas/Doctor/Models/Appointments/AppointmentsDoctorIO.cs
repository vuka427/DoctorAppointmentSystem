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

        public int CountApptPending(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a=>a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Pending").Count();
        }
        public int CountApptToDay(int doctorId)
        {
            var startDate = DateTime.Now;
            startDate = startDate.Date.AddHours(1).AddMinutes(0).AddSeconds(0);
           var endDate = startDate.AddDays(1);

            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && 
                                                    a.APPOIMENTSTATUS == "Confirm" &&
                                                    a.DOCTORID == doctorId &&
                                                    a.DATEOFCONSULTATION >=  startDate && a.DATEOFCONSULTATION < endDate).Count();
        }

        public int CountApptComfirmed(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Confirm").Count();
        }
        public int CountApptCompleted(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Completed").Count();
        }
    }
}