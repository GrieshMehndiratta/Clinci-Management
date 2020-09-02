using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClinicManagementSystemModels.Models
{
    public class PatientModel
    {
        #region Patient Properties
        [Key]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Height is required")]
        public double Height { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        public double Weight { get; set; }
        public virtual UserModel UserDetails { get; set; }

        public int UserDetailsId { get; set; }

        public virtual EmergencyContactDetails EmergencyContactDetails { get; set; }

        public int EmergencyContactDetailsId { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RegistrationDate { get; set; }
        #endregion
    }
}