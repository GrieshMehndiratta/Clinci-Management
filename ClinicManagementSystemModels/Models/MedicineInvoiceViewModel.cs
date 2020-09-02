using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystemModels.Models
{
    public class MedicineInvoiceViewModel
    {
        public string Medicine { get; set; }

        public double Rate { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}