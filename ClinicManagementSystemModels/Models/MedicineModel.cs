using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagementSystemModels.Models
{
    [Table("Medicines")]
    public class MedicineModel
    {
        [Key]
        public int MedicineID { get; set; }

        [Required(ErrorMessage ="Invalid Medicine Name")]
        [MaxLength(12)]
        [Display(Name ="Medicine Name")]
        public string MedicineName { get; set; }
         
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Quantity must be Number")]
        [Display(Name = "Cost")]
        public float MedicineCost { get; set; }

    }
}