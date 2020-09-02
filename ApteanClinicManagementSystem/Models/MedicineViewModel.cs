using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class MedicineViewModel
    {
        public bool Selected { get; set; }

        public int MedicineId { get; set; }

        public string MedicineName { get; set; }

        [Range(0, 10, ErrorMessage = "Please Insert the Valid Quantity")]
        //[Editable(false, AllowInitialValue = false)]
        public int Quantity { get; set; }
        public double Price { get; set; }

    }
}