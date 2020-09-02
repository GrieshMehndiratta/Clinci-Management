using ClinicManagementDataLayer;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementBusinessLogic
{
    public class MedicineValidations
    {
        Medicine medicine = new Medicine();
        ClinicManagementDataLayer.Medicine Obj = new ClinicManagementDataLayer.Medicine();
        public bool MedicineValidation(MedicineModel medicineModel)
        {
            if (Obj.getMedicine(medicineModel.MedicineName) == false)
            { 
                medicine.addMedicine(medicineModel);
                return true;
            }
            return false;

        }

        public List<MedicineModel> MedicineList()
        {
            List<MedicineModel> list = medicine.MedicineList();
            return list;
        }

        public MedicineModel MedicineDetails(int? id)
        {
            MedicineModel editMedicine = Obj.getMedicineDetails(id);
            return editMedicine;
        }

        public void UpdateMedicine(MedicineModel medicineModel)
        {
            medicine.UpdateMedicine(medicineModel);
        }

        public void DeleteMedicine(MedicineModel medicineDetails)
        {
            medicine.DeleteMedicine(medicineDetails);
        }

        public List<string> GetMedicineNames()
        {
            var result = medicine.GetMedicines();
            return result;
        }

        public List<MedicineModel> PrescribeMedicine(int appointmentId)
        {
            return medicine.PrescribedMedicines(appointmentId);
        }
    }
}
