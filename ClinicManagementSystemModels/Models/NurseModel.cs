using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicManagementSystemModels.Models
{
    public class NurseModel
    {
        #region Nurse Properties
        [Key]
        public int NurseId { get; set; }

        public virtual UserModel UserDetails { get; set; }

        //public int UserDetailsId { get; set; }        
        #endregion
    }
}