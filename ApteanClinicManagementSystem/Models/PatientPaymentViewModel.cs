using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class PatientPaymentViewModel
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; }

        public double Amount { get; set; }
    }
}