using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemModels.Models
{
    public class PrescriptionModel
    {
        [Key]
        public int PrescriptionId { get; set; }

        public int AppointmentId { get; set; }

        public AppointmentModel Appointment { get; set; }
    }
}
