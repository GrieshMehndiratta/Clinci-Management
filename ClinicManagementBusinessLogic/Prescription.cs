using ClinicManagementDataLayer;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementBusinessLogic
{
    public class Prescription
    {
        PrescriptionDataAccess prescription = new PrescriptionDataAccess();
        
        public PrescriptionStatus CreateNewPrescription(int AppointmentId)
        {
            Appointment appointment = new Appointment();
            if (!appointment.isValidAppointment(AppointmentId))
                return PrescriptionStatus.Error;
            if (!appointment.CheckAppointmentStatus(AppointmentId))
                return PrescriptionStatus.AppointmentNotApproved;
            if (prescription.CheckPrescriptionByAppointment(AppointmentId))
                return PrescriptionStatus.AlreadyPrescribed;
            PrescriptionModel prescriptionModel = new PrescriptionModel();
            prescriptionModel.AppointmentId = AppointmentId;
            if(prescription.AddPrescription(prescriptionModel))
                return PrescriptionStatus.Success;
            return PrescriptionStatus.Error;
        }

        public int GetPrescriptionId(int AppointmentId)
        {
            return prescription.GetPrescription(AppointmentId);
        }

        public bool PrescribeMedicines(List<PrescribedMedicinesModel> medicines)
        {
            return prescription.AddPrescribedMedicines(medicines);
        }

        public bool CheckPrescription(int AppointmentId)
        {
            Appointment appointment = new Appointment();
            if (!appointment.CheckAppointmentStatus(AppointmentId))
                return false;
            return !prescription.CheckPrescriptionByAppointment(AppointmentId);
        }
    }
}
