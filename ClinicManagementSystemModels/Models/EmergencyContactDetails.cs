using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicManagementSystemModels.Models
{
    public class EmergencyContactDetails
    {
        [Key]
        public int EmergencyContactId { get; set; }
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public RelationType Relation { get; set; }  
    }
}