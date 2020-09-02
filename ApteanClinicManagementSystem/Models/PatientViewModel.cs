using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class PatientViewModel
    {
        [Key]
        public int PatientId { get; set; }
        [Display(Name = "Age")]
        [Required(ErrorMessage ="Age is required")]
        public int Age { get; set; }
        [Display(Name = "Height")]
        [Required(ErrorMessage ="Height is required")]
        public double Height { get; set; }
        [Display(Name = "Weight")]
        [Required(ErrorMessage ="Weight is required")]
        public double Weight { get; set; }

        [Key]
        public int EmergencyContactId { get; set; }
        [Display(Name = "Emergency Contact's Name")]
        [Required(ErrorMessage = "Emergency Contact's Name is required")]
        public string Name { get; set; }
        [Display(Name = "Emergency Contact's Phone Number")]
        [Required(ErrorMessage = "Emergency Contact's Phone Number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string PhoneNo { get; set; }
        [Display(Name = "Emergency Contact's Address")]
        [Required(ErrorMessage = "Emergency Contact's Address is required")]
        public string Address { get; set; }
        [Display(Name = "Relation")]
        public RelationType Relation { get; set; }

        public DateTime RegistrationDate { get; set; }
        public virtual UserDetailsViewModel UserDetails { get; set; }
        public int AppointmentCount { get; set; }
        public int MedicalHistoyCount { get; set; }
    }
}