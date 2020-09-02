using ApteanClinicManagementSystem.Models;
using ClinicManagementBusinessLogic;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApteanClinicManagementSystem.Controllers
{
    [Authorize(Roles="Admin")]
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PatientReports()
        {
            List<PatientReport> models = GetOutStandingPatients();
            ViewBag.Title = "OutStanding Invoices";
            ViewBag.ReportType = 1;
            return View(models);
        }

        [HttpPost]
        public ActionResult PatientOutStandingReports(string PatientName)
        {
            List<PatientReport> models = GetPatientsStandingInvoices(PatientName);
            ViewBag.Title = PatientName + " Patient OutStanding Invoice";
            ViewBag.ReportType = 1;
            return View("PatientReports", models);
        }

        public ActionResult PatientsAllInvoices()
        {
            List<PatientReport> models = GetPatientInvoices();
            ViewBag.Title = "All Invoices";
            ViewBag.ReportType = 2;
            return View("PatientReports", models);
        }

        [HttpPost]
        public ActionResult PatientsAllInvoice(string PatientName)
        {
            List<PatientReport> models = GetPatientInvoices(PatientName);
            ViewBag.Title = PatientName + " All Invoices";
            ViewBag.ReportType = 2;
            return View("PatientReports", models);
        }

        [HttpGet]
        public ActionResult MonthlyReport()
        {
            Months SelectedMonth = Months.June;
            ReportViewModel Reports = GetReports(SelectedMonth,DateTime.Now.Year);
            //Reports.SelectedMonth = SelectedMonth;
            Reports.SelectedMonth = (Months)DateTime.Now.Month;
            Reports.Year = DateTime.Now.Year;
            return View(Reports);
        }

        [HttpPost]
        public ActionResult MonthlyReport(Months SelectedMonth,int Year)
        {
            ReportViewModel Reports = GetReports(SelectedMonth,Year);
            return View(Reports);
        }


        [HttpGet]
        public ActionResult _FiscalReports(ReportViewModel Reports)
        {
            return View(Reports);
        }

        #region NonActionMethods
        [NonAction]
        private List<PatientReport> GetOutStandingPatients()
        {
            Reporting reports = new Reporting();
            List<InvoiceModel> Invoices = reports.GetOutStandingInvoices();
            List<PatientReport> OutStandingInvoices = InvoiceToReport(Invoices);
            return OutStandingInvoices;
        }

        [NonAction]
        private List<PatientReport> GetPatientsStandingInvoices(string Name)
        {
            Reporting reports = new Reporting();
            List<InvoiceModel> Invoices = reports.GetOutStandingInvoices(Name);
            List<PatientReport> OutStandingInvoices = InvoiceToReport(Invoices);
            return OutStandingInvoices;
        }
        
        [NonAction]
        private List<PatientReport> InvoiceToReport(List<InvoiceModel> invoices)
        {
            List<PatientReport> OutStandingInvoices = new List<PatientReport>();
            foreach (var Invoice in invoices)
            {
                OutStandingInvoices.Add(new PatientReport() { InvoiceDate = Invoice.InvoiceDate, InvoiceNumber = Invoice.InvoiceId, Name = Invoice.Prescription.Appointment.Patient.UserDetails.FullName, Total = Invoice.Total, PatientId = Invoice.Prescription.Appointment.PatientId });
            }
            return OutStandingInvoices;
        }

        [NonAction]
        private List<PatientReport> GetPatientInvoices(string PatientName = null)
        {
            Reporting reports = new Reporting();
            List<InvoiceModel> Invoices = reports.GetAllInvoices(PatientName);
            List<PatientReport> OutStandingInvoices = InvoiceToReport(Invoices);
            return OutStandingInvoices;
        }

        [NonAction]
        private ReportViewModel GetReports(Months month,int Year)
        {
            Reporting reports = new Reporting();
            ReportViewModel result = new ReportViewModel();
            result.PatientPayment = new List<PatientPaymentViewModel>();
            List<InvoiceModel> invoices = reports.GetMonthlyInvoices(month,Year);
            result.TotalCost = invoices.Sum(m => m.Total);
            result.PatientCount = reports.GetTotalPatient(month,Year);
            List<PatientModel> patients = reports.GetPatientsForMonth(month,Year);
            foreach (var patient in patients)
            {
                if(!result.PatientPayment.Any(p=>p.PatientId == patient.PatientId))
                    result.PatientPayment.Add(new PatientPaymentViewModel() { PatientId = patient.PatientId, PatientName = patient.UserDetails.FullName, Amount = invoices.Where(m => m.Prescription.Appointment.PatientId == patient.PatientId).Select(m => m.Total).Sum() });
            }
            return result;
        }
        #endregion
    }
}