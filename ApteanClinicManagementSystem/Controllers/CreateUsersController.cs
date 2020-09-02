using ApteanClinicManagementSystem.Models;
using ClinicManagementBusinessLogic;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace ApteanClinicManagementSystem.Controllers
{

    public class CreateUsersController : Controller
    {
        // GET: RegisterPatient
        [HttpGet]
        public ActionResult RegisterPatient()
        {
            return View();
        }
        // POST: RegisterPatient
        [HttpPost]
        public ActionResult RegisterPatient(PatientViewModel patientViewModel)
        {
            ManageUsers addPatient = new ManageUsers();
            try
            {
                patientViewModel.UserDetails.Password = Utility.UtilityClass.ComputeSha256Hash(patientViewModel.UserDetails.Password);
                patientViewModel.UserDetails.ConfirmPassword = Utility.UtilityClass.ComputeSha256Hash(patientViewModel.UserDetails.ConfirmPassword);
                patientViewModel.RegistrationDate = DateTime.Now;
            }
            catch
            {

            }
            PatientModel patientModel = new PatientModel();
            patientModel.UserDetails = new UserModel();
            EmergencyContactDetails emergencyContactDetails = new EmergencyContactDetails();
            try
            {
                patientModel.UserDetails.FullName = patientViewModel.UserDetails.FullName;
                patientModel.UserDetails.Gender = patientViewModel.UserDetails.Gender;
                patientModel.Age = patientViewModel.Age;
                patientModel.UserDetails.Address = patientViewModel.UserDetails.Address;
                patientModel.UserDetails.PhoneNo = patientViewModel.UserDetails.PhoneNo;
                patientModel.UserDetails.EmailId = patientViewModel.UserDetails.EmailId;
                patientModel.UserDetails.UserName = patientViewModel.UserDetails.UserName;
                patientModel.UserDetails.Password = patientViewModel.UserDetails.Password;
                patientModel.Height = patientViewModel.Height;
                patientModel.Weight = patientViewModel.Weight;
                patientModel.Age = patientViewModel.Age;
                patientModel.RegistrationDate = patientViewModel.RegistrationDate;
                patientModel.RegistrationDate = patientViewModel.RegistrationDate;
                emergencyContactDetails.Name = patientViewModel.Name;
                emergencyContactDetails.Address = patientViewModel.Address;
                emergencyContactDetails.PhoneNo = patientViewModel.Name;
                emergencyContactDetails.Relation = patientViewModel.Relation;
               
            }
            catch(Exception ex)
            {
                
            }
            UserErrorStatus status = addPatient.AddPatient(patientModel, emergencyContactDetails);
            switch (status)
            {
                case UserErrorStatus.SuccessFull:
                    return RedirectToAction("Index", "Login");
                case UserErrorStatus.UserNameExists:
                    ViewBag.ErrorMessage = patientModel.UserDetails.UserName + ": User Name already Exists";
                    break;
                default:
                    ViewBag.ErrorMessage = "Error While Adding Patient " + patientModel.UserDetails.FullName;
                    break;
            }
            return View();
        }

        [HttpGet]
        public ActionResult PatientList()
        {
            ManageUsers patientList = new ManageUsers();
            List<PatientModel> list = patientList.PatientList();
            return View(list);
        }

        public ActionResult EditPatient(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageUsers manageUsers = new ManageUsers();
            PatientModel patientModel = manageUsers.patientDetails(Id);
            //PatientViewModel patientDetailsViewModel = PatientModelToViewModel(patientModel);
            EditPatientViewModel editPatientViewModel = PatientModelToEditViewModel(patientModel);

            if (editPatientViewModel == null)
            {
                return HttpNotFound();
            }
            return View(editPatientViewModel);
        }

        [HttpPost]
        public ActionResult EditPatient(EditPatientViewModel patientDetailsViewModel)
        {
            ManageUsers manageUsers = new ManageUsers();
            if (ModelState.IsValid)
            {
                PatientModel patientDetails = PatientViewModeltoModel(patientDetailsViewModel);
                manageUsers.UpdatePatient(patientDetails);
                return RedirectToAction("PatientList");
            }
            return View(patientDetailsViewModel);
        }

        private PatientModel PatientViewModeltoModel(EditPatientViewModel patientDetailsViewModel)
        {
            ManageUsers manageUsers = new ManageUsers();
            PatientModel patientModel = manageUsers.patientDetails(patientDetailsViewModel.PatientId);
            patientModel.UserDetails.FullName = patientDetailsViewModel.Name;
            patientModel.Age = patientDetailsViewModel.Age;
            patientModel.Height = patientDetailsViewModel.Height;
            patientModel.Weight = patientDetailsViewModel.Weight;
            patientModel.UserDetails.Address = patientDetailsViewModel.Address;
            patientModel.UserDetails.PhoneNo = patientDetailsViewModel.PhoneNo;
            return patientModel;
        }

        public ActionResult PatientDetails(int? Id)
        {
            ManageUsers manageUsers = new ManageUsers();
            PatientModel patientDetails = new PatientModel();
            EditPatientViewModel patientDetailsViewModel = new EditPatientViewModel();
            AppointmentBusiness appointmentBusiness = new AppointmentBusiness();
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            patientDetails = manageUsers.patientDetails(Id);
            patientDetailsViewModel = PatientModelToEditViewModel(patientDetails);
            List<AppointmentModel> list = appointmentBusiness.GetPatientAppointments(patientDetails.PatientId);
            patientDetailsViewModel.AppointmentCount = list.Count();
            patientDetailsViewModel.MedicalHistoyCount = manageUsers.MedicalHistoryCount(patientDetailsViewModel.PatientId);
            if (patientDetailsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(patientDetailsViewModel);
        }

        private EditPatientViewModel PatientModelToEditViewModel(PatientModel patientDetails)
        {
            EditPatientViewModel patientDetailsViewModel = new EditPatientViewModel();
            //patientDetailsViewModel.UserDetails = new UserDetailsViewModel();
            patientDetailsViewModel.Name = patientDetails.UserDetails.FullName;
            patientDetailsViewModel.PatientId = patientDetails.PatientId;
            patientDetailsViewModel.Sex = patientDetails.UserDetails.Gender;
            patientDetailsViewModel.RegistrationDate = patientDetails.RegistrationDate;
            patientDetailsViewModel.PhoneNo = patientDetails.UserDetails.PhoneNo;
            patientDetailsViewModel.Height = patientDetails.Height;
            patientDetailsViewModel.Weight = patientDetails.Weight;
            patientDetailsViewModel.Age = patientDetails.Age;
            patientDetailsViewModel.Address = patientDetails.UserDetails.Address;
            return patientDetailsViewModel;

        }

        public ActionResult DeletePatient(int? Id)
        {
            PatientModel patientDetails = new PatientModel();
            ManageUsers manageUsers = new ManageUsers();
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            patientDetails = manageUsers.patientDetails(Id);

            if (patientDetails == null)
            {
                return HttpNotFound();
            }
            return View(patientDetails);

        }
        public ActionResult DeleteConfirmed(int Id)
        {
            ManageUsers manageUsers = new ManageUsers();
            manageUsers.PatientDeleteConfrimd(Id);
            return RedirectToAction("PatientList");
        }


        //GET: AddPatient
        [Authorize(Roles = "Admin,Nurse")]
        [HttpGet]
        public ActionResult AddPatient()
        {
            return View();
        }

        //POST: RegisterPatient
        [HttpPost]
        [Authorize(Roles = "Admin , Nurse")]
        public ActionResult AddPatient(PatientViewModel patientViewModel)
        {
            ManageUsers addPatient = new ManageUsers();
            try
            {
                patientViewModel.UserDetails.Password = Utility.UtilityClass.ComputeSha256Hash(patientViewModel.UserDetails.Password);
                patientViewModel.UserDetails.ConfirmPassword = Utility.UtilityClass.ComputeSha256Hash(patientViewModel.UserDetails.ConfirmPassword);
                patientViewModel.RegistrationDate = DateTime.Now;
            }
            catch
            {

            }
            PatientModel patientModel = new PatientModel();
            patientModel.UserDetails = new UserModel();
            patientModel.UserDetails.FullName = patientViewModel.UserDetails.FullName;
            patientModel.UserDetails.Gender = patientViewModel.UserDetails.Gender;
            patientModel.UserDetails.Address = patientViewModel.UserDetails.Address;
            patientModel.UserDetails.PhoneNo = patientViewModel.UserDetails.PhoneNo;
            patientModel.UserDetails.EmailId = patientViewModel.UserDetails.EmailId;
            patientModel.UserDetails.UserName = patientViewModel.UserDetails.UserName;
            patientModel.UserDetails.Password = patientViewModel.UserDetails.Password;
            patientModel.Height = patientViewModel.Height;
            patientModel.Weight = patientViewModel.Weight;
            patientModel.Age = patientViewModel.Age;
            patientModel.RegistrationDate = patientViewModel.RegistrationDate;
            EmergencyContactDetails emergencyContactDetails = new EmergencyContactDetails();
            emergencyContactDetails.Name = patientViewModel.Name;
            emergencyContactDetails.Address = patientViewModel.Address;
            emergencyContactDetails.PhoneNo = patientViewModel.Name;
            emergencyContactDetails.Relation = patientViewModel.Relation;
            UserErrorStatus status = addPatient.AddPatient(patientModel, emergencyContactDetails);
            switch (status)
            {
                case UserErrorStatus.SuccessFull:
                    return RedirectToAction("Dashboard", "Login");
                case UserErrorStatus.UserNameExists:
                    ViewBag.ErrorMessage = "User Name already Exists";
                    break;
                default:
                    ViewBag.ErrorMessage = "Error While Adding Patient " + patientModel.UserDetails.FullName;
                    break;
            }
            return View();
        }

        // GET: AddDoctor
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddDoctor()
        {
            return View();
        }


        // POST: AddDoctor
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddDoctor(DoctorViewModel doctorViewModel)
        {
            ManageUsers addDoctor = new ManageUsers();
            try
            {
                doctorViewModel.UserDetails.Password = Utility.UtilityClass.ComputeSha256Hash(doctorViewModel.UserDetails.Password);
                doctorViewModel.UserDetails.ConfirmPassword = Utility.UtilityClass.ComputeSha256Hash(doctorViewModel.UserDetails.ConfirmPassword);
            }
            catch
            {

            }
            DoctorModel doctorModel = new DoctorModel();
            doctorModel.UserDetails = new UserModel();
            doctorModel.UserDetails.FullName = doctorViewModel.UserDetails.FullName;
            doctorModel.UserDetails.Gender = doctorViewModel.UserDetails.Gender;
            doctorModel.UserDetails.Address = doctorViewModel.UserDetails.Address;
            doctorModel.UserDetails.PhoneNo = doctorViewModel.UserDetails.PhoneNo;
            doctorModel.UserDetails.EmailId = doctorViewModel.UserDetails.EmailId;
            doctorModel.UserDetails.UserName = doctorViewModel.UserDetails.UserName;
            doctorModel.UserDetails.Password = doctorViewModel.UserDetails.Password;
            doctorModel.Specialization = doctorViewModel.Specialization;
            doctorModel.Fee = doctorViewModel.Fee;
            UserErrorStatus status = addDoctor.AddDoctor(doctorModel);
            switch (status)
            {
                case UserErrorStatus.SuccessFull:
                    return RedirectToAction("Dashboard", "Login");
                case UserErrorStatus.UserNameExists:
                    SetViewBagMessage(status, doctorModel.UserDetails.UserName);
                    ViewBag.ErrorMessage = "User Name already Exists";
                    break;
                default:
                    SetViewBagMessage(status, doctorModel.UserDetails.UserName);
                    ViewBag.ErrorMessage = "Error While Adding Patient" + doctorModel.UserDetails.FullName;
                    break;
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin , Nurse")]
        public ActionResult DoctorList()
        {
            ManageUsers manageUsers = new ManageUsers();
            List<DoctorModel> list = manageUsers.DoctorList();
            return View(list);
        }

        [Authorize(Roles = "Admin , Nurse")]
        public ActionResult DoctorDetails(int? Id)
        {
            ManageUsers manageUsers = new ManageUsers();
            DoctorModel doctorModel = new DoctorModel();
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            doctorModel = manageUsers.doctorDetails(Id);


            if (doctorModel == null)
            {
                return HttpNotFound();
            }
            return View(doctorModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteDoctor(int? Id)
        {
            DoctorModel doctorDetails = new DoctorModel();
            ManageUsers manageUsers = new ManageUsers();
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            doctorDetails = manageUsers.doctorDetails(Id);

            if (doctorDetails == null)
            {
                return HttpNotFound();
            }
            return View(doctorDetails);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteDoctorConfirmed(int Id)
        {
            ManageUsers manageUsers = new ManageUsers();
            manageUsers.DoctorDeleteConfrimd(Id);
            return RedirectToAction("DoctorList");
        }

        public ActionResult EditDoctor(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageUsers manageUsers = new ManageUsers();
            EditDoctorViewModel doctorViewModel = DoctorModelToViewModel(Id);


            if (doctorViewModel == null)
            {
                return HttpNotFound();
            }
            return View(doctorViewModel);
        }

        [HttpPost]
        public ActionResult EditDoctor(EditDoctorViewModel doctorViewModel)
        {
            ManageUsers manageUsers = new ManageUsers();
            if (ModelState.IsValid)
            {
                DoctorModel doctorModel = DoctorViewModelToModel(doctorViewModel);
                manageUsers.UpdateDoctor(doctorModel);
                return RedirectToAction("DoctorList");
            }
            return View(doctorViewModel);
        }

        private DoctorModel DoctorViewModelToModel(EditDoctorViewModel doctorViewModel)
        {
            ManageUsers manageUsers = new ManageUsers();
            DoctorModel doctorModel = manageUsers.doctorDetails(doctorViewModel.DoctorId);
            doctorModel.Fee = doctorViewModel.Fee;
            doctorModel.UserDetails.FullName = doctorViewModel.FullName;
            doctorModel.UserDetails.EmailId = doctorViewModel.EmailId;
            doctorModel.UserDetails.Address = doctorViewModel.Address;
            doctorModel.UserDetails.PhoneNo = doctorViewModel.PhoneNo;
            return doctorModel;
        }

        public ActionResult UpdateProfile()
        {
            ManageUsers manageUsers = new ManageUsers();

            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            string UserName = ticket.Name;

            UserModel userDetails = manageUsers.GetUserDetails(UserName);
            ProfileViewModel profileViewModel = UserModelToProfileModel(userDetails);
            return View(profileViewModel);
        }
        
        [HttpPost]
        public ActionResult UpdateProfile(ProfileViewModel profileViewModel)
        {
            ManageUsers manageUsers = new ManageUsers();
            if (ModelState.IsValid)
            {
                UserModel userModel = ProfileModelToUserModel(profileViewModel);
                manageUsers.UpdateProfile(userModel);
                return RedirectToAction("DoctorList");
            }
            return View(profileViewModel);
        }

        private UserModel ProfileModelToUserModel(ProfileViewModel profileViewModel)
        {
            ManageUsers manageUsers = new ManageUsers();
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            UserModel userDetails = manageUsers.GetUserDetails(profileViewModel.FullName);
            userDetails.FullName = profileViewModel.FullName;
            userDetails.Gender = profileViewModel.Gender;
            userDetails.Address = profileViewModel.Address;
            userDetails.City = profileViewModel.City;
            userDetails.EmailId = profileViewModel.EmailId;
            userDetails.PhoneNo = profileViewModel.PhoneNo;
            userDetails.UserName = ticket.UserData.ToString();
            return userDetails;
        }

        private ProfileViewModel UserModelToProfileModel(UserModel userDetails)
        {
            ProfileViewModel profileViewModel = new ProfileViewModel();
            profileViewModel.FullName = userDetails.FullName;
            profileViewModel.Gender = userDetails.Gender;
            profileViewModel.EmailId = userDetails.EmailId;
            profileViewModel.City = userDetails.City;
            profileViewModel.Address = userDetails.Address;
            profileViewModel.PhoneNo = userDetails.PhoneNo;
            return profileViewModel;
        }

        private EditDoctorViewModel DoctorModelToViewModel(int? id)
        {
            ManageUsers manageUsers = new ManageUsers();
            DoctorModel doctorModel = manageUsers.doctorDetails(id);
            EditDoctorViewModel doctorViewModel = new EditDoctorViewModel();
            doctorViewModel.Address = doctorModel.UserDetails.Address;
            doctorViewModel.DoctorId = doctorModel.DoctorId;
            doctorViewModel.EmailId = doctorModel.UserDetails.EmailId;
            doctorViewModel.Fee = doctorModel.Fee;
            doctorViewModel.FullName = doctorModel.UserDetails.FullName;
            doctorViewModel.PhoneNo = doctorModel.UserDetails.PhoneNo;
            return doctorViewModel;
        }

        private void SetViewBagMessage(UserErrorStatus status, string UserName)
        {
            ViewBag.ErrorMessage = "Error While Inserting" + UserName + status.ToString();
        }
        // GET: AddNurse
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AddNurse()
        {
            return View();
        }

        // POST: AddNurse
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddNurse(NurseViewModel nurseViewModel)
        {
            ManageUsers addNurse = new ManageUsers();
            try
            {
                nurseViewModel.UserDetails.Password = Utility.UtilityClass.ComputeSha256Hash(nurseViewModel.UserDetails.Password);
                nurseViewModel.UserDetails.ConfirmPassword = Utility.UtilityClass.ComputeSha256Hash(nurseViewModel.UserDetails.ConfirmPassword);
            }
            catch
            {

            }
            NurseModel nurseModel = new NurseModel();
            nurseModel.UserDetails = new UserModel();
            nurseModel.UserDetails.FullName = nurseViewModel.UserDetails.FullName;
            nurseModel.UserDetails.Gender = nurseViewModel.UserDetails.Gender;
            nurseModel.UserDetails.Address = nurseViewModel.UserDetails.Address;
            nurseModel.UserDetails.PhoneNo = nurseViewModel.UserDetails.PhoneNo;
            nurseModel.UserDetails.EmailId = nurseViewModel.UserDetails.EmailId;
            nurseModel.UserDetails.UserName = nurseViewModel.UserDetails.UserName;
            nurseModel.UserDetails.Password = nurseViewModel.UserDetails.Password;
            
            UserErrorStatus status = addNurse.AddNurse(nurseModel);
            switch (status)
            {
                case UserErrorStatus.SuccessFull:
                    return RedirectToAction("Dashboard", "Login");
                case UserErrorStatus.UserNameExists:
                    SetViewBagMessage(status, nurseModel.UserDetails.UserName);
                    ViewBag.ErrorMessage = "UserName already Exists";
                    break;
                default:
                    SetViewBagMessage(status, nurseModel.UserDetails.UserName);
                    //ViewBag.ErrorMessage = "Error While Adding Patient" + doctorModel.UserDetails.FullName;
                    break;
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult NurseList()
        {
            ManageUsers manageUsers = new ManageUsers();
            List<NurseModel> list = manageUsers.NurseList();
            return View(list);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult NurseDetails(int? Id)
        {
            ManageUsers manageUsers = new ManageUsers();
            NurseModel nurseModel = new NurseModel();
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            nurseModel = manageUsers.NurseDetails(Id);


            if (nurseModel == null)
            {
                return HttpNotFound();
            }
            return View(nurseModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteNurse(int? Id)
        {
            NurseModel nurseDetails = new NurseModel();
            ManageUsers manageUsers = new ManageUsers();
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            nurseDetails = manageUsers.NurseDetails(Id);

            if (nurseDetails == null)
            {
                return HttpNotFound();
            }
            return View(nurseDetails);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteNurseConfirmed(int Id)
        {
            ManageUsers manageUsers = new ManageUsers();
            manageUsers.NurseDeleteConfrimd(Id);
            return RedirectToAction("NurseList");
        }
        public ActionResult EditNurse(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageUsers manageUsers = new ManageUsers();
            EditNurseViewModel nurseViewModel = NurseModelToViewModel(Id);


            if (nurseViewModel == null)
            {
                return HttpNotFound();
            }
            return View(nurseViewModel);
        }

        [HttpPost]
        public ActionResult EditNurse(EditNurseViewModel nurseViewModel)
        {
            ManageUsers manageUsers = new ManageUsers();
            //if (ModelState.IsValid)
           //{
                NurseModel nurseModel = NurseViewModelToModel(nurseViewModel);
                manageUsers.UpdateNurse(nurseModel);
                return RedirectToAction("NurseList");
            //}
            //return View(nurseViewModel);
        }

        private NurseModel NurseViewModelToModel(EditNurseViewModel nurseViewModel)
        {
            ManageUsers manageUsers = new ManageUsers();
            NurseModel nurseModel = manageUsers.NurseDetails(nurseViewModel.NurseID);
            nurseModel.NurseId = nurseViewModel.NurseID;
            nurseModel.UserDetails.Address = nurseViewModel.Address;
            nurseModel.UserDetails.PhoneNo = nurseViewModel.PhoneNo;
            nurseModel.UserDetails.EmailId = nurseViewModel.EmailId;
            nurseModel.UserDetails.FullName = nurseViewModel.FullName;
            return nurseModel;
        }

        private EditNurseViewModel NurseModelToViewModel(int? id)
        {
            ManageUsers manageUsers = new ManageUsers();
            NurseModel nurseModel = manageUsers.NurseDetails(id);
            EditNurseViewModel nurseViewModel = new EditNurseViewModel();
            nurseViewModel.Address = nurseModel.UserDetails.Address;
            nurseViewModel.NurseID = nurseModel.NurseId;
            nurseViewModel.EmailId = nurseModel.UserDetails.EmailId;
            nurseViewModel.FullName = nurseModel.UserDetails.FullName;
            nurseViewModel.PhoneNo = nurseModel.UserDetails.PhoneNo;
            return nurseViewModel;
        }

    }
}
