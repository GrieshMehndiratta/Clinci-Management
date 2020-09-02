using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystemModels.Models
{
    //Roles Associated with the User
    public class UserRoles
    {
        [Key]
        public int UserRolesId { get; set; }

        [ForeignKey("Roles")]
        public int RoleId { get; set; }

        public Roles Roles { get; set; }

        [ForeignKey("userModel")]

        public int UserId { get; set; }

        public UserModel userModel { get; set; }

    }
}
