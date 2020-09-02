using ApteanClinicManagementSystem.Models;
using ClinicManagementBusinessLogic;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApteanClinicManagementSystem.Controllers
{
    public class EmergencyDetailsController : Controller
    {
        // GET: EmergencyDetails
        public ActionResult Details(int? id)
        {
            ManageUsers manageUsers = new ManageUsers();
            EmergencyContactDetails emergencyContact = manageUsers.EmergencyDetails((int)id);
            EmergencyContactViewModel emergencyDetails = new EmergencyContactViewModel();
            emergencyDetails.Name = emergencyContact.Name;
            emergencyDetails.Relation = emergencyContact.Relation;
            emergencyDetails.PhoneNo = emergencyContact.PhoneNo;
            return View(emergencyDetails);
        }
    }
}