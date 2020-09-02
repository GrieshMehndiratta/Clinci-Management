using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Please Enter a Valid UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter the Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}