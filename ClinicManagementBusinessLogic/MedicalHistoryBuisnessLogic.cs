using ClinicManagementDataLayer;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementBusinessLogic
{
    public class MedicalHistoryBuisnessLogic
    {
        #region Members

        private MedicalHistoryDataLayer medicalHistoryDL = new MedicalHistoryDataLayer();

        #endregion

        #region Methods
        /// <summary>
        /// Create Medical History based on Apppointment Id
        /// </summary>
        /// <param name="medicalHistory">Model of Medical History</param>
        /// <param name="appointmentId">AppointmentId</param>
        public void AddMedicalHistory(MedicalHistoryModel medicalHistory, int appointmentId)
        {
            medicalHistory.AppointmentId = appointmentId;
            medicalHistory.Date = DateTime.Now;
            medicalHistory.PatientId = medicalHistoryDL.GetPatientId(appointmentId);
            medicalHistory.DoctorId = medicalHistoryDL.GetDoctorId(appointmentId);
            medicalHistoryDL.AddMedicalHistory(medicalHistory);
        }

        /// <summary>
        /// Get Medical History details of specific patient
        /// </summary>
        /// <param name="id">PatientId</param>
        /// <returns>Object Of Medical History</returns>
        public object PatientViewOfMedicalHistory(int? id)
        {
            var medicalHistories = medicalHistoryDL.GetMedicalHistoryOfSpecificPatient(id);
            return medicalHistories;
        }

        /// <summary>
        /// Get All Medical History
        /// </summary>
        /// <returns>Object of Medical History</returns>
        public object ShowAllMedicalHistory()
        {
            var  medicalHistories = medicalHistoryDL.GetAllMedicalHistory();
            return medicalHistories;
        }

        /// <summary>
        /// Get specific Medical History details
        /// </summary>
        /// <param name="id">MedicalHistoryId</param>
        /// <returns></returns>
        public MedicalHistoryModel MedicalHistoryDetails(int? id)
        {
            MedicalHistoryModel medicalHistory = medicalHistoryDL.GetMedicalHistoryDetails(id);
            return medicalHistory;
        }

        public void UpdateMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            medicalHistoryDL.UpdateMedicalHistory(medicalHistory);
        }

        public object GetMedicalHistoryByMedicalHistoryId(int? id)
        {
            var medicalHistory = medicalHistoryDL.GetMedicalHistory(id);
            return medicalHistory;
        }

        /// <summary>
        /// Count of Patient's Medical History
        /// </summary>
        /// <param name="id">Patient Id</param>
        /// <returns>Count Of specific Patient Medical History</returns>
        public int PatientMedicalHistoryCount(int id)
        {
            return medicalHistoryDL.PatientMedicalHistoryCount(id);
        }

        public int GetPatientId(string UserName)
        {
            GetUserDetails details = new GetUserDetails();
            return details.GetPatientIdByUserName(UserName);
        }

        /// <summary>
        /// To check that medical history is added by the doctor who is linked with that appointment
        /// </summary>
        /// <param name="id">AppointmentId</param>
        /// <param name="userName">User Name</param>
        /// <returns>True if doctor is the one who is linked with that appointment</returns>
        public bool CheckDoctorId(int id, string userName)
        {
            int doctorId = medicalHistoryDL.GetDoctorId(id);
            int currentDoctorId = medicalHistoryDL.GetDoctorIdByUserName(userName);
            if(doctorId == currentDoctorId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckMedicalHistory(int id)
        {
            return medicalHistoryDL.MedicalHistoryExist(id);
        }
        #endregion
    }
}
