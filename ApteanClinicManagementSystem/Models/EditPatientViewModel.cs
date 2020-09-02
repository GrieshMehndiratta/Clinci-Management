using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class EditPatientViewModel
    {
        [Key]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public GenderType Sex { get; set; }

        [Required(ErrorMessage = "Age is Required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Height is Required")]
        public double Height { get; set; }
        [Required(ErrorMessage = "Weight is Required")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone No is required")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Date Required")]
        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }
        [Display(Name = "Appointment Count")]
        public int AppointmentCount { get; set; }
        [Display(Name = "Medical History Count")]
        public int MedicalHistoyCount { get; set; }
    }
}