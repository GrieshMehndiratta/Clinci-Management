using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystemModels.Models
{
    #region Enums
    /// <summary>
    /// Gender Of User
    /// </summary>
    public enum GenderType
    {
        Male,
        Female
    };

    /// <summary>
    /// Realtion With Patient
    /// </summary>
    public enum RelationType
    {
        Father,
        Guardian,
        Spouse,
        Friend
    }

    /// <summary>
    /// Specialization Field Of Doctor
    /// </summary>
    public enum SpecializationType
    {
        Heart,
        Dentist,
        ENT,
        Cardiologists,
        CriticalCareMedicineSpecialists,
        Dermatologists,
        FamilyPhysicians,
        Neurologists

    }

    /// <summary>
    /// Account Status of Employee
    /// </summary>
    public enum AccountStatus
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public enum EmplyoeeStatus
    {
        Available,
        NotAvailable
    }

    public enum Role
    {
        Admin=1,
        Doctor,
        Nurse,
        Patient
    }

    /// <summary>
    /// Name of city
    /// </summary>
    public enum City
    {
        Delhi,
        Mumbai,
        Pune,
        Bangalore,
        Shimla,
        Dheradun
    }

    public enum InvoiceStatus
    {
        UnPaid,
        Paid
    }

    public enum Months
    {
        All,
        January,
        Febraury,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        November,
        December
    }
    #endregion
}