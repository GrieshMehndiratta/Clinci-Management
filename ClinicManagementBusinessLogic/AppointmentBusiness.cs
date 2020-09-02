using ClinicManagementDataLayer;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementBusinessLogic
{
    public class AppointmentBusiness
    {
        Appointment appointment = new Appointment();
        public Dictionary<string, TimeSpan> GetAvailableTime(int doctorId, int patientId, DateTime appointmentDate)
        {
            Appointment appointment = new Appointment();
            List<TimeSpan> allSlots = getAllTimeSlots(appointmentDate);
            List<TimeSpan> occupiedSlots = appointment.GetOccupiedSlots(doctorId, patientId, appointmentDate);
            List<string> stringTimeSlots = new List<string>();
            int duration = Convert.ToInt32(ConfigurationManager.AppSettings["duration"]);
            TimeSpan timeDuration = TimeSpan.FromMinutes(duration);
            Dictionary<string, TimeSpan> availableSlots = new Dictionary<string, TimeSpan>();
            for (int i = 0; i < allSlots.Count; i++)
            {
                if (!occupiedSlots.Contains(allSlots[i]))
                {
                    DateTime dateTime = DateTime.Today.Add(allSlots[i]);
                    string displayTime = dateTime.ToString("hh:mm tt");
                    availableSlots.Add(displayTime, allSlots[i]);

                    //To check slot conflict in case clinic changes time duration. To be done if times is left
                    //for (int j = 0; j <= occupiedSlots.Count; j++)
                    //{
                    //    if (allSlots[i] > occupiedSlots[j] && allSlots[i] < occupiedSlots[j].Add(timeDuration))
                    //    {

                    //    }
                    //}
                }
            }

            return availableSlots;
        }

        public List<string> GetAvailableTimeList(int doctorId, int patientId, DateTime appointmentDate)
        {
            Appointment appointment = new Appointment();
            List<TimeSpan> allSlots = getAllTimeSlots(appointmentDate);
            List<TimeSpan> occupiedSlots = appointment.GetOccupiedSlots(doctorId, patientId, appointmentDate);
            List<string> stringTimeSlots = new List<string>();
            int duration = Convert.ToInt32(ConfigurationManager.AppSettings["duration"]);
            TimeSpan timeDuration = TimeSpan.FromMinutes(duration);
            List<string> availableSlots = new List<string>();
            for (int i = 0; i < allSlots.Count; i++)
            {
                if (!occupiedSlots.Contains(allSlots[i]))
                {
                    DateTime dateTime = DateTime.Today.Add(allSlots[i]);
                    string displayTime = dateTime.ToString("hh:mm tt");
                    availableSlots.Add(displayTime);

                    //To check slot conflict in case clinic changes time duration. To be done if times is left
                    //for (int j = 0; j <= occupiedSlots.Count; j++)
                    //{
                    //    if (allSlots[i] > occupiedSlots[j] && allSlots[i] < occupiedSlots[j].Add(timeDuration))
                    //    {

                    //    }
                    //}
                }
            }

            return availableSlots;
        }

        //Gets time slots between Working hours of all doctors
        private List<TimeSpan> getAllTimeSlots(DateTime appointmentDate)
        {
            List<TimeSpan> allTimeSlots = new List<TimeSpan>();
            int startTime = Convert.ToInt32(ConfigurationManager.AppSettings["startTime"]);
            int endTime = Convert.ToInt32(ConfigurationManager.AppSettings["endTime"]);
            int duration = Convert.ToInt32(ConfigurationManager.AppSettings["duration"]);

            if (appointmentDate.Date == DateTime.Now.Date && DateTime.Now.Hour >= startTime)
            {
                if (DateTime.Now.Hour < endTime)
                    startTime = DateTime.Now.Hour + 1; //Patient can't book appointment prior to passed time
                else
                    startTime = endTime;
            }
            TimeSpan timeDuration = TimeSpan.FromMinutes(duration);
            TimeSpan time = new TimeSpan(startTime, 0, 0);
            while (time.Hours != endTime)
            {
                allTimeSlots.Add(time);
                time = time.Add(timeDuration);
            }
            return allTimeSlots;
        }

        public int GetCurrentTokenNo()
        {
            Appointment appointment = new Appointment();
            return appointment.GetCurrentTokenNo();
        }

        public List<AppointmentModel> GetPatientAppointments(int? patientId)
        {
            Appointment appointment = new Appointment();
            List<AppointmentModel> appointmentModels = appointment.GetPatientAppointments(patientId);
            return appointmentModels;
        }

        public void AddAppointment(AppointmentModel appointmentModel)
        {
            appointment.AddAppointment(appointmentModel);
        }

        public void AssignNurseToAppointment(AppointmentModel appointmentModel, DateTime appointmentDate, TimeSpan appointmentTime)
        {
            Appointment appointment = new Appointment();
            NurseModel nurse = appointment.GetAvailableNurse(appointmentDate, appointmentTime);
            if (nurse == null)
                throw new Exception("No nurse available at this time, please choose a different time slot");
            appointmentModel.NurseId = nurse.NurseId;
        }

        public bool ValidateAppointmentFields(AppointmentModel appointmentModel)
        {
            Appointment appointment = new Appointment();
            //ManageUsers manageUsers = new ManageUsers();
            if (CommonValidations.isEmpty(appointmentModel.AppointmentStatus.ToString()))
            {
                return false;
            }

            if (appointmentModel.AppointmentDate.Date < DateTime.Now.Date || appointmentModel.AppointmentDate.Date > DateTime.Now.AddDays(60).Date)
                return false;

            if (appointmentModel.AppointmentDate.Date == DateTime.Now.Date && appointmentModel.AppointmentTime < DateTime.Now.TimeOfDay)
                return false;

            //List<TimeSpan> availableTime = GetAvailableTime(appointmentModel.DoctorId, appointmentModel.PatientId, appointmentModel.AppointmentDate); //Get List of available slots from database
            //if (!availableTime.Contains(appointmentModel.AppointmentTime))
            //    return false;

            //if (!manageUsers.UsersExist(appointmentModel.Doctor.UserDetails.UserName, appointmentModel.Doctor.UserDetails.UserName))
            //    return false;

            return true;
        }

        public List<AppointmentModel> GetUpcomingDoctorAppointments(string username)
        {
            Appointment appointment = new Appointment();
            List<AppointmentModel> upcomingAppointments = appointment.UpcomingDoctorAppointments(username);
            return upcomingAppointments;
        }

        public AppointmentModel EditAppointment(int? id)
        {
            Appointment appointment = new Appointment();
            return appointment.EditAppointment(id);
        }

        public void UpdateAppointment(AppointmentModel appointmentModel)
        {
            Appointment appointment = new Appointment();
            appointment.UpdateAppointment(appointmentModel);
        }

        public void CancelAppointment(int appointmentId)
        {
            Appointment appointment = new Appointment();
            appointment.CancelAppointment(appointmentId);
        }

        public AppointmentModel GetAppointment(int appointmentId)
        {
            Appointment appointment = new Appointment();
            return appointment.GetAppointment(appointmentId);
        }

        public bool isValidAppointment(int appointmentId)
        {
            Appointment appointment = new Appointment();
            return appointment.isValidAppointment(appointmentId);
        }

        public List<AppointmentModel> GetAppointmentList(string userName, Role role)
        {
            Appointment appointment = new Appointment();
            return appointment.GetAppointmentList(userName, role);
        }

        public List<AppointmentModel> GetNurseAppointmentList(string userName)
        {
            Appointment appointment = new Appointment();
            return appointment.GetNurseAppointmentList(userName);
        }

        public void CancelAllAppointments(int? id, Role role)
        {
            if (id == null)
                return;
            int patientId = int.Parse(id.ToString());
            Appointment appointment = new Appointment();
            appointment.CancelAllAppointments(patientId, role);
        }
        public void CloseAppointment(int appointmentID)
        {
            Appointment appointment = new Appointment();
            appointment.CloseAppointment(appointmentID);
        }
    }
}
