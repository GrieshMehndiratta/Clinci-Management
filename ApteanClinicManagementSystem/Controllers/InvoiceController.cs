using ApteanClinicManagementSystem.Models;
using ClinicManagementBusinessLogic;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ApteanClinicManagementSystem.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        //Get Method 
        public ActionResult GenerateInvoice(int PrescriptionId)
        {
            Invoice Invoices = new Invoice();
            InvoiceCreateStatus status = Invoices.CreateInvoice(PrescriptionId);
            string Message = "Alert(";
            switch(status)
            {
                case InvoiceCreateStatus.CreatedSuccessfully:
                    Message += "Invoice Generated SuccessFully";
                    break;
                case InvoiceCreateStatus.Exists:
                    Message += "Invoice Already Exists";
                    break;
                default:
                    Message += "!Error\n While Generating Invoice";
                    break;
            }
            Message += ")";
            return JavaScript(Message);
        }

        [Authorize(Roles= "Admin,Nurse,Patient")]
        public ActionResult ViewInvoice(int AppointmentId)
        {
            Invoice invoices = new Invoice();
            InvoiceModel Invoice = invoices.GetInvoiceByAppointment(AppointmentId);
            InvoiceViewModel model = new InvoiceViewModel() {InvoiceNumber = Invoice.InvoiceId,InvoiceDate = Invoice.InvoiceDate, Total = Invoice.Total,Discount = Invoice.Discount, status = Invoice.Status };
            model.Medicines = invoices.GetMedicines(Invoice.PrescriptionId);
            model.Medicines.Add(new MedicineInvoiceViewModel { Medicine = "Fee", Rate = Invoice.DoctorFee, Quantity = 0, Price = Invoice.DoctorFee });
            model.Patient = GetInvoicedPatient(Invoice.InvoiceId);
            return View(model);
        }

        [Authorize(Roles = "Admin,Nurse,Patient")]
        public ActionResult ListInvoices()
        {
            Invoice invoices = new Invoice();
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            int UserId = ticket.Version;
            List<InvoiceModel> myInvoices = invoices.GetInvoices(UserId);
            List<InvoiceViewModel> model = new List<InvoiceViewModel>();
            foreach (var invoice in myInvoices)
            {
                PatientInvoiceViewModel patientView = GetInvoicedPatient(invoice.InvoiceId);
                model.Add(new InvoiceViewModel() { InvoiceNumber = invoice.InvoiceId, InvoiceDate = invoice.InvoiceDate, Total = invoice.Total, Discount = invoice.Discount,Patient = patientView, status = invoice.Status });
            }
            return View(model);
        }
        
        [Authorize(Roles="Admin,Nurse,Patient")]
        public ActionResult View(int invoiceId)
        {
            Invoice invoices = new Invoice();
            int appointmentId = invoices.GetAppointmentId(invoiceId);
            return RedirectToAction("ViewInvoice", new { AppointmentId = appointmentId });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int invoiceId)
        {
            Invoice invoices = new Invoice();
            invoices.DeleteInvoice(invoiceId);
            return RedirectToAction("ListInvoices");
        }

        [Authorize(Roles="Nurse,Admin")]
        public ActionResult Payment()
        {
            Reporting reports = new Reporting();
            List<InvoiceModel> OutStandingInvoices = reports.GetOutStandingInvoices();
            List<InvoiceViewModel> model = new List<InvoiceViewModel>();
            foreach (var invoice in OutStandingInvoices)
            {
                PatientInvoiceViewModel patientView = GetInvoicedPatient(invoice.InvoiceId);
                model.Add(new InvoiceViewModel() { InvoiceNumber = invoice.InvoiceId, InvoiceDate = invoice.InvoiceDate, Total = invoice.Total, Discount = invoice.Discount, Patient = patientView, status = invoice.Status });
            }
            return View("ListInvoices",model);
        }

        [Authorize(Roles="Admin,Nurse")]
        public ActionResult TakePayment(int InvoiceId)
        {
            Invoice invoices = new Invoice();
            if (!invoices.TakePayment(InvoiceId))
                ViewBag.Message = "Error While Making Payment";
            else
                ViewBag.Message = "Payment Made SuccessFully";
            int appointmentId = invoices.GetAppointmentId(InvoiceId);
            return RedirectToAction("ViewInvoice",new { AppointmentId = appointmentId });
        }

        [NonAction]
        private PatientInvoiceViewModel GetInvoicedPatient(int InvoiceId)
        {
            Invoice invoices = new Invoice();
            PatientModel patient = invoices.GetPatientByInvoice(InvoiceId);
            return new PatientInvoiceViewModel() { Name = patient.UserDetails.FullName, Age = patient.Age, Phone = long.Parse(patient.UserDetails.PhoneNo) };
        }


    }
}