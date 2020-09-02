using ClinicManagementBusinessLogic;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ClinicManagementSystemModels;
using static ClinicManagementSystemModels.Models.AppointmentModel;

namespace ApteanClinicManagementSystem.Models
{
    public class CustomDateTimeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Please Enter a valid time ");
            DateTime appointmentTime = Convert.ToDateTime(value).Date;
            if (appointmentTime >= DateTime.Now.Date)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Please Enter a valid time ");
        }
    }
    public class CreateAppointmentViewModel
    {
        public int TokenNo { get; set; }
        public string Username { get; set; }
        [Required]
        public SpecializationType Specializations { get; set; }
        
        public List<DoctorModel> Doctors { get; set; }
        public List<int> DoctorId { get; set; }
        [Display(Name ="Doctor Name")]
        public List<string> DoctorName { get; set; }

        [Required]
        [CustomDateTimeValidator]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Appointment Time")]
        public List<string> AppointmentTime { get; set; }

        public string Details { get; set; }

        public CreateAppointmentViewModel()
        {
            Doctors = new List<DoctorModel>();
            DoctorName = new List<string>();
            DoctorId = new List<int>();
            AppointmentTime = new List<string>();     
        }
    }
    public class EditAppointmentViewModel
    {
        [Key]
        public int AppointmentID { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Details { get; set; }
        public int Status { get; set; }
        public EditAppointmentViewModel()
        {

        }
        public EditAppointmentViewModel(int AppointmentID, string DoctorName, DateTime AppointmentDate, TimeSpan AppointmentTime, string Details, AppointmentModel.Status Status)
        {
            this.AppointmentID = AppointmentID;
            this.DoctorName = DoctorName;
            this.AppointmentDate = AppointmentDate;
            this.AppointmentTime = AppointmentTime;
            this.Details = Details;
            switch (Status)
            {
                case AppointmentModel.Status.Approved:
                    this.Status = 0;
                    break;
                case AppointmentModel.Status.Pending:
                    this.Status = 1;
                    break;
                case AppointmentModel.Status.Cancelled:
                    this.Status = 2;
                    break;
            }
        }
    }

    public class AppointmentDetailsViewModel
    {
        public int PatientId { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public Status AppointmentStatus { get; set; }
    }
}