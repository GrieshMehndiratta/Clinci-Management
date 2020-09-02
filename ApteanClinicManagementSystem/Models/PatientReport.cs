using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class PatientReport
    {

        public int PatientId { get; set; }

        public int InvoiceNumber { get; set; }

        public double Total { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string Name { get; set; }

    }
}