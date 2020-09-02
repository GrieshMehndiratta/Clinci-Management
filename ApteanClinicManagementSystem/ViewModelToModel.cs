using ClinicManagementSystemModels.Models;
using ApteanClinicManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClinicManagementBusinessLogic;
using static ClinicManagementSystemModels.Models.AppointmentModel;

namespace ApteanClinicManagementSystem
{
    public static class ViewModelToModel
    {
        public static AppointmentModel AppointmentViewModelToModel(this CreateAppointmentViewModel appointmentViewModel)
        {
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            AppointmentModel appointmentModel = new AppointmentModel();
            ManageUsers manageUsers = new ManageUsers();
            appointmentModel.DoctorId = appointmentViewModel.DoctorId[0];
            appointmentModel.PatientId = manageUsers.GetPatientIdByUsername(appointmentViewModel.Username);

            if (appointmentModel.PatientId == 0)
                throw new Exception("Invalid Username");
            Dictionary<string, TimeSpan> timeDisplay = appointmentBusiness.GetAvailableTime(appointmentModel.DoctorId, appointmentModel.PatientId, appointmentModel.AppointmentDate);
            appointmentModel.AppointmentTime = timeDisplay[appointmentViewModel.AppointmentTime[0]];

            DateTime appointmentDateTime = appointmentViewModel.AppointmentDate.Add(appointmentModel.AppointmentTime); // To store both date and time in appointment date
            appointmentModel.AppointmentDate = appointmentDateTime;
            
            appointmentModel.Details = appointmentViewModel.Details;
            appointmentModel.AppointmentStatus = Status.Pending;
            return appointmentModel;


        }

    }
}