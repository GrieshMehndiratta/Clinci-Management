using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class EmergencyContactViewModel
    {
        public int PatientId { get; set; }
        [Display(Name = "Name*")]
        [Required(ErrorMessage = "This field can't be left empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field can't be left empty")]
        [Display(Name = "Phone No*")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string PhoneNo { get; set; }
        [Display(Name = "Address*")]
        [Required(ErrorMessage = "This field can't be left empty")]
        public string Address { get; set; }
        [Display(Name = "Realtion*")]
        [Required(ErrorMessage = "This field can't be left empty")]
        public RelationType Relation { get; set; }        
    }
}