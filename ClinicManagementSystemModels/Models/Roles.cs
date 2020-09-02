using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystemModels.Models
{
    //Roles specified in Application
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
