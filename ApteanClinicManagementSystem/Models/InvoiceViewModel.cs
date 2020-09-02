using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class InvoiceViewModel
    {
        [Display(Name="Invoice Num")]
        public int InvoiceNumber { get; set; }

        public PatientInvoiceViewModel Patient { get; set; }

        public List<MedicineInvoiceViewModel> Medicines { get; set; }

        [Display(Name ="Date")]
        public DateTime InvoiceDate { get; set; }

        public double Total { get; set; }

        public double Discount { get; set; }

        public InvoiceStatus status { get; set; }
    }
}