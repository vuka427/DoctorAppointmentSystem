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
        public List<ScheduleViewModel> GetData()
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
                    row.doctorID = item.DOCTORID;
                    row.workingDay = item.WORKINGDAY.ToString("dd-MM-yyyy");
                    row.doctorName = item.DOCTORNAME;
                    row.speciality = item.SPECIALITY;
                    int gender = 0;
                    Int32.TryParse(item.DOCTORGENDER, out gender);
                    row.gender = dbContext.SYSTEM_PARA.Find(gender).PARAVAL;
                    row.consultantTime = item.CONSULTANTTIME.ToString()+ "'";

                    // Get the list of scheduled appointments
                    var bookedList = from appointment in dbContext.APPOINTMENT
                                      join schedule in dbContext.SCHEDULE on new { appointment.WORKINGDAY, appointment.DOCTORID } equals new { schedule.WORKINGDAY, schedule.DOCTORID }
                                      select new { appointment.APPOIMENTNO };

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
    }
}