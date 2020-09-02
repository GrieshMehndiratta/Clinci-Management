using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class DoctorViewModel
    {
        [Key]
        public int DoctorId { get; set; }
        public UserDetailsViewModel UserDetails { get; set; }
        [Display(Name = "Specialization")]
        public SpecializationType Specialization { get; set; }
        [Display(Name ="Fee")]
        public Double Fee { get; set; }
    }
}