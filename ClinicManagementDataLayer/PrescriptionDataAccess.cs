using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementDataLayer
{
    public class PrescriptionDataAccess
    {
        Context DbContext = new Context();
        public bool CheckPrescriptionByAppointment(int AppointmentId)
        {
            bool res = DbContext.Prescriptions.Any(m => m.AppointmentId == AppointmentId);
            return res;
        }
        public bool AddPrescription(PrescriptionModel prescription)
        {
            try
            {
                DbContext.Prescriptions.Add(prescription);
                DbContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                //Log the Exception
                return false;
            }
        }
        public int GetPrescription(int AppointmentId)
        {
            try
            {
                PrescriptionModel prescribe = DbContext.Prescriptions.SingleOrDefault(m => m.AppointmentId == AppointmentId);
                return prescribe.PrescriptionId;
            }
            catch(Exception ex)
            {
                //Log the Exception Details
                return -1;
            }
        }

        public bool AddPrescribedMedicines(List<PrescribedMedicinesModel> medicines)
        {
            try
            {
                DbContext.PrescribedMedicines.AddRange(medicines);
                DbContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        ~PrescriptionDataAccess()
        {
            if (DbContext != null)
                DbContext.Dispose();
        }
    }
}
