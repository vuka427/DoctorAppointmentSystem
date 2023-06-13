using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.Appointment.MakeAppointment;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Appointment
{
    public class AppointmentIO
    {
        public List<ScheduleViewModel> LoadSchedules()
        {
            using (DBContext dbContext = new DBContext())
            {
                var query = from doctor in dbContext.DOCTOR
                            join schedule in dbContext.SCHEDULE on doctor.DOCTORID equals schedule.DOCTORID
                            select new
                            {
                                doctor.DOCTORID,
                                doctor.DOCTORNAME,
                                doctor.SPECIALITY,
                                doctor.DOCTORGENDER,
                                schedule.SCHEDULEID,
                                schedule.BREAKTIME,
                                schedule.SHIFTTIME,
                                schedule.WORKINGDAY,
                                schedule.CONSULTANTTIME
                            };

                var list = query.ToList();

                List<ScheduleViewModel> results = new List<ScheduleViewModel>();
                foreach (var item in list)
                {
                    ScheduleViewModel row = new ScheduleViewModel();
                    row.scheduleID = item.SCHEDULEID;
                    row.doctorID = item.DOCTORID;
                    row.workingDay = item.WORKINGDAY.ToString("yyyy-MM-dd");
                    row.doctorName = item.DOCTORNAME;
                    row.speciality = item.SPECIALITY;
                    int gender = item.DOCTORGENDER;
                    row.gender = dbContext.SYSTEM_PARA.Find(gender).PARAVAL;
                    row.consultantTime = item.CONSULTANTTIME.ToString()+ "'";

                    // Get the list of scheduled appointments
                    var bookedList = from appointment in dbContext.APPOINTMENT
                                      join schedule in dbContext.SCHEDULE on new { appointment.SCHEDULEID, appointment.DOCTORID } equals new { schedule.SCHEDULEID, schedule.DOCTORID }
                                      select new { appointment.APPOINTMENTID };

                    // Count the number of scheduled appointments
                    int bookedTimes = bookedList.Count();

                    // Total time spent on scheduled appointments
                    TimeSpan totalTimeBooked = TimeSpan.FromMilliseconds(bookedTimes * item.CONSULTANTTIME);

                    TimeSpan startAvailableTimeline = item.SHIFTTIME.Add(totalTimeBooked);
                    row.availableTime = startAvailableTimeline.Hours + ":" + startAvailableTimeline.Minutes + " - " +  item.BREAKTIME.Hours + ":" + item.BREAKTIME.Minutes;
                    results.Add(row);

                }
                
                return results;
            }
        }
        public AppointmentViewModel LoadAppointment(int doctorID, string username, int scheduleID)
        {
            using (DBContext dbContext = new DBContext())
            {
                AppointmentViewModel avm = new AppointmentViewModel();

                PATIENT patient;
                SCHEDULE schedule;
                DOCTOR doctor = dbContext.DOCTOR.Where(d => d.DOCTORID.Equals(doctorID)).FirstOrDefault();
                USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();

                if (doctor != null)
                {
                    schedule = dbContext.SCHEDULE.Where(s => s.SCHEDULEID.Equals(scheduleID) && s.DOCTORID.Equals(doctorID)).FirstOrDefault();
                }
                else
                {
                    schedule = null;
                }
                                                
                if(user != null)
                {
                    patient = dbContext.PATIENT.Where(p => p.USERID.Equals(user.USERID)).FirstOrDefault();
                }
                else
                {
                    patient = null;
                }
                
                if(schedule != null && patient != null && doctor != null)
                {
                    avm.doctorID = doctor.DOCTORID;
                    avm.doctorName = doctor.DOCTORNAME;
                    avm.doctorGender = SystemParaHelper.GetParaval(doctor.DOCTORGENDER);
                    avm.doctorSpeciality = doctor.SPECIALITY;
                    avm.patientID = patient.PATIENTID;
                    avm.patientName = patient.PATIENTNAME;
                    avm.patientGender = SystemParaHelper.GetParaval(patient.PATIENTGENDER);
                    avm.patientDateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("yyyy-MM-dd");
                    avm.schuduleID = scheduleID;
                    avm.consultantTime = SystemParaHelper.GetParaval(schedule.CONSULTANTTIME) + "Minutes";
                    avm.workingDay = schedule.WORKINGDAY.ToString("yyyy-MM-dd");
                }

                return avm;
            }
        }


    }
}