using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemModels.Models
{
    public class MedicalHistoryModel
    {
        [Key]
        public int MedicalHistoryId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public int AppointmentId { get; set; }

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

    public class DisplayMedicalHistoryModel
    {
        public int MedicalHistoryId { get; set; }
        [Display (Name = "Token")]
        public int AppointmentId { get; set; }

        [Display (Name = "History Date")]
        public DateTime Date { get; set; }

        [Display (Name = "Patient Name")]
        public string PatientName { get; set; }

        [Display (Name = "Doctor Name")]
        public string DoctorName { get; set; }

        [Display (Name = "Diagnosis")]
        public string Diagnosis { get; set; }

        [Display (Name = "Medicine")]
        public string Medicine { get; set; }

        [Display (Name ="Medical Observations")]
        public string Remarks { get; set; }
    }
}
