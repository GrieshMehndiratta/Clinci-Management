using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementDataLayer
{
    public class InvoiceDataAccess
    {
        Context DbContext = new Context();

        public bool CreateInvoice(int PrescriptionId)
        {
            InvoiceModel Invoice = new InvoiceModel();
            Invoice.PrescriptionId = PrescriptionId;
            try
            {
                var medicine = from prescribedMedicines in DbContext.PrescribedMedicines
                        where prescribedMedicines.PrescriptionId == PrescriptionId
                        select (prescribedMedicines.Cost);

                var r = DbContext.Prescriptions.Include("Appointment").Include("Appointment.Doctor").SingleOrDefault(m => m.PrescriptionId == PrescriptionId);


                Invoice.DoctorFee = r.Appointment.Doctor.Fee;

                Invoice.Discount = 0;
                Invoice.Total = medicine.Sum() + Invoice.DoctorFee - Invoice.Discount;
                Invoice.InvoiceDate = DateTime.Now;
                DbContext.Invoices.Add(Invoice);
                DbContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public double GetDoctorFee(int AppointmentId)
        {
            var result = from appointments in DbContext.Appointments
                         join doctors in DbContext.Doctors
                         on appointments.DoctorId equals doctors.DoctorId
                         where appointments.AppointmentId == AppointmentId
                         select (doctors.Fee);
            return result.Sum();
        }
        public bool CheckInvoiceByPrescription(int PrescriptionID)
        {
            return DbContext.Invoices.Any(m => m.PrescriptionId == PrescriptionID);
        }

        public PatientModel GetInvoicePatientDetails(int InvoiceId)
        {
            try
            {
                var result = from invoice in DbContext.Invoices
                             join prescriptions in DbContext.Prescriptions
                             on invoice.PrescriptionId equals prescriptions.PrescriptionId
                             join appointment in DbContext.Appointments
                             on prescriptions.AppointmentId equals appointment.AppointmentId
                             join patient in DbContext.Patient
                             on appointment.PatientId equals patient.PatientId
                             where invoice.InvoiceId == InvoiceId
                             select patient;

                return result.ElementAt(0);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public InvoiceModel GetInvoiceByAppointment(int AppointmentId)
        {
            using (Context DbContext = new Context())
            {
                InvoiceModel result = DbContext.Invoices.SingleOrDefault(m => m.Prescription.AppointmentId == AppointmentId);
                return result;
            }
        }

        public PatientModel GetPatientByInvoice(int InvoiceId)
        {
                var patients = from invoice in DbContext.Invoices
                                        join prescription in DbContext.Prescriptions
                                        on invoice.PrescriptionId equals prescription.PrescriptionId
                                        join appointment in DbContext.Appointments
                                        on prescription.AppointmentId equals appointment.AppointmentId
                                        join patient in DbContext.Patient
                                        on appointment.PatientId equals patient.PatientId
                                        select (patient.PatientId);
                int Id = patients.ToList().ElementAtOrDefault(0);
                PatientModel result = DbContext.Patient.SingleOrDefault(m => m.PatientId == Id);
                return result;
        }
        public List<MedicineInvoiceViewModel> GetPrescribedMedicines(int PrescriptionId)
        {
            var medicine = from medicines in DbContext.Medicines
                           join prescrinedMedicines in DbContext.PrescribedMedicines
                           on medicines.MedicineID equals prescrinedMedicines.MedicineId
                           where prescrinedMedicines.PrescriptionId == PrescriptionId
                           select (new MedicineInvoiceViewModel() { Medicine = medicines.MedicineName, Rate = medicines.MedicineCost, Quantity = prescrinedMedicines.Quantity, Price = prescrinedMedicines.Cost});
            List<MedicineInvoiceViewModel> medic = medicine.ToList();

            return medic;
        }

        public List<InvoiceModel> GetAllInvoices()
        {
            return DbContext.Invoices.ToList();
        }

        public List<InvoiceModel> GetInvoicesById(int UserID)
        {
            var result = DbContext.Invoices.Where(m => m.Prescription.Appointment.Patient.UserDetails.UserId == UserID);
            return result.ToList();
        }

        public InvoiceModel GetInvoice(int InvoiceId)
        {
            return DbContext.Invoices.SingleOrDefault(invoice => invoice.InvoiceId == InvoiceId);
        }

        public int GetAppointmentByInvoice(int InvoiceId)
        {
            var result = DbContext.Invoices.Include("Prescription").SingleOrDefault(invoice=>invoice.InvoiceId == InvoiceId);
            return result.Prescription.AppointmentId;
        }

        public bool DeleteInvoice(int InvoiceId)
        {
            InvoiceModel invoice = GetInvoice(InvoiceId);
            try
            {
                DbContext.Invoices.Remove(invoice);
                DbContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                //Log the Error, If Required
                return false;
            }
            
        }

        public bool TakePayment(int InvoiceId)
        {
            InvoiceModel model = new InvoiceModel();
            using (Context DbContext = new Context())
            {
                InvoiceModel invoice = DbContext.Invoices.SingleOrDefault(i => i.InvoiceId == InvoiceId);
                if(invoice!=null)
                {
                    invoice.Status = InvoiceStatus.Paid;
                    DbContext.Entry(invoice).State = EntityState.Modified;
                    DbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        ~InvoiceDataAccess()
        {
            if (DbContext != null)
                DbContext.Dispose();
        }
    }
}
