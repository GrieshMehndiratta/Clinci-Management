using ClinicManagementDataLayer;
using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementBusinessLogic
{
    public class Invoice
    {
        InvoiceDataAccess Invoices = new InvoiceDataAccess();
        GetUserDetails userDetails = new GetUserDetails();

        public InvoiceCreateStatus CreateInvoice(int PrescriptionId)
        {
            if (Invoices.CheckInvoiceByPrescription(PrescriptionId))
                return InvoiceCreateStatus.Exists;
            if (Invoices.CreateInvoice(PrescriptionId))
                return InvoiceCreateStatus.CreatedSuccessfully;
            return InvoiceCreateStatus.Error;
        }

        public InvoiceModel GetInvoiceByAppointment(int AppointmentId)
        {
            return Invoices.GetInvoiceByAppointment(AppointmentId);
        }

        public PatientModel GetPatientByInvoice(int InvoiceId)
        {
            return Invoices.GetPatientByInvoice(InvoiceId);
        }

        public List<MedicineInvoiceViewModel> GetMedicines(int PrescriptionId)
        {
            return Invoices.GetPrescribedMedicines(PrescriptionId);
        }

        public List<InvoiceModel> GetInvoices(int UserId)
        {
            List<Roles> roles = userDetails.GetRoles(UserId);
            var UserRole = roles.SingleOrDefault(role => role.RoleName.Equals("Admin",StringComparison.InvariantCultureIgnoreCase) || role.RoleName.Equals("Nurse",StringComparison.InvariantCultureIgnoreCase));
            if (UserRole!=null)
            {
                return Invoices.GetAllInvoices();
            }
            else
            {
                return Invoices.GetInvoicesById(UserId);
            }
        }

        public int GetAppointmentId(int InvoiceId)
        {
            return Invoices.GetAppointmentByInvoice(InvoiceId);
        }

        public bool DeleteInvoice(int InvoiceId)
        {
            return Invoices.DeleteInvoice(InvoiceId);
        }

        public bool TakePayment(int InvoiceId)
        {
            return Invoices.TakePayment(InvoiceId);
        }
    }
}
