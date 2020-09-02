using ClinicManagementDataLayer;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementBusinessLogic
{
    public class ManageUsers
    {
        public UserErrorStatus AddPatient(PatientModel patientDetails, EmergencyContactDetails emergencyContact)
        {
            GetUserDetails Details = new GetUserDetails();
            if (Details.CheckUserName(patientDetails.UserDetails.UserName))
                return UserErrorStatus.UserNameExists;
            patientDetails.UserDetails.AccountStatus = false;
            if (Details.AddPatient(patientDetails, emergencyContact))
                return UserErrorStatus.SuccessFull;
            return UserErrorStatus.Error;
            
        }

        public UserErrorStatus AddDoctor(DoctorModel doctorDetails)
        {
            GetUserDetails Details = new GetUserDetails();
            if (Details.CheckUserName(doctorDetails.UserDetails.UserName))
                return UserErrorStatus.UserNameExists;
            doctorDetails.UserDetails.AccountStatus = false;
            if (Details.AddDoctor(doctorDetails))
                return UserErrorStatus.SuccessFull;
            return UserErrorStatus.Error;
        }

        public UserErrorStatus AddNurse(NurseModel nurseDetails)
        {
            GetUserDetails Details = new GetUserDetails();
            if (Details.CheckUserName(nurseDetails.UserDetails.UserName))
                return UserErrorStatus.UserNameExists;
            nurseDetails.UserDetails.AccountStatus = false;
            if (Details.AddNurse(nurseDetails))
                return UserErrorStatus.SuccessFull;
            return UserErrorStatus.Error;
        }

        public List<PatientModel> PatientList()
        {
            GetUserDetails Details = new GetUserDetails();
            List<PatientModel> list = Details.PatientList();
            return list;
        }

        public int GetPatientIdByUsername(string username)
        {

            GetUserDetails Details = new GetUserDetails();
            int userId = Details.GetUserId(username);
               if (userId == 0)
                    return 0;           
            PatientModel patient = Details.GetPatientByUserId(userId);
            return patient.PatientId;
        }

        public PatientModel GetPatientByUserId(int userId)
        {
            GetUserDetails Details = new GetUserDetails();
            PatientModel patient = Details.GetPatientByUserId(userId);
            return patient;
        }

        public PatientModel patientDetails(int? id)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            return getUserDetails.PatientDetails(id);
        }

        public void PatientDeleteConfrimd(int? id)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            getUserDetails.PatientDeleteConfrimed(id);
            
            AppointmentBusiness appointment = new AppointmentBusiness();
            appointment.CancelAllAppointments(id, Role.Patient);
        }
        public List<int> getStats()
        {
            GetUserDetails details = new GetUserDetails();
            List<int> result = details.GetStats(DateTime.Now);
            return result;
        }


        public List<DoctorModel> DoctorList()
        {
            GetUserDetails Details = new GetUserDetails();
            List<DoctorModel> list = Details.DoctorList();
            return list;
        }

        public List<DoctorModel> GetDoctors(string specialization)
        {
            if (string.IsNullOrEmpty(specialization))
                return null;
            GetUserDetails Details = new GetUserDetails();
            List<DoctorModel> doctors = Details.GetDoctorsBySpecialization(specialization);
            return doctors;
        }

        public DoctorModel doctorDetails(int? id)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            return getUserDetails.DoctorDetails(id);
        }

        public void UpdatePatient(PatientModel patientModel)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            getUserDetails.UpdatePatient(patientModel);
        }

        public void DoctorDeleteConfrimd(int? id)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            getUserDetails.DoctorDeleteConfrimed(id);

            AppointmentBusiness appointment = new AppointmentBusiness();
            appointment.CancelAllAppointments(id, Role.Doctor);
        }

        public List<NurseModel> NurseList()
        {
            GetUserDetails Details = new GetUserDetails();
            List<NurseModel> list = Details.NurseList();
            return list;
        }

        public NurseModel NurseDetails(int? id)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            return getUserDetails.NurseDetails(id);
        }

        public bool UsersExist(params string[] id)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            for (int i = 0; i < id.Length; i++)
            {
                if (!getUserDetails.CheckUserName(id[i]))
                    return false;
            }
            return true;
        }

        public void NurseDeleteConfrimd(int id)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            getUserDetails.NurseDeleteConfrimed(id);

            AppointmentBusiness appointment = new AppointmentBusiness();
            appointment.CancelAllAppointments(id, Role.Nurse);
        }

        public void UpdateDoctor(DoctorModel doctorModel)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            getUserDetails.UpdateDoctor(doctorModel);
        }

        public void UpdateNurse(NurseModel nurseModel)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            getUserDetails.UpdateNurse(nurseModel);
        }

        public UserModel GetUserDetails(string userName)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            return getUserDetails.GetUserDetail(userName);
        }

        public void UpdateProfile(UserModel userModel)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            getUserDetails.UpdateProfile(userModel);
        }

        public void AddEmergencyDetails(EmergencyContactDetails emergencyContact)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            getUserDetails.AddEmergencyContactDetails(emergencyContact);
        }

        public EmergencyContactDetails EmergencyDetails(int id)
        {
            GetUserDetails getUserDetails = new GetUserDetails();
            EmergencyContactDetails emergencyContact = getUserDetails.GetEmergencyDetails(id);
            return emergencyContact;
        }

        public int MedicalHistoryCount(int patientId)
        {
            MedicalHistoryDataLayer medicalHistoryDataLayer = new MedicalHistoryDataLayer();
            return medicalHistoryDataLayer.PatientMedicalHistoryCount(patientId);
        }

        public bool IsDoctorAvailable(int doctorId)
        {
            Appointment appointment = new Appointment();
            List<TimeSpan> todaySlots = new List<TimeSpan>();
            appointment.GetDoctorOccupiedSlots(doctorId, DateTime.Now, todaySlots);
            return todaySlots.Count > 0 ? true : false;
        }
    }
}
