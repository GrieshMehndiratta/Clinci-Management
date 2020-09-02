using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClinicManagementSystemModels.Models.AppointmentModel;

namespace ClinicManagementDataLayer
{
    public class Appointment
    {
        public List<TimeSpan> GetOccupiedSlots(int doctorId, int patientId, DateTime appointmentDate)
        {
            List<TimeSpan> occupiedSlots = new List<TimeSpan>();
            List<TimeSpan> doctorsOccupiedSlots = GetDoctorOccupiedSlots(doctorId, appointmentDate, occupiedSlots);
            List<TimeSpan> patientOccupiedSlots = GetPatientOccupiedSlots(patientId, appointmentDate, occupiedSlots);
            return occupiedSlots;
        }

        public List<TimeSpan> GetPatientOccupiedSlots(int patientId, DateTime appointmentDate, List<TimeSpan> occupiedSlots)
        {
            using (Context DBContext = new Context())
            {
                List<AppointmentModel> occupiedAppointments = DBContext.Appointments.Where(m => m.PatientId == patientId && DbFunctions.TruncateTime(m.AppointmentDate) == appointmentDate.Date).ToList();
                foreach (var appointment in occupiedAppointments)
                {
                    occupiedSlots.Add(appointment.AppointmentTime);
                }
                return occupiedSlots;
            }
        }

        public List<TimeSpan> GetDoctorOccupiedSlots(int doctorId, DateTime appointmentDate, List<TimeSpan> occupiedSlots)
        {
            using (Context DBContext = new Context())
            {
                List<AppointmentModel> occupiedAppointments = DBContext.Appointments.Where(m => m.DoctorId == doctorId && DbFunctions.TruncateTime(m.AppointmentDate) == appointmentDate.Date && m.AppointmentStatus != Status.Cancelled).ToList();
                foreach (var appointment in occupiedAppointments)
                {
                    occupiedSlots.Add(appointment.AppointmentTime);
                }
                return occupiedSlots;
            }
        }

        public List<AppointmentModel> GetUserAppointments(int id, Role role)
        {
            using (Context DBContext = new Context())
            {
                List<AppointmentModel> userAppointments;
                if (role == Role.Doctor)
                {
                    userAppointments = DBContext.Appointments.Include(m => m.Patient).Include(m => m.Patient.UserDetails).Where(m => m.DoctorId == id).ToList();
                    return userAppointments;
                }
                else if (role == Role.Patient)
                {
                    userAppointments = DBContext.Appointments.Where(m => m.PatientId == id).ToList();
                    return userAppointments;
                }
                return null;
            }
        }

        public int GetCurrentTokenNo()
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    int token = DBContext.Appointments.Max(m => m.AppointmentId);
                    return token + 1;
                }
                catch
                {
                    return 1;
                }
            }

        }

        public void AddAppointment(AppointmentModel appointmentModel)
        {
            using (Context DBContext = new Context())
            {
                DBContext.Appointments.Add(appointmentModel);
                DBContext.SaveChanges();
            }
        }

        public List<AppointmentModel> GetPatientAppointments(int? patientId)
        {
            using (Context DBContext = new Context())
            {
                List<AppointmentModel> appointmentModels = DBContext.Appointments.Include(model => model.Doctor).Include(model => model.Doctor.UserDetails).Where(m => m.PatientId == patientId).ToList();
                return appointmentModels;
            }
        }

        public PatientModel GetPatientByUsername(string username)
        {
            using (Context DBContext = new Context())
            {
                try
                {
                    PatientModel patient = DBContext.Patient.Single(m => m.UserDetails.UserName == username);
                    return patient;
                }
                catch
                {
                    throw new Exception("Username Not Found");
                }
            }
        }

        public NurseModel GetAvailableNurse(DateTime appointmentDate, TimeSpan appointmentTime)
        {
            using (Context DBContext = new Context())
            {
                AppointmentModel appointment = null;
                List<NurseModel> nurses = DBContext.Nurse.ToList();
                Dictionary<NurseModel, int> nurseAppointmentPair = new Dictionary<NurseModel, int>();
                foreach (var nurse in nurses)
                {
                    int count = DBContext.Appointments.Count(m => m.NurseId == nurse.NurseId && m.AppointmentDate == appointmentDate);
                    nurseAppointmentPair.Add(nurse, count);
                }

                foreach (var nurse in nurseAppointmentPair.OrderBy(n => n.Value))
                {
                    appointment = DBContext.Appointments.SingleOrDefault(m => m.NurseId == nurse.Key.NurseId && m.AppointmentDate == appointmentDate && m.AppointmentTime == appointmentTime);
                    if (appointment == null)
                        return nurse.Key;
                }
                return null;
            }
        }

        public void CancelAppointment(int appointmentId)
        {
            using (Context DBContext = new Context())
            {
                AppointmentModel appointmentModel = DBContext.Appointments.Include(model => model.Doctor).Include(model => model.Patient).SingleOrDefault(model => model.AppointmentId == appointmentId);
                appointmentModel.AppointmentStatus = Status.Cancelled;
                DBContext.Entry(appointmentModel).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }

        public AppointmentModel GetAppointment(int appointmentId)
        {
            using (Context DBContext = new Context())
            {
                AppointmentModel model = DBContext.Appointments.Include(m => m.Doctor).Include(m => m.Patient).Include(m => m.Doctor.UserDetails).Include(m => m.Patient.UserDetails).Single(m => m.AppointmentId == appointmentId);
                return model;
            }
        }

        public bool isValidAppointment(int appointmentId)
        {
            using (Context DBContext = new Context())
            {
                bool result = DBContext.Appointments.Any(m => m.AppointmentId == appointmentId);
                return result;
            }
        }

        public List<AppointmentModel> UpcomingDoctorAppointments(string username)
        {
            using (Context DBContext = new Context())
            {
                List<AppointmentModel> upcomingAppointments = DBContext.Appointments.Include(model => model.Doctor).Include(model => model.Patient).Include(model => model.Patient.UserDetails).Include(model => model.Doctor.UserDetails).Where(model => model.Doctor.UserDetails.UserName == username && model.AppointmentDate >= DateTime.Now).ToList();
                return upcomingAppointments;
            }
        }

        public List<AppointmentModel> AdminViewList()
        {
            using (Context DBContext = new Context())
            {
                return DBContext.Appointments.Include(model => model.Doctor).Include(model => model.Patient).Include(m => m.Doctor.UserDetails).Include(m => m.Patient.UserDetails).ToList();
            }
        }

        public AppointmentModel EditAppointment(int? AppointmentId)
        {
            using (Context DBContext = new Context())
            {
                return DBContext.Appointments.Include(model => model.Doctor).Include(model => model.Patient).Include(m => m.Doctor.UserDetails).Include(m => m.Patient.UserDetails).Where(model => model.AppointmentId == AppointmentId).FirstOrDefault();
            }
        }

        public void UpdateAppointment(AppointmentModel appointmentDetails)
        {
            using (Context DBContext = new Context())
            {
                AppointmentModel appointmentModel = DBContext.Appointments.Include(model => model.Doctor).Include(model => model.Patient).SingleOrDefault(model => model.AppointmentId == appointmentDetails.AppointmentId);
                appointmentModel.AppointmentStatus = appointmentDetails.AppointmentStatus;
                DBContext.Entry(appointmentModel).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }

        public void CloseAppointment(int appointmentID)
        {
            using (Context DBContext = new Context())
            {
                AppointmentModel appointmentModel = DBContext.Appointments.FirstOrDefault(m => m.AppointmentId == appointmentID);
                appointmentModel.AppointmentStatus = Status.Closed;
                DBContext.Entry(appointmentModel).State = EntityState.Modified;
                DBContext.SaveChanges();
            }
        }
       
        public List<AppointmentModel> GetNurseAppointmentList(string userName)
        {
            using (Context DBContext = new Context())
            {
                List<AppointmentModel> nurseAppointments = DBContext.Appointments.Include(model => model.Doctor).Include(model => model.Patient).Include(model => model.Patient.UserDetails).Include(model => model.Doctor.UserDetails).Where(model => model.Nurse.UserDetails.UserName == userName).ToList();
                return nurseAppointments;
            }
        }

        public List<AppointmentModel> GetAppointmentList(string userName, Role role)
        {
            using (Context DBContext = new Context())
            {
                if (role == Role.Admin || role == Role.Nurse)
                    return DBContext.Appointments.Include(model => model.Doctor).Include(model => model.Patient).Include(model => model.Patient.UserDetails).Include(model => model.Doctor.UserDetails).ToList();
                else if (role == Role.Doctor)
                    return DBContext.Appointments.Include(model => model.Doctor).Include(model => model.Patient).Include(model => model.Patient.UserDetails).Include(model => model.Doctor.UserDetails).Where(model => model.Doctor.UserDetails.UserName == userName).ToList();
                else
                    return DBContext.Appointments.Include(model => model.Doctor).Include(model => model.Patient).Include(model => model.Patient.UserDetails).Include(model => model.Doctor.UserDetails).Where(model => model.Patient.UserDetails.UserName == userName).ToList();
            }
        }

        public bool CheckAppointmentStatus(int AppointmentID)
        {
            using (Context DBContext = new Context())
            {
                var result = DBContext.Appointments.Any(appointment => appointment.AppointmentId == AppointmentID && appointment.AppointmentStatus == Status.Approved && DbFunctions.TruncateTime(appointment.AppointmentDate) <= DateTime.Now);
                return result;
            }
        }

        public void CancelAllAppointments(int id, Role role)
        {
            using (Context DBContext = new Context())
            {
                switch (role)
                {
                    case Role.Doctor:
                        List<AppointmentModel> appointmentsDoctor = DBContext.Appointments.Where(m => m.DoctorId == id).ToList();
                        foreach (AppointmentModel appointment in appointmentsDoctor)
                        {
                            appointment.AppointmentStatus = Status.Cancelled;
                            DBContext.Entry(appointment).State = EntityState.Modified;
                            DBContext.SaveChanges();
                        }
                        break;
                    case Role.Patient:
                        List<AppointmentModel> appointmentsPatient = DBContext.Appointments.Where(m => m.PatientId == id).ToList();
                        foreach (AppointmentModel appointment in appointmentsPatient)
                        {
                            appointment.AppointmentStatus = Status.Cancelled;
                            DBContext.Entry(appointment).State = EntityState.Modified;
                            DBContext.SaveChanges();
                        }
                        break;
                    case Role.Nurse:
                        List<AppointmentModel> appointmentsNurse = DBContext.Appointments.Where(m => m.NurseId == id).ToList();
                        foreach (AppointmentModel appointment in appointmentsNurse)
                        {
                            appointment.AppointmentStatus = Status.Cancelled;
                            DBContext.Entry(appointment).State = EntityState.Modified;
                            DBContext.SaveChanges();
                        }
                        break;
                }
            }
        }
    }
}
