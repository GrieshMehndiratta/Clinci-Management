using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class PrescriptionViewModel
    {
        public int AppointmentID { get; set; }
        public int PrescriptionId { get; set; }

        public List<MedicineViewModel> Medicines { get; set; }
    }
}