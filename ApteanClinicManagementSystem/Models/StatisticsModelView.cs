using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApteanClinicManagementSystem.Models
{
    public class StatisticsModelView
    {
        public int PatientsCount { get; set; }

        public int TotalPatientsCount{ get; set; }

        public int TotalDoctorsCount { get; set; }

        public int DoctorsCount { get; set; }

        public int TotalAppoinmentsCount { get; set; }

        public int AppointmentsCount { get; set; }

        public int ActiveUsers { get; set; }

        public int TotalUsersCount { get; set; }

        public int PatientMedicalHistoryCount { get; set; }
    }
}