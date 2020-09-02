using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class MedicalHistoryViewModel
    {
        public int AppointmentId { get; set; }
        public DateTime HistoryDate { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }

        [Required(ErrorMessage = "This field cannot be empty")]
        [Display(Name = "Diagnosis*")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "This field cannot be empty")]
        [Display(Name = "Medicines*")]
        public string Medicines { get; set; }

        [Required(ErrorMessage = "This field cannot be empty")]
        [Display(Name = "Clinic Remarks*")]
        public string ClinicRemarks { get; set; }

    }
}