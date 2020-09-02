using ClinicManagementBusinessLogic;
using ClinicManagementSystemModels.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApteanClinicManagementSystem.Tests.Business
{
    [TestClass]
    public class InvoiceAndPrescripitonTest
    {
        [TestMethod]
        public void GetInvoiceList()
        {
            Invoice invoice = new Invoice();
            List<InvoiceModel>invoices = invoice.GetInvoices(0);
            Assert.AreEqual(0, invoices.Count);
        }

        [TestMethod]
        public void GetInvoiceForCancelledAppointment()
        {
            Invoice invoice = new Invoice();
            InvoiceModel invoices = invoice.GetInvoiceByAppointment(0);
            Assert.AreEqual(null, invoices);
        }

        [TestMethod]
        public void CheckPrescribeMedicineForPendingAppointment()
        {
            Prescription prescribe = new Prescription();
            PrescriptionStatus status = prescribe.CreateNewPrescription(4);
            Assert.IsTrue(status == PrescriptionStatus.AppointmentNotApproved);
        }

        [TestMethod]
        public void CheckPrescribeMedicineForInvalidAppointment()
        {
            Prescription prescribe = new Prescription();
            PrescriptionStatus status = prescribe.CreateNewPrescription(0);
            Assert.IsTrue(status == PrescriptionStatus.Error);
        }

        [TestMethod]
        public void TakePaymentForInvalidInvoice()
        {
            Invoice invoices = new Invoice();
            bool result = invoices.TakePayment(0);
            Assert.IsFalse(result);
        }
    }
}
