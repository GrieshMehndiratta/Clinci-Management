using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementDataLayer
{
    public class ReportingDataAccess
    {
        public List<InvoiceModel> GetInvoices(InvoiceStatus PaymentStatus)
        {
            Context DbContext = new Context();
            var result = DbContext.Invoices.Include("Prescription").Include("Prescription.Appointment").Include("Prescription.Appointment.Patient").Include("Prescription.Appointment.Patient.UserDetails").Where(invoice => invoice.Status == PaymentStatus).ToList();
            //var result = DbContext.Invoices.Where(m => m.Status == PaymentStatus).ToList();
            return result;
        }
        public List<InvoiceModel> GetInvoices(InvoiceStatus PaymentStatus,string PatientName)
        {
            Context DbContext = new Context();
            var result = DbContext.Invoices.Include("Prescription").Include("Prescription.Appointment").Include("Prescription.Appointment.Patient").Include("Prescription.Appointment.Patient.UserDetails").Where(invoice => invoice.Status == PaymentStatus && invoice.Prescription.Appointment.Patient.UserDetails.FullName.Contains(PatientName)).ToList();
            return result;
        }

        public List<InvoiceModel> GetAllInvoices()
        {
            Context DbContext = new Context();
            var result = DbContext.Invoices.Include("Prescription").Include("Prescription.Appointment").Include("Prescription.Appointment.Patient").Include("Prescription.Appointment.Patient.UserDetails").ToList();
            return result;
        }

        public List<InvoiceModel> GetAllInvoices(string PatientName)
        {
            Context DbContext = new Context();
            var result = DbContext.Invoices.Include("Prescription").Include("Prescription.Appointment").Include("Prescription.Appointment.Patient").Include("Prescription.Appointment.Patient.UserDetails").Where(invoice=>invoice.Prescription.Appointment.Patient.UserDetails.FullName.Contains(PatientName)).ToList();
            return result;
        }

        public int GetTotalPatientsForMonth(Months month,int Year)
        {
            Context DbContext = new Context();
            int SelectedMonth = (int)month;
            int result;
            if (SelectedMonth != 0)
            {
                 result = DbContext.Patient.Where(patient => patient.RegistrationDate.Month == SelectedMonth && patient.RegistrationDate.Year == Year).Count();
            }
            else
            {
                result = DbContext.Patient.Where(patient => patient.RegistrationDate.Year == Year).Count();
            }
            
            return result;
            
        }

        public List<InvoiceModel> GetMonthlyInvoices(Months month,int Year)
        {
            Context DbContext = new Context();
            int SelectedMonth = (int)month;
            List<InvoiceModel> result;
            if(SelectedMonth != 0)
            {
                result = DbContext.Invoices.Include("Prescription").Include("Prescription.Appointment").Include("Prescription.Appointment.Patient").Include("Prescription.Appointment.Patient.UserDetails").Where(invoice => invoice.InvoiceDate.Month == SelectedMonth && invoice.InvoiceDate.Year == Year).ToList();
            }
            else
            {
                result = DbContext.Invoices.Include("Prescription").Include("Prescription.Appointment").Include("Prescription.Appointment.Patient").Include("Prescription.Appointment.Patient.UserDetails").Where(invoice => invoice.InvoiceDate.Year == Year).ToList();
            }
            
            return result;
        }

        public List<PatientModel> GetInvoicedPatientForMonths(Months month,int Year)
        {
            Context DbContext = new Context();
            int SelectedMonth = (int)month;
            IQueryable<PatientModel> result;
            if(SelectedMonth!=0)
            {
                 result = from invoices in DbContext.Invoices
                             join prescription in DbContext.Prescriptions
                             on invoices.PrescriptionId equals prescription.PrescriptionId
                             join appointment in DbContext.Appointments
                             on prescription.AppointmentId equals appointment.AppointmentId
                             join patient in DbContext.Patient
                             on appointment.PatientId equals patient.PatientId
                             where invoices.InvoiceDate.Month == SelectedMonth && invoices.InvoiceDate.Year == Year
                             select (patient);
            }
            else
            {
                result = from invoices in DbContext.Invoices
                             join prescription in DbContext.Prescriptions
                             on invoices.PrescriptionId equals prescription.PrescriptionId
                             join appointment in DbContext.Appointments
                             on prescription.AppointmentId equals appointment.AppointmentId
                             join patient in DbContext.Patient
                             on appointment.PatientId equals patient.PatientId
                             where invoices.InvoiceDate.Year == Year
                             select (patient);
            }
             
            return result.ToList();
        }
    }
}
