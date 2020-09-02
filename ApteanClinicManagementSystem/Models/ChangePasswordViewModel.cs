using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class ChangePasswordViewModel
    {

        public  string UserName { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Please Enter Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Please Enter a New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "New Password and Confirm Password Should be Same")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage = "Confirm Password Doesn't Match")]
        public string ConfirmPassword { get; set; }

    }
}