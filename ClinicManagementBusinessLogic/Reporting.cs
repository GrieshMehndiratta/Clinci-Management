using ClinicManagementDataLayer;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementBusinessLogic
{
    public class Reporting
    {
        public List<InvoiceModel> GetOutStandingInvoices()
        {
            ReportingDataAccess reports = new ReportingDataAccess();
            List<InvoiceModel> res = reports.GetInvoices(InvoiceStatus.UnPaid);
            return res;
        }
        public List<InvoiceModel> GetOutStandingInvoices(string Name)
        {
            ReportingDataAccess reports = new ReportingDataAccess();
            List<InvoiceModel> res = reports.GetInvoices(InvoiceStatus.UnPaid,Name);
            return res;
        }

        public List<InvoiceModel> GetAllInvoices(string Name)
        {
            ReportingDataAccess reports = new ReportingDataAccess();
            if (string.IsNullOrEmpty(Name))
            {
                return reports.GetAllInvoices();
            }
            return reports.GetAllInvoices(Name);
        }

        public int GetTotalPatient(Months month,int Year)
        {
            ReportingDataAccess reports = new ReportingDataAccess();
            return reports.GetTotalPatientsForMonth(month,Year);
        }
        
        public List<InvoiceModel> GetMonthlyInvoices(Months month,int Year)
        {
            ReportingDataAccess reports = new ReportingDataAccess();
            return reports.GetMonthlyInvoices(month,Year);
        }

        public List<PatientModel> GetPatientsForMonth(Months month,int Year)
        {
            ReportingDataAccess reports = new ReportingDataAccess();
            return reports.GetInvoicedPatientForMonths(month,Year);
        }
    }
}
