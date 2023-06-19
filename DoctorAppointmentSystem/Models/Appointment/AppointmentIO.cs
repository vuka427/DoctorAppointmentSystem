using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.Appointment.History;
using DoctorAppointmentSystem.Models.Appointment.MakeAppointment;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Appointment
{
    public class AppointmentIO
    {
        public List<ScheduleViewModel> LoadScheduleList()
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
                    // Skip past days
                    if (item.WORKINGDAY.CompareTo(DateTime.Now.Date) < 0 || (item.WORKINGDAY.CompareTo(DateTime.Now.Date) == 0 && item.BREAKTIME.CompareTo(DateTime.Now.TimeOfDay) < 0))
                    {
                        continue;
                    }

                    ScheduleViewModel row = new ScheduleViewModel();
                    row.scheduleID = item.SCHEDULEID;
                    row.doctorID = item.DOCTORID;
                    row.workingDay = item.WORKINGDAY.ToString("yyyy-MM-dd");
                    row.doctorName = item.DOCTORNAME;
                    row.speciality = item.SPECIALITY;
                    int gender = item.DOCTORGENDER;
                    row.gender = dbContext.SYSTEM_PARA.Find(gender).PARAVAL;
                    row.consultantTime = SystemParaHelper.GetParaval(item.CONSULTANTTIME).ToString() + " minutes";

                    // Get the list of scheduled appointments
                    var bookedList = from appointment in dbContext.APPOINTMENT
                                     join schedule in dbContext.SCHEDULE on new { appointment.SCHEDULEID, appointment.DOCTORID } equals new { schedule.SCHEDULEID, schedule.DOCTORID }
                                     where schedule.SCHEDULEID == item.SCHEDULEID
                                     select new { appointment.APPOINTMENTID };

                    // Count the number of scheduled appointments
                    int bookedTimes = bookedList.Count();

                    // Total time spent on scheduled appointments
                    TimeSpan totalTimeBooked = TimeSpan.FromMinutes(bookedTimes * Convert.ToInt32(SystemParaHelper.GetParaval(item.CONSULTANTTIME)));
                    TimeSpan startAvailableTimeline = item.SHIFTTIME.Add(totalTimeBooked);

                    row.availableTime = startAvailableTimeline.ToString(@"hh\:mm") + " - " + item.BREAKTIME.ToString(@"hh\:mm");
                    results.Add(row);

                }

                return results;
            }
        }

        public List<HistoryViewModel> GetAllAppointment(string username)
        {
            using (DBContext dbContext = new DBContext())
            {
                int userID = GetInfo.GetUserID(username);
                int patientID = GetInfo.GetPatientID(userID);

                List<APPOINTMENT> appointmentList = dbContext.APPOINTMENT.Where(a => a.PATIENTID.Equals(patientID)).ToList();
                List<HistoryViewModel> result = new List<HistoryViewModel>();

                foreach (APPOINTMENT appointment in appointmentList)
                {
                    HistoryViewModel row = new HistoryViewModel();
                    row.appointmentID = appointment.APPOINTMENTID;

                    int doctorID = appointment.DOCTORID;
                    DOCTOR doctor = dbContext.DOCTOR.Find(doctorID);

                    row.doctorName = doctor.DOCTORNAME;
                    row.appointmentStatus = appointment.APPOIMENTSTATUS;
                    DateTime dateOfConsultation = (DateTime)appointment.DATEOFCONSULTATION;
                    row.dateOfConsultation = dateOfConsultation.Date.ToString("yyyy-MM-dd");
                    row.consultationTime = dateOfConsultation.TimeOfDay.ToString(@"hh\:mm");
                    row.consultationDay = dateOfConsultation.DayOfWeek.ToString();

                    result.Add(row);
                }
                return result;
            }
        }

        public List<HistoryViewModel> GetUpcomingAppointment(string username)
        {
            using (DBContext dbContext = new DBContext())
            {
                int userID = GetInfo.GetUserID(username);
                int patientID = GetInfo.GetPatientID(userID);

                List<APPOINTMENT> appointmentList = dbContext.APPOINTMENT
                    .Where(a => a.PATIENTID.Equals(patientID) && a.APPOIMENTSTATUS.ToLower().Equals("confirm"))
                    .OrderBy(a => a.APPOINTMENTDATE)
                    .ThenByDescending(a => a.APPOINTMENTDATE)
                    .Take(3)
                    .ToList();

                List<HistoryViewModel> result = new List<HistoryViewModel>();

                foreach (APPOINTMENT appointment in appointmentList)
                {
                    HistoryViewModel row = new HistoryViewModel();
                    row.appointmentID = appointment.APPOINTMENTID;

                    int doctorID = appointment.DOCTORID;
                    DOCTOR doctor = dbContext.DOCTOR.Find(doctorID);

                    row.doctorName = doctor.DOCTORNAME;
                    row.appointmentStatus = appointment.APPOIMENTSTATUS;
                    DateTime dateOfConsultation = (DateTime)appointment.DATEOFCONSULTATION;
                    row.dateOfConsultation = dateOfConsultation.Date.ToString("yyyy-MM-dd");
                    row.consultationTime = dateOfConsultation.TimeOfDay.ToString(@"hh\:mm");
                    row.consultationDay = dateOfConsultation.DayOfWeek.ToString();

                    result.Add(row);
                }
                return result;
            }
        }


        public AppointmentViewModel ViewAppointment(int appointmentID)
        {
            using (DBContext dbContext = new DBContext())
            {
                APPOINTMENT appointment = dbContext.APPOINTMENT.Find(appointmentID);

                int doctorID = appointment.DOCTORID;
                DOCTOR doctor = dbContext.DOCTOR.Find(doctorID);

                int patientID = appointment.PATIENTID;
                PATIENT patient = dbContext.PATIENT.Find(patientID);

                int scheduleID = appointment.SCHEDULEID;
                SCHEDULE schedule = dbContext.SCHEDULE.Find(scheduleID, doctorID);

                AppointmentViewModel avm = new AppointmentViewModel();

                if (doctor != null && patient != null && schedule != null)
                {
                    // Doctor's Info
                    string doctorGender = SystemParaHelper.GetParaval(doctor.DOCTORGENDER);
                    avm.doctorName = doctor.DOCTORNAME;
                    avm.doctorGender = doctorGender;
                    avm.doctorSpeciality = doctor.SPECIALITY;

                    // Patient's Info
                    String patientGender = SystemParaHelper.GetParaval(patient.PATIENTGENDER);
                    avm.patientName = patient.PATIENTNAME;
                    avm.patientGender = patientGender;
                    avm.patientDateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("yyyy-MM-dd");

                    // Appointment Details
                    avm.modeOfConsultant = appointment.MODEOFCONSULTANT;
                    avm.consultantType = appointment.CONSULTANTTYPE;

                    DateTime dateOfConsultation = (DateTime)appointment.DATEOFCONSULTATION;
                    avm.dateOfConsultation = dateOfConsultation.Date.ToString("yyyy-MM-dd");
                    avm.consultationTime = dateOfConsultation.TimeOfDay.ToString(@"hh\:mm");
                    DateTime appointmentDate = new DateTime();
                    if (appointment.APPOINTMENTDATE != null)
                    {
                        appointmentDate = (DateTime)appointment.APPOINTMENTDATE;
                    }

                    avm.appointmentDate = appointmentDate.Date.ToString("yyyy-MM-dd");
                    avm.appointmentTime = appointmentDate.TimeOfDay.ToString(@"hh\:mm");

                    string consultantTime = SystemParaHelper.GetParaval(schedule.CONSULTANTTIME);
                    avm.consultantTime = consultantTime;

                    avm.symtoms = appointment.SYMTOMS;
                    avm.existingIllness = appointment.EXISTINGILLNESS;
                    avm.drugAlergies = appointment.DRUGALLERGIES;
                }
                return avm;
            }
        }

        public bool CancelAppointment(int appointmentID)
        {
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    APPOINTMENT appt = dbContext.APPOINTMENT.Find(appointmentID);

                    if (appt == null)
                    {
                        throw new Exception("APPOINTMENT not found.");
                    }
                    else
                    {
                        appt.APPOIMENTSTATUS = "Cancel";
                        dbContext.SaveChanges();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "CancelAppointment";
                string sEventType = "U";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return false;
            }
        }

        public List<ScheduleViewModel> LoadScheduleOfDoctor(int doctorID)
        {
            using (DBContext dbContext = new DBContext())
            {
                var query = from doctor in dbContext.DOCTOR
                            join schedule in dbContext.SCHEDULE on doctor.DOCTORID equals schedule.DOCTORID
                            where doctor.DOCTORID == doctorID
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
                    // Skip past days
                    if (item.WORKINGDAY.CompareTo(DateTime.Now.Date) < 0 || (item.WORKINGDAY.CompareTo(DateTime.Now.Date) == 0 && item.BREAKTIME.CompareTo(DateTime.Now.TimeOfDay) < 0))
                    {
                        continue;
                    }

                    ScheduleViewModel row = new ScheduleViewModel();
                    row.scheduleID = item.SCHEDULEID;
                    row.doctorID = item.DOCTORID;
                    row.workingDay = item.WORKINGDAY.ToString("yyyy-MM-dd");
                    row.doctorName = item.DOCTORNAME;
                    row.speciality = item.SPECIALITY;
                    int gender = item.DOCTORGENDER;
                    row.gender = dbContext.SYSTEM_PARA.Find(gender).PARAVAL;
                    row.consultantTime = SystemParaHelper.GetParaval(item.CONSULTANTTIME).ToString() + " minutes";

                    // Get the list of scheduled appointments
                    var bookedList = from appointment in dbContext.APPOINTMENT
                                     join schedule in dbContext.SCHEDULE on new { appointment.SCHEDULEID, appointment.DOCTORID } equals new { schedule.SCHEDULEID, schedule.DOCTORID }
                                     where schedule.SCHEDULEID == item.SCHEDULEID
                                     select new { appointment.APPOINTMENTID };

                    // Count the number of scheduled appointments
                    int bookedTimes = bookedList.Count();

                    // Total time spent on scheduled appointments
                    TimeSpan totalTimeBooked = TimeSpan.FromMinutes(bookedTimes * Convert.ToInt32(SystemParaHelper.GetParaval(item.CONSULTANTTIME)));
                    TimeSpan startAvailableTimeline = item.SHIFTTIME.Add(totalTimeBooked);

                    row.availableTime = startAvailableTimeline.ToString(@"hh\:mm") + " - " + item.BREAKTIME.ToString(@"hh\:mm");
                    results.Add(row);

                }

                return results;
            }
        }

        public ScheduleViewModel LoadDoctorInfo(int doctorID, string username, int scheduleID)
        {
            using (DBContext dbContext = new DBContext())
            {
                ScheduleViewModel svm = new ScheduleViewModel();

                PATIENT patient;
                SCHEDULE schedule;
                DOCTOR doctor = dbContext.DOCTOR.Where(d => d.DOCTORID.Equals(doctorID)).FirstOrDefault();
                USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
                USER doctorAccount = dbContext.USER.Find(doctor.USERID);

                if (doctor != null)
                {
                    schedule = dbContext.SCHEDULE.Where(s => s.SCHEDULEID.Equals(scheduleID) && s.DOCTORID.Equals(doctorID)).FirstOrDefault();
                }
                else
                {
                    schedule = null;
                }

                if (user != null)
                {
                    patient = dbContext.PATIENT.Where(p => p.USERID.Equals(user.USERID)).FirstOrDefault();
                }
                else
                {
                    patient = null;
                }

                if (schedule != null && patient != null && doctor != null)
                {
                    svm.doctorID = doctor.DOCTORID;
                    svm.doctorName = doctor.DOCTORNAME;
                    svm.gender = SystemParaHelper.GetParaval(doctor.DOCTORGENDER);
                    svm.speciality = doctor.SPECIALITY;
                    svm.dateOfBirth = doctor.DOCTORDATEOFBIRTH.ToString("yyyy-MM-dd");
                    svm.scheduleID = scheduleID;
                    svm.consultantTime = SystemParaHelper.GetParaval(schedule.CONSULTANTTIME) + " minutes";
                    svm.workingDay = schedule.WORKINGDAY.ToString("yyyy-MM-dd");
                    svm.shiftTime = schedule.SHIFTTIME.ToString();
                    svm.breakTime = schedule.BREAKTIME.ToString();
                    svm.phoneNumber = doctor.DOCTORMOBILENO;
                    svm.email = doctorAccount.EMAIL;
                }


                return svm;
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

                if (user != null)
                {
                    patient = dbContext.PATIENT.Where(p => p.USERID.Equals(user.USERID)).FirstOrDefault();
                }
                else
                {
                    patient = null;
                }

                if (schedule != null && patient != null && doctor != null)
                {
                    avm.doctorID = doctor.DOCTORID;
                    avm.doctorName = doctor.DOCTORNAME;
                    avm.doctorGender = SystemParaHelper.GetParaval(doctor.DOCTORGENDER);
                    avm.doctorSpeciality = doctor.SPECIALITY;
                    avm.patientID = patient.PATIENTID;
                    avm.patientName = patient.PATIENTNAME;
                    avm.patientGender = SystemParaHelper.GetParaval(patient.PATIENTGENDER);
                    avm.patientDateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("yyyy-MM-dd");
                    avm.scheduleID = scheduleID;
                    avm.consultantTime = SystemParaHelper.GetParaval(schedule.CONSULTANTTIME) + " minutes";
                    avm.workingDay = schedule.WORKINGDAY.ToString("yyyy-MM-dd");
                }

                return avm;
            }
        }

        public List<DOCTOR> GetAvailableDoctors()
        {
            using (DBContext dbContext = new DBContext())
            {
                List<DOCTOR> doctorList = new List<DOCTOR>();
                DateTime today = DateTime.Now.Date;
                TimeSpan now = DateTime.Now.TimeOfDay;
                List<SCHEDULE> scheduleList = dbContext.SCHEDULE.Where(s => s.WORKINGDAY.CompareTo(today) > 0 || (s.WORKINGDAY.CompareTo(today) == 0 && s.BREAKTIME.CompareTo(now) > 0)).OrderBy(s => s.DOCTORID).ToList();

                int doctorID = 0;
                foreach (SCHEDULE s in scheduleList)
                {
                    if (doctorID == s.DOCTORID) continue;
                    doctorID = s.DOCTORID;
                    DOCTOR doctor = dbContext.DOCTOR.Find(doctorID);

                    doctorList.Add(doctor);
                }

                return doctorList;
            }
        }

        // Get a list of doctors with corresponding work schedules
        public List<DoctorViewModel> GetDoctors(DateTime dateOfConsultation, TimeSpan time)
        {
            using (DBContext dbContext = new DBContext())
            {
                List<DoctorViewModel> result = new List<DoctorViewModel>();
                List<SCHEDULE> schedules = dbContext.SCHEDULE
                    .Where(s => s.WORKINGDAY.Equals(dateOfConsultation) && s.SHIFTTIME.CompareTo(time) <= 0 && s.BREAKTIME.CompareTo(time) > 0)
                    .OrderBy(s => s.DOCTORID)
                    .ToList();

                int doctorID = 0;
                foreach (SCHEDULE s in schedules)
                {
                    if (s.DOCTORID == doctorID) continue;

                    doctorID = s.DOCTORID;
                    DOCTOR doctor = dbContext.DOCTOR.Find(doctorID);

                    DoctorViewModel dvm = new DoctorViewModel();
                    dvm.doctorID = doctor.DOCTORID;
                    dvm.doctorName = doctor.DOCTORNAME;
                    dvm.scheduleID = s.SCHEDULEID;

                    result.Add(dvm);
                }

                return result;
            }
        }

        public bool MakeAppointment(AppointmentViewModel avm, string username)
        {
            using (DBContext dbContext = new DBContext())
            {
                int userID = GetInfo.GetUserID(username);
                int patientID = GetInfo.GetPatientID(userID);
                TimeSpan consultationTime = TimeSpan.Parse(avm.consultationTime);
                DateTime dateOfConsultation = Convert.ToDateTime(avm.dateOfConsultation) + consultationTime;

                DateTime truncatedDate = dateOfConsultation.Date;
                List<APPOINTMENT> validList = dbContext.APPOINTMENT
                    .Where(a => a.PATIENTID.Equals(patientID)
                    && a.APPOIMENTSTATUS.ToLower().CompareTo("cancel") != 0
                    && DbFunctions.TruncateTime(a.DATEOFCONSULTATION) == truncatedDate)
                    .ToList();


                foreach (APPOINTMENT a in validList)
                {
                    DateTime adateOfConsultation = (DateTime)a.DATEOFCONSULTATION;
                    TimeSpan hihi = adateOfConsultation.TimeOfDay.Add(TimeSpan.FromMinutes(30));
                    if (adateOfConsultation.Date.Equals(dateOfConsultation.Date)
                        && (dateOfConsultation.TimeOfDay.CompareTo(adateOfConsultation.TimeOfDay.Add(TimeSpan.FromMinutes(30))) <= 0
                        && dateOfConsultation.TimeOfDay.CompareTo(adateOfConsultation.TimeOfDay.Subtract(TimeSpan.FromMinutes(30))) >= 0))
                    {
                        return false;
                    }

                }

                APPOINTMENT appointment = new APPOINTMENT();
                appointment.DOCTORID = avm.doctorID;
                appointment.PATIENTID = patientID;
                appointment.MODEOFCONSULTANT = avm.modeOfConsultant;
                appointment.CONSULTANTTYPE = avm.consultantType;
                appointment.SYMTOMS = avm.symtoms;
                appointment.EXISTINGILLNESS = avm.existingIllness;
                appointment.DRUGALLERGIES = avm.drugAlergies;
                appointment.SCHEDULEID = avm.scheduleID;

                appointment.DATEOFCONSULTATION = dateOfConsultation;
                appointment.APPOIMENTSTATUS = "Pending";

                appointment.CREATEDBY = username;
                appointment.CREATEDDATE = DateTime.Now;
                appointment.UPDATEDBY = username;
                appointment.UPDATEDDATE = DateTime.Now;
                appointment.DELETEDFLAG = false;

                //-----------//
                appointment.APPOINTMENTDATE = dateOfConsultation;
                //-----------//

                dbContext.APPOINTMENT.Add(appointment);
                dbContext.SaveChanges();
                return true;
            }
        }


    }
}