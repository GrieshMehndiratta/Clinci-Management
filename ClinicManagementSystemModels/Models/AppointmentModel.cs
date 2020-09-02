using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClinicManagementSystemModels.Models
{
    public class CustomDateTimeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Please Enter a valid time ");
            DateTime appointmentTime = Convert.ToDateTime(value);
            if (appointmentTime.Date >= DateTime.Now.Date)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Please Enter a valid time ");
        }
    }



    public class AppointmentModel
    {
        public enum Status
        {
            Approved,
            Pending,
            Cancelled,
            Closed
        }

        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }
        public PatientModel Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public DoctorModel Doctor { get; set; }

        [Required]
        public int NurseId { get; set; }
        public NurseModel Nurse { get; set; }

        [Required]
        [CustomDateTimeValidator]        
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentTime { get; set; }
        
        public string Details { get; set; }

        [Required]
        public Status AppointmentStatus { get; set; }

        //For practice, to be ignored
        private void sampleFunction()
        {
            TimeSpan fromTime = TimeSpan.FromHours(10);
            
        }
                
    }
}
