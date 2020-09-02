using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementDataLayer
{
    public class MedicalHistoryDataLayer
    {
        Context DBContext = new Context();

        /// <summary>
        /// Add Medical History in the database
        /// </summary>
        /// <param name="medicalHistory">Main Model containing data</param>
        public void AddMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            DBContext.MedicalHistory.Add(medicalHistory);
            DBContext.SaveChanges();

        }

        /// <summary>
        /// Returns Patient Id from appointment Id
        /// </summary>
        /// <param name="appointmentId">Appointment Id</param>
        /// <returns>Patient Id</returns>
        public int GetPatientId(int appointmentId)
        {
            AppointmentModel appointmentModel = DBContext.Appointments.SingleOrDefault(m => m.AppointmentId == appointmentId);
            return appointmentModel.PatientId;
        }

        /// <summary>
        /// Returns Doctor Id from appointment Id
        /// </summary>
        /// <param name="appointmentId">Appointment Id</param>
        /// <returns>Doctor Id</returns>
        public int GetDoctorId(int appointmentId)
        {
            AppointmentModel appointmentModel = DBContext.Appointments.SingleOrDefault(m => m.AppointmentId == appointmentId);
            return appointmentModel.DoctorId;
        }

        /// <summary>
        /// Select medical History with help of medical history details
        /// </summary>
        /// <param name="id">Medical History Id</param>
        /// <returns>Medical History Model</returns>
        public MedicalHistoryModel GetMedicalHistoryDetails(int? id)
        {
            var medicalHistoryDetails = DBContext.MedicalHistory.Where(m => m.MedicalHistoryId == id).FirstOrDefault();
            return medicalHistoryDetails;
        }

        public object GetMedicalHistoryOfSpecificPatient(int? id)
        {
            //var medicalHistories = from MedicalHistoryModel in DBContext.MedicalHistory where MedicalHistoryModel.PatientId == id select (MedicalHistoryModel);
            //List<MedicalHistoryModel> medicalHistoryOfPatient = medicalHistories.ToList();
            var medicalHistories = from e in DBContext.MedicalHistory
                                   join p in DBContext.Patient on e.PatientId equals p.PatientId
                                   join q in DBContext.Doctors on e.DoctorId equals q.DoctorId
                                   where e.PatientId == id
                                   select new DisplayMedicalHistoryModel
                                   {
                                       MedicalHistoryId = e.MedicalHistoryId,
                                       Date = e.Date,
                                       PatientName = p.UserDetails.FullName,
                                       DoctorName = q.UserDetails.FullName,
                                       Diagnosis = e.Diagnosis,
                                       Medicine = e.Medicines,
                                       Remarks = e.ClinicRemarks
                                   };
            return medicalHistories;
        }

        public object GetAllMedicalHistory()
        {
            var medicalHistories = from medicalHistory in DBContext.MedicalHistory
                                   join patient in DBContext.Patient on medicalHistory.PatientId equals patient.PatientId
                                   join doctor in DBContext.Doctors on medicalHistory.DoctorId equals doctor.DoctorId
                                   select new DisplayMedicalHistoryModel
                                   {
                                       MedicalHistoryId = medicalHistory.MedicalHistoryId,
                                       Date = medicalHistory.Date,
                                       PatientName = patient.UserDetails.FullName,
                                       DoctorName = doctor.UserDetails.FullName,
                                       Diagnosis = medicalHistory.Diagnosis,
                                       Medicine = medicalHistory.Medicines,
                                       Remarks = medicalHistory.ClinicRemarks
                                   };
            return medicalHistories;
        }

        public object GetMedicalHistory(int? id)
        {
            var medicalHistory =   from history in DBContext.MedicalHistory
                                   join patient in DBContext.Patient on history.PatientId equals patient.PatientId
                                   join doctor in DBContext.Doctors on history.DoctorId equals doctor.DoctorId
                                   where history.MedicalHistoryId == id
                                   select new DisplayMedicalHistoryModel
                                   {
                                       MedicalHistoryId = history.MedicalHistoryId,
                                       AppointmentId = history.AppointmentId,
                                       Date = history.Date,
                                       PatientName = patient.UserDetails.FullName,
                                       DoctorName = doctor.UserDetails.FullName,
                                       Diagnosis = history.Diagnosis,
                                       Medicine = history.Medicines,
                                       Remarks = history.ClinicRemarks
                                   };
            return medicalHistory;
        }

        public void UpdateMedicalHistory(MedicalHistoryModel medicalHistory)
        {
            DBContext.Entry(medicalHistory).State = EntityState.Modified;
            DBContext.SaveChanges();
        }

        public int PatientMedicalHistoryCount(int id)
        {
            List<MedicalHistoryModel> medicalHistories = DBContext.MedicalHistory.Where(m => m.PatientId == id).ToList();
            return medicalHistories.Count;
        }

        public int GetDoctorIdByUserName(string userName)
        {
            using (Context DbContext = new Context())
            {
                try
                {
                    var result = DbContext.Doctors.SingleOrDefault(m => m.UserDetails.UserName == userName);
                    return result.DoctorId;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        public bool MedicalHistoryExist(int id)
        {
            using (Context DbContext = new Context())
            {
                try
                {
                    int medicalHistoryCount = DbContext.MedicalHistory.Count(medicalHistory => medicalHistory.AppointmentId == id);
                    if(medicalHistoryCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch(Exception ex)
                {
                    return true;
                }
            }
        }
    }
}
