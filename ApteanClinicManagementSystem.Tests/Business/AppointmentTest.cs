using System;
using ClinicManagementBusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApteanClinicManagementSystem.Tests.Business
{
    [TestClass]
    public class Appointment
    {
        [TestMethod]
        public void CheckValidAppointment()
        {
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            bool isValidAppointment = appointmentBusiness.isValidAppointment(1);
            //Assert.IsTrue();
        }
    }
}
