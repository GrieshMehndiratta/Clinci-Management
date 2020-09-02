using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementDataLayer
{
    public class Medicine
    {
        Context DBContext = new Context();
        public bool getMedicine(string MedicineName)
        {
            try
            {
                MedicineModel medicine = DBContext.Medicines.Single(m => m.MedicineName == MedicineName);
                if (medicine == null)
                    throw new Exception("Medicine not found");
            }
            catch(Exception ex)
            {
                return false;
            }
              return true;
        }

        public void addMedicine(MedicineModel medicineModel)
        {
            DBContext.Dispose();
            using (var dbContext = new Context())
            {
                dbContext.Medicines.Add(medicineModel);
                dbContext.SaveChanges();
            }
            //DBContext.Medicines.Add(medicineModel);
            //DBContext.SaveChanges();
        }

        public List<MedicineModel> MedicineList()
        {
            return DBContext.Medicines.ToList();
        }

        //TODO Select Medicines Which are not Prescribed
        public List<MedicineModel> PrescribedMedicines(int AppointmentId)
        {
            List<MedicineModel> medicines = new List<MedicineModel>();
            //int prescriptionId = int.Parse(DBContext.Prescriptions.Where(m => m.AppointmentId == AppointmentId).Select(m => m.PrescriptionId).ToString());
            //List<int> MedicineID = DBContext.PrescribedMedicines.Where(m => m.PrescriptionId == prescriptionId).Select(m => m.MedicineId).ToList();
            var PrescribedMedicines = from medicine in DBContext.Medicines
                                      join prescribed in DBContext.PrescribedMedicines
                                      on medicine.MedicineID equals prescribed.MedicineId
                                      where prescribed.PrescriptionId == AppointmentId
                                      select (medicine);
            List<MedicineModel> pres = PrescribedMedicines.ToList();
            medicines = DBContext.Medicines.ToList();
            medicines = medicines.Except(pres).ToList();
            return medicines;
        }
        public MedicineModel getMedicineDetails(int? id)
        {
            var medicineDetails = DBContext.Medicines.Where(m => m.MedicineID == id).FirstOrDefault();
            return medicineDetails;
        }

        public void DeleteMedicine(MedicineModel medicineDetails)
        {
            DBContext.Medicines.Remove(DBContext.Medicines.Where(m=>m.MedicineID==medicineDetails.MedicineID).FirstOrDefault());
            DBContext.SaveChanges();
        }

        public void UpdateMedicine(MedicineModel medicineModel)
        {
            DBContext.Entry(medicineModel).State = EntityState.Modified;
            DBContext.SaveChanges();
        }

        public List<string> GetMedicines()
        {
            var result = from Medicines in DBContext.Medicines
                                  orderby Medicines.MedicineName
                                  select Medicines.MedicineName;
            return result.ToList();

        }
        ~Medicine()
        {
            if (DBContext != null)
                DBContext.Dispose();
        }
    }
}
