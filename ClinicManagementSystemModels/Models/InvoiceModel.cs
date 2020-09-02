using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemModels.Models
{
    public class InvoiceModel
    {
        [Key]
        public int InvoiceId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public double Discount { get; set; }

        public double Total { get; set; }

        public int PrescriptionId { get; set; }

        public PrescriptionModel Prescription { get; set; }

        public InvoiceStatus Status { get; set; }

        public double  DoctorFee { get; set; }
    }
}
