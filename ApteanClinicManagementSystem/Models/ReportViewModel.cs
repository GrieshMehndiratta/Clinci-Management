using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class ReportViewModel
    {
        public int PatientCount { get; set; }

        public List<PatientPaymentViewModel> PatientPayment { get; set; }

        public double TotalCost { get; set; }

        public Months SelectedMonth { get; set; }

        [Range(2018,2025,ErrorMessage = "Please Specify the Year with in Range of 2019-2025")]
        public int Year { get; set; }
    }
}