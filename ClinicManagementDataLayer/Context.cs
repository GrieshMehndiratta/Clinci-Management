using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Configuration;

namespace ClinicManagementDataLayer
{
    public class Context : DbContext
    {
        public Context():base("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApteanClinic;Integrated Security=SSPI;") {
           
        }
        public DbSet<UserModel> LoginUsers { get; set; }

        //public DbSet<AppointmentModel> DummyAppointments { get; set; }

        

        //public DbSet<DummyUsersModel> dummyUsers { get; set; }

        //Edit By Griesh
        public DbSet<MedicineModel> Medicines { get; set; }

        public DbSet<Roles> Roles { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<PatientModel> Patient { get; set; }

        public DbSet<DoctorModel> Doctors { get; set; }

        public DbSet<NurseModel> Nurse { get; set; }
        public DbSet<AppointmentModel> Appointments { get; set; }

        //Added by Fazil Khan -- Prescription

        public DbSet<PrescriptionModel> Prescriptions { get; set; }

        public DbSet<PrescribedMedicinesModel> PrescribedMedicines { get; set; }

        public DbSet<InvoiceModel> Invoices { get; set; }
        public DbSet<MedicalHistoryModel> MedicalHistory { get; set; }

        public DbSet<EmergencyContactDetails> EmergencyContactDetails { get; set; }
    }
}
