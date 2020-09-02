using ApteanClinicManagementSystem.Models;
using ClinicManagementBusinessLogic;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ApteanClinicManagementSystem.Controllers
{
    public class MedicalHistoryController : Controller
    {
        #region Members 
        private MedicalHistoryBuisnessLogic medicalHistoryBL = new MedicalHistoryBuisnessLogic();

        #endregion

        #region Methods

        /// <summary>
        /// Form To Create Medical History 
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <returns>Form to be filled with medical observation</returns>
        [HttpGet]
        [Authorize]
        public ActionResult Create(int? id)
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            bool isAllowed=true;
            bool isExist = medicalHistoryBL.CheckMedicalHistory((int)id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (isExist)
            {
                ViewBag.Message = "Medical Observation Already Added";
                return View("PrescribedMedicineMessage","Appointment");
            }
            if (!ticket.UserData.ToString().Equals("Admin"))
            {
                isAllowed = medicalHistoryBL.CheckDoctorId((int)id, ticket.Name);
            }
            
            if (isAllowed)
            {
                MedicalHistoryViewModel model = new MedicalHistoryViewModel();
                model.AppointmentId = (int)id;
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Create Medical History
        /// </summary>
        /// <param name="medicalHistory">Model of Medical History</param>
        /// <returns>Still to be figure out</returns>
        [HttpPost]
        public ActionResult Create(MedicalHistoryViewModel medicalHistory)
        {
            MedicalHistoryModel medicalHistoryModel = new MedicalHistoryModel() { Diagnosis = medicalHistory.Diagnosis, Medicines = medicalHistory.Medicines, ClinicRemarks = medicalHistory.ClinicRemarks };
            medicalHistoryBL.AddMedicalHistory(medicalHistoryModel, medicalHistory.AppointmentId);
            return RedirectToAction("DeleteAppointment","Appointment",new { id = medicalHistory.AppointmentId });
        }

        /// <summary>
        /// Show All the Medical History 
        /// </summary>
        /// <returns>View Containing all medical history</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var medicalHistories = medicalHistoryBL.ShowAllMedicalHistory();
            return View(medicalHistories);
        }

        /// <summary>
        /// Show medical history details of specific patient
        /// </summary>
        /// <param name="id">Patient Id</param>
        /// <returns>View showing all the medical history of patient details till date</returns>

        [HttpGet]
        [Authorize(Roles = "Patient")]
        public ActionResult PatientDetails()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            int id = medicalHistoryBL.GetPatientId(ticket.Name);
            if (id == 0)
            {
                return HttpNotFound();
            }
            var medicalHistory = medicalHistoryBL.PatientViewOfMedicalHistory(id);
            if (medicalHistory == null)
            {
                return HttpNotFound();
            }
            return View("Details",medicalHistory);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Doctor")]
        public ActionResult PatientDetailsForList(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var medicalHistory = medicalHistoryBL.PatientViewOfMedicalHistory((int)id);
            if (medicalHistory == null)
            {
                return HttpNotFound();
            }
            return View(medicalHistory);
        }
        /// <summary>
        /// Show medical history details
        /// </summary>
        /// <param name="id">MedicalHistoryId</param>
        /// <returns>View displaying the details of medical history</returns>
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var medicalHistory = medicalHistoryBL.GetMedicalHistoryByMedicalHistoryId(id);
            if (medicalHistory == null)
            {
                return HttpNotFound();
            }
            return View(medicalHistory);
        }

        /// <summary>
        /// Edit Page to edit medical history
        /// </summary>
        /// <param name="id">MedicalHistoryId</param>
        /// <returns>Pass the View Consisting new Medical History Model</returns>
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalHistoryModel medicalHistory;
            medicalHistory = medicalHistoryBL.MedicalHistoryDetails(id);

            if (medicalHistory == null)
            {
                return HttpNotFound();
            }
            return View(medicalHistory);
        }

        /// <summary>
        /// Edit Medical History
        /// </summary>
        /// <param name="editMedicalHistory">Model Of Medical History</param>
        /// <returns>Redirect to Index</returns>
        [HttpPost]
        public ActionResult Edit(MedicalHistoryModel editMedicalHistory)
        {
            if (ModelState.IsValid)
            {
                medicalHistoryBL.UpdateMedicalHistory(editMedicalHistory);
                return RedirectToAction("Index");
            }
            return View(editMedicalHistory);
        }

        #endregion
    }
}