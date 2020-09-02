using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class NurseViewModel
    {
        [Key]
        public int NurseID { get; set; }
        public virtual UserDetailsViewModel UserDetails { get; set; }
        
    }
}