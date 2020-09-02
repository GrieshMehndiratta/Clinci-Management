using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemModels.Models
{
    public class PrescribedMedicinesModel
    {
        [Key]
        public int PrescribedMedicineID { get; set; }

        public int PrescriptionId { get; set; }

        public PrescriptionModel Prescription { get; set; }

        public int MedicineId { get; set; }

        public MedicineModel Medicine { get; set; }

        public int Quantity { get; set; }

        public double Cost { get; set; }

    }
}
