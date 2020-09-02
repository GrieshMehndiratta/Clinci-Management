using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementBusinessLogic
{
    public enum LoginStatus
    {
        InvalidUserName,
        InvalidPassword,
        Successfull
    }
    public enum UserErrorStatus
    {
        UserNameExists,
        SuccessFull,
        Error
    }

    public enum PrescriptionStatus
    {
        Success,
        AlreadyPrescribed,
        Error,
        AppointmentNotApproved
    }

    public enum InvoiceCreateStatus
    {
        Exists,
        CreatedSuccessfully,
        Error
    }

}
