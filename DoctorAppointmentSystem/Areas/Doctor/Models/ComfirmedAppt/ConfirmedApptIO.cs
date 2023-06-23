using DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.ComfirmedAppt
{
    public class ConfirmedApptIO
    {
        public List<AppointmentViewModel> GetConfirmedAppts(int doctorID, string appointmentStatus)
        {
            List<AppointmentViewModel> results = new List<AppointmentViewModel>();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    List<APPOINTMENT> appointmentList = dbContext.APPOINTMENT.Where(a => a.DOCTORID.Equals(doctorID)
                    && a.APPOIMENTSTATUS.ToLower().Equals(appointmentStatus))
                        .ToList();

                    if (appointmentList.Count == 0)
                    {
                        throw new Exception();
                    }

                    foreach (APPOINTMENT appointment in appointmentList)
                    {
                        PATIENT patient = dbContext.PATIENT.Find(appointment.PATIENTID);

                        if (patient == null)
                        {
                            throw new Exception();
                        }

                        AppointmentViewModel avm = new AppointmentViewModel();

                        avm.appointmentID = appointment.APPOINTMENTID;
                        avm.patientName = patient.PATIENTNAME;

                        string patientGender = SystemParaHelper.GetParaval(patient.PATIENTGENDER);
                        avm.patientGender = patientGender;
                        avm.patientDateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("yyyy-MM-dd");
                        avm.dateOfConsultation = appointment.DATEOFCONSULTATION.ToString("yyy-MM-dd");
                        avm.consultationTime = appointment.DATEOFCONSULTATION.TimeOfDay.ToString(@"hh\:mm");
                        avm.consultationDay = appointment.DATEOFCONSULTATION.DayOfWeek.ToString();
                        avm.appointmentStatus = appointment.APPOIMENTSTATUS;

                        results.Add(avm);
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username);
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "LoadData";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return results;
            }
        }

        public MemberDetailViewModel GetMemberDetails(int appointmentID)
        {
            MemberDetailViewModel results = new MemberDetailViewModel();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    APPOINTMENT appointment = dbContext.APPOINTMENT.Find(appointmentID);
                    if (appointment == null)
                    {
                        throw new Exception();
                    }
                    PATIENT patient = dbContext.PATIENT.Find(appointment.PATIENTID);
                    if(patient == null)
                    {
                        throw new Exception();
                    }
                    USER user = dbContext.USER.Find(patient.USERID);
                    results.fullName = patient.PATIENTNAME;
                    results.dateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("yyyy-MM-dd");
                    string gender = SystemParaHelper.GetParaval(patient.PATIENTGENDER);
                    results.gender = gender;
                    results.nationalID = patient.PATIENTNATIONALID;
                    string avatarUrl = GetInfo.GetImgPath(user.USERNAME);
                    results.avatar = avatarUrl;
                    results.email = user.EMAIL;
                    results.phoneNumber = patient.PATIENTMOBILENO;
                    results.address = patient.PATIENTADDRESS;
                    string modeOfConsultant = SystemParaHelper.GetParaval(appointment.MODEOFCONSULTANT);
                    results.modeOfConsutant = modeOfConsultant;
                    string consultantType = SystemParaHelper.GetParaval(appointment.CONSULTANTTYPE);
                    results.consultantType = consultantType;
                    results.dateOfConsultation = appointment.DATEOFCONSULTATION.Date.ToString("yyyy-MM-dd");
                    results.consultationTime = appointment.DATEOFCONSULTATION.TimeOfDay.ToString(@"hh\:mm");
                    results.appointmentDate = appointment.APPOINTMENTDATE.Value.Date.ToString("yyyy-MM-dd");
                    results.appointmentTime = appointment.APPOINTMENTDATE.Value.TimeOfDay.ToString(@"hh\:mm");
                    results.symtoms = appointment.SYMTOMS;
                    results.existingIllness = appointment.EXISTINGILLNESS;
                    results.drugAlergies = appointment.DRUGALLERGIES;

                    return results;
                }
            }
            catch (Exception ex)
            {
                
                return results;
            }
        }

    }
}