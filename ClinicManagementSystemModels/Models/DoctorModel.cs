using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystemModels.Models
{
    public class DoctorModel
    {
        #region Doctor Properties

        [Key]
        public int DoctorId { get; set; }
        public SpecializationType Specialization { get; set; }
        [Required(ErrorMessage ="Fee is required")]
        public double Fee { get; set; }
        public virtual  UserModel UserDetails { get; set; }

        //public int UserDetailsId { get; set; }
        #endregion
    }
}