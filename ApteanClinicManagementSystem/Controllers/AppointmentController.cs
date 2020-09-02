using ApteanClinicManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClinicManagementBusinessLogic;
using ClinicManagementSystemModels.Models;
using System.Web.Security;
using Vereyon.Web;
using System.Net;

namespace ApteanClinicManagementSystem.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {

        // GET: Appointment/Details/5
        [ActionName("PatientAppointmentsView")]       
        [Authorize(Roles ="Admin , Nurse")]
        public ActionResult PatientAppointmentsView([Bind(Prefix = "id")] int? PatientId)
        {
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            List<AppointmentModel> appointmentModels = appointmentBusiness.GetPatientAppointments(PatientId);
            return View(appointmentModels);
        }

        [Authorize(Roles="Admin, Nurse")]
        public ActionResult AppointmentListForPatientList([Bind(Prefix = "id")] int? PatientId)
        {
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            List<AppointmentModel> appointmentModels = appointmentBusiness.GetPatientAppointments(PatientId);
            return View(appointmentModels);
        }

        // GET: Appointment/Create
        [ActionName("CreateAppointmentView")]
        [Authorize(Roles = "Admin, Nurse, Patient")]
        public ActionResult Create()
        {
            CreateAppointmentViewModel model = new CreateAppointmentViewModel();
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            ManageUsers manageUsers = new ManageUsers();
            model.TokenNo = appointmentBusiness.GetCurrentTokenNo();
            string selectedSpecialization = model.Specializations.ToString();            
            model.Doctors = manageUsers.GetDoctors(selectedSpecialization);
            if (model.Doctors != null && model.Doctors.Count > 0)
            {
                foreach (var doctor in model.Doctors)
                {
                    model.DoctorId.Add(doctor.DoctorId);
                    model.DoctorName.Add(doctor.UserDetails.FullName);
                }
                model.AppointmentTime = appointmentBusiness.GetAvailableTimeList(model.DoctorId[0], 0, model.AppointmentDate);
            }
            return View(model);
        }

        // POST: Appointment/AddAppointment
        [HttpPost]
        [ActionName("CreateAppointmentView")]
        [Authorize(Roles = "Admin, Nurse, Patient")]
        public ActionResult AddAppointment(CreateAppointmentViewModel appointmentViewModel)
        {
            try
            {
                AppointmentModel appointmentModel = new AppointmentModel();
                AppointmentBusiness appointment = new AppointmentBusiness();
                var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                if (ticket.UserData.ToString() == "Patient")
                {
                    appointmentViewModel.Username = ticket.Name;
                }
                else if (string.IsNullOrWhiteSpace(appointmentViewModel.Username) || appointmentViewModel.Username == "")
                {
                    FlashMessage.Warning("Enter Username");
                    TempData["ErrorMessage"] = "Enter Username";
                    return RedirectToAction("CreateAppointmentView");
                }                
                appointmentModel = appointmentViewModel.AppointmentViewModelToModel();
                if (appointmentModel == null)
                {
                    FlashMessage.Warning("Enter Username");
                    TempData["ErrorMessage"] = "Username doesn't exist";
                    return RedirectToAction("CreateAppointmentView");
                }
                appointment.AssignNurseToAppointment(appointmentModel, appointmentModel.AppointmentDate, appointmentModel.AppointmentTime);
                
                if (appointment.ValidateAppointmentFields(appointmentModel))
                {
                    appointment.AddAppointment(appointmentModel);
                    ViewBag.SuccessMessage = "Appointment created";
                    return RedirectToAction("ViewAppointments");
                }
                else
                {
                    FlashMessage.Warning("Some Unknown Error Occured, Appointment not added");
                    TempData["ErrorMessage"] = "Some Unknown Error Occured, Appointment not added";
                    return RedirectToAction("CreateAppointmentView");
                }

            }
            catch (Exception ex)
            {
                FlashMessage.Warning(ex.Message);  //To be done if time left
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("CreateAppointmentView");
            }
        }


        public ActionResult CreateSelectedPatientAppointment(int patientId)
        {
            CreateAppointmentViewModel model = new CreateAppointmentViewModel();
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            ManageUsers manageUsers = new ManageUsers();
            model.TokenNo = appointmentBusiness.GetCurrentTokenNo();
            string selectedSpecialization = model.Specializations.ToString();
            model.Doctors = manageUsers.GetDoctors(selectedSpecialization);
            if (model.Doctors != null && model.Doctors.Count > 0)
            {
                foreach (var doctor in model.Doctors)
                {
                    model.DoctorId.Add(doctor.DoctorId);
                    model.DoctorName.Add(doctor.UserDetails.FullName);
                }
                model.AppointmentTime = appointmentBusiness.GetAvailableTimeList(model.DoctorId[0], 0, model.AppointmentDate);
            }
            return View(model);
        }
        // GET: Appointment/Edit/5
      
        // GET: Appointment/Delete/5
        [ActionName("DeleteAppointment")]
        [Authorize(Roles = "Admin, Nurse, Patient, Doctor")]
        public ActionResult Delete([Bind(Prefix ="id")] int appointmentId)
        {
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            if (appointmentBusiness.isValidAppointment(appointmentId))
            {
                AppointmentModel model = appointmentBusiness.GetAppointment(appointmentId);
                return View(model);
            }
            return RedirectToAction("PatientAppointmentsView");
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin, Nurse, Patient")]
        [ActionName("DeleteAppointment")]
        public ActionResult DeleteAppointment(int id)
        {
            try
            {
                AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
                if (appointmentBusiness.isValidAppointment(id))
                {
                    AppointmentModel model = appointmentBusiness.GetAppointment(id);
                    appointmentBusiness.CancelAppointment(id);
                    return RedirectToAction("PatientAppointmentsView");
                }
                return RedirectToAction("PatientAppointmentsView");
            }
            catch
            {
                return ViewBag("Unknown error occured");
            }
        }

        //Added By Fazil Khan
        [HttpGet]
        [Authorize(Roles="Admin,Doctor")]
        public ActionResult PrescribeMedicine([Bind(Prefix ="id")]int AppointmentId)
        {
            Prescription prescription = new Prescription();
            if (prescription.CheckPrescription(AppointmentId))
            {
                MedicineValidations medicines = new MedicineValidations();
                List<MedicineModel> result = medicines.PrescribeMedicine(AppointmentId);
                PrescriptionViewModel model = new PrescriptionViewModel();
                model.AppointmentID = AppointmentId;
                List<MedicineViewModel> MedicineList = new List<MedicineViewModel>();
                foreach (var medicine in result)
                {
                    MedicineList.Add(new MedicineViewModel { MedicineId = medicine.MedicineID, MedicineName = medicine.MedicineName, Selected = false, Price = medicine.MedicineCost });
                }
                model.Medicines = MedicineList;
                return View(model);
            }
            ViewBag.Message = "Prescription Already Made/Appointment is Not Approved";
            return View();
        }

        //Added By Fazil Khan
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor,Nurse")]
        public ActionResult PrescribeMedicine(PrescriptionViewModel prescriptions)
        {
            List<MedicineViewModel> checkprescribedMedicines = prescriptions.Medicines.Where(m => m.Selected == true && m.Quantity<=0).ToList();
            if(checkprescribedMedicines.Count>0)
            {
                ViewBag.Message = "Medicine Quantity Should be Greater than Zero to Make Prescription";
                return View(prescriptions);
            }
            Prescription prescription = new Prescription();
            PrescriptionStatus pstatus = prescription.CreateNewPrescription(prescriptions.AppointmentID);
            if(pstatus == PrescriptionStatus.AlreadyPrescribed || pstatus == PrescriptionStatus.Error || pstatus == PrescriptionStatus.AppointmentNotApproved)
            {
                ViewBag.Message = pstatus.ToString();
                return View("PrescribedMedicineMessage");
            }
            prescriptions.PrescriptionId = prescription.GetPrescriptionId(prescriptions.AppointmentID);
            List<MedicineViewModel> prescribedMedicines = prescriptions.Medicines.Where(m => m.Selected == true).ToList();
            List<PrescribedMedicinesModel> medicines = new List<PrescribedMedicinesModel>();
            foreach (var prescribedMedicine in prescribedMedicines)
            {
                medicines.Add(new PrescribedMedicinesModel { MedicineId = prescribedMedicine.MedicineId, PrescriptionId = prescriptions.PrescriptionId, Quantity = prescribedMedicine.Quantity, Cost = prescribedMedicine.Quantity * prescribedMedicine.Price });
            }
            if (prescription.PrescribeMedicines(medicines))
            {
                //Generate Invoice after medicines are prescribed 
                Invoice Invoices = new Invoice();
                InvoiceCreateStatus status = Invoices.CreateInvoice(prescriptions.PrescriptionId);
                string Message = "Alert(";
                switch (status)
                {
                    case InvoiceCreateStatus.CreatedSuccessfully:
                        Message += "Invoice Generated SuccessFully";
                        break;
                    case InvoiceCreateStatus.Exists:
                        Message += "Invoice Already Exists";
                        break;
                    default:
                        Message += "!Error\n While Generating Invoice";
                        break;
                }
                Message += ")";
                AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
                appointmentBusiness.CloseAppointment(prescriptions.AppointmentID);

                return RedirectToAction("DeleteAppointment", new { id = prescriptions.AppointmentID });
            }
            return Content("Medicine Not Prescribed");
        }

        public ActionResult ViewAppointments()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            string Roles = ticket.UserData.ToString();
            Role role= (Role)Enum.Parse(typeof(Role), Roles, true);
            List<AppointmentModel> list;
            string UserName = ticket.Name;
            list = appointmentBusiness.GetAppointmentList(UserName, role);
            if (Roles.Equals("Doctor"))
            {
                return View("AppointmentListForDoctor", list);
            }
            else
            {
                return View(list);
            }
        }
        [ChildActionOnly]
        [Authorize(Roles = "Doctor")]
        public PartialViewResult GetUpcomingAppointments()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            List<AppointmentModel> model = new AppointmentBusiness().GetUpcomingDoctorAppointments(ticket.Name);
            return PartialView("_AppointmentList", model);
        }
        [Authorize(Roles = "Nurse")]
        public ActionResult NurseAppointments()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();            
            string UserName = ticket.Name;

            List<AppointmentModel> appointments = appointmentBusiness.GetNurseAppointmentList(UserName);
            return View("ViewAppointments", appointments);
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ActionResult EditAppointment(int? Id)
        {

            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            AppointmentModel appointmentModel = appointmentBusiness.EditAppointment(Id);
            EditAppointmentViewModel editAppointmentViewModel = new EditAppointmentViewModel(appointmentModel.AppointmentId, appointmentModel.Doctor.UserDetails.FullName, appointmentModel.AppointmentDate, appointmentModel.AppointmentTime, appointmentModel.Details, appointmentModel.AppointmentStatus);
            if (appointmentModel == null)
            {
                return HttpNotFound();
            }
            return View(editAppointmentViewModel);
        }

        [HttpPost]
        public ActionResult EditAppointment(EditAppointmentViewModel editappointmentViewModel)
        {
            AppointmentModel appointmentModel = EditAppointmentViewModelToModel(editappointmentViewModel);
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            appointmentBusiness.UpdateAppointment(appointmentModel);

            return RedirectToAction("ViewAppointments");
        }

        private AppointmentModel EditAppointmentViewModelToModel(EditAppointmentViewModel editappointmentViewModel)
        {
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            AppointmentModel appointmentModel = appointmentBusiness.EditAppointment(editappointmentViewModel.AppointmentID);
            appointmentModel.Doctor.UserDetails.FullName = editappointmentViewModel.DoctorName;
            appointmentModel.AppointmentDate = editappointmentViewModel.AppointmentDate;
            appointmentModel.AppointmentTime = editappointmentViewModel.AppointmentTime;
            appointmentModel.Details = editappointmentViewModel.Details;
            switch (editappointmentViewModel.Status)
            {
                case 0:
                    appointmentModel.AppointmentStatus = AppointmentModel.Status.Approved;
                    break;
                case 1:
                    appointmentModel.AppointmentStatus = AppointmentModel.Status.Pending;
                    break;
                case 2:
                    appointmentModel.AppointmentStatus = AppointmentModel.Status.Cancelled;
                    break;
            }
            return appointmentModel;
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor,Nurse,Patient")]
        public ActionResult GetDoctors(SpecializationType specialization)
        {
            AppointmentBusiness appointment = new AppointmentBusiness();
            ManageUsers manageUsers = new ManageUsers();
            return Json(manageUsers.GetDoctors(specialization.ToString()));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor,Nurse,Patient")]
        public ActionResult GetTimeSlots(string doctorId, string patientUsername, string appointmentDate)
        {
            try
            {
                var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket.UserData.ToString() == "Patient")
                {
                    patientUsername = ticket.Name;
                }
                AppointmentBusiness appointment = new AppointmentBusiness();
                ManageUsers manageUsers = new ManageUsers();
                int patientId;
                if (string.IsNullOrWhiteSpace(patientUsername) || patientUsername == "")
                    patientId = 0;
                else
                    patientId = manageUsers.GetPatientIdByUsername(patientUsername);
                
                return Json((appointment.GetAvailableTimeList(Convert.ToInt32(doctorId), patientId, Convert.ToDateTime(appointmentDate))));
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("CreateAppointmentView");
            }

        }
    }
}
