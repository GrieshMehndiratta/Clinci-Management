using ClinicManagementBusinessLogic;
//using ClinicManagementDataLayer;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace ApteanClinicManagementSystem.Controllers
{
    [Authorize]
    public class MedicineController : Controller
    {
        MedicineValidations medicineValidations = new MedicineValidations();
        //Context context = new Context();
        MedicineModel medicineDetails;
        public ActionResult Index()
        {
            List<MedicineModel> list = medicineValidations.MedicineList();
            return View(list);
        }
        // GET: Medicine
        [HttpGet]
        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Create(MedicineModel medicineModel)
        {
            if (ModelState.IsValid)
            {
                if (medicineValidations.MedicineValidation(medicineModel))
                {
                    ViewBag.SuccessMessage = "Successfully Added";
                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Medicine already exist";
                }
            }
            else
            {
                return View(medicineModel);
            }
            return View();
        }
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            medicineDetails = medicineValidations.MedicineDetails(Id);

            if (medicineDetails == null)
            {
                return HttpNotFound();
            }
            return View(medicineDetails);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "MedicineID,MedicineName,MedicineCost")] MedicineModel medicineModel)
        {
            if (ModelState.IsValid)
            {
                medicineValidations.UpdateMedicine(medicineModel);
                return RedirectToAction("Index");
            }
            return View(medicineModel);
        }

        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medicineDetails = medicineValidations.MedicineDetails(Id);

            if (medicineDetails == null)
            {
                return HttpNotFound();
            }
            return View(medicineDetails);
        }

        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            medicineDetails = medicineValidations.MedicineDetails(Id);

            if (medicineDetails == null)
            {
                return HttpNotFound();
            }
            return View(medicineDetails);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            medicineDetails = medicineValidations.MedicineDetails(Id);
            medicineValidations.DeleteMedicine(medicineDetails);
            return RedirectToAction("Index");
        }


    }
}