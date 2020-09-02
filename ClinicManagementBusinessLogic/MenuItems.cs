using ClinicManagementSystemModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ClinicManagementBusinessLogic
{
    public class MenuItems
    {
        private static MenuItems instance = null;
        public static MenuItems Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuItems();
                }
                return instance;
            }
        }

        private List<MenuItemsModel> menuItemsList;

        
        public List<MenuItemsModel> GetMenuItems(string newRole)
        {

            //check if dict key exists and value for the key exists
            //if exists pick dict value and return
            //if not exists call GetMenuItemsFOrAdmin and store in dict for further use

            Role role = (Role)Enum.Parse(typeof(Role), newRole, true);
            if(dictMenuItems.Keys.Contains(role))
            {
                if(dictMenuItems[role]!=null)
                {
                    return dictMenuItems[role];
                }
                else
                {
                    menuItemsList = new List<MenuItemsModel>();
                    switch (role)
                    {
                        case Role.Admin:
                            SetAdminMenuItems();
                            break;
                        case Role.Doctor:
                            SetDoctorMenuItems();
                            break;
                        case Role.Nurse:
                            SetNurseMenuItems();
                            break;
                        case Role.Patient:
                            SetPatientMenuItems();
                            break;
                    }
                    dictMenuItems[role] = menuItemsList;
                }
            }
            else
            {
                throw new Exception("Menu items for given role '" + newRole + "' is not defined");
            }

            return dictMenuItems[role];
        }

        private Dictionary<Role, List<MenuItemsModel>> dictMenuItems;

        private MenuItems()
        {
            dictMenuItems = new Dictionary<Role, List<MenuItemsModel>>()
            {
                {Role.Admin,null },
                {Role.Doctor,null},
                {Role.Nurse,null },
                {Role.Patient,null },
            };

        }
        private void SetAdminMenuItems()
        {
            MenuItemsModel menuItem = new MenuItemsModel("Manage Users", "fab fa-dropbox", "ManageUsers");
            menuItem.ChildMenuItems = new List<MenuItemsModel>();
            menuItem.ChildMenuItems.Add(new MenuItemsModel("Add Patient", "glyphicon glyphicon-plus", "/CreateUsers/AddPatient"));
            menuItem.ChildMenuItems.Add(new MenuItemsModel("Add Doctor", "glyphicon glyphicon-plus", "/CreateUsers/AddDoctor"));
            menuItem.ChildMenuItems.Add(new MenuItemsModel("Add Nurse", "glyphicon glyphicon-plus", "/CreateUsers/AddNurse"));
            menuItemsList.Add(menuItem);

            menuItem = new MenuItemsModel("Add Medicine", "glyphicon glyphicon-plus", "/Medicine/Create");
            menuItemsList.Add(menuItem);

            menuItemsList.Add(new MenuItemsModel("Patient List", "fas fa-bed", "/CreateUsers/PatientList"));
            menuItemsList.Add(new MenuItemsModel("Doctor List", "fas fa-bed", "/CreateUsers/DoctorList"));
            menuItemsList.Add(new MenuItemsModel("Nurse List", "fas fa-bed", "/CreateUsers/NurseList"));
            menuItemsList.Add(new MenuItemsModel("Medicine List", "fas fa-notes-medical", "/Medicine/"));
            menuItemsList.Add(new MenuItemsModel("Appointment List", "fas fa-bed", "/Appointment/ViewAppointments"));

            menuItem = new MenuItemsModel("Manage Appointments", "glyphicon glyphicon-earphone", "ManageAppointments");
            menuItem.ChildMenuItems = new List<MenuItemsModel>();
            menuItem.ChildMenuItems.Add(new MenuItemsModel("Add Appointment", "glyphicon glyphicon-plus", "/Appointment/CreateAppointmentView"));
            menuItemsList.Add(menuItem);

            menuItem = new MenuItemsModel("Manage History", "fas fa-th-list", "ManageHistory");
            menuItem.ChildMenuItems = new List<MenuItemsModel>();
            menuItem.ChildMenuItems.Add(new MenuItemsModel("Medical History", "fas fa-book-medical", "/MedicalHistory/Index"));
            menuItemsList.Add(menuItem);

            menuItem = new MenuItemsModel("Manage Finance", "fas fa-landmark", "ManageFinance");
            menuItem.ChildMenuItems = new List<MenuItemsModel>();
            //Dummy
            menuItem.ChildMenuItems.Add(new MenuItemsModel("All Invoices", "fas fa-file-invoice-dollar", "/Reports/PatientsAllInvoices"));
            menuItem.ChildMenuItems.Add(new MenuItemsModel("Outstanding Invoices", "far fa-list-alt", "/Reports/PatientReports"));
            menuItem.ChildMenuItems.Add(new MenuItemsModel("Report", "fas fa-notes-medical", "/Reports/MonthlyReport"));
            menuItemsList.Add(menuItem);
        }
        private void SetDoctorMenuItems()
        {
            menuItemsList.Add(new MenuItemsModel("Appointment List", "fas fa-bed", "/Appointment/ViewAppointments"));
        }
        private void SetNurseMenuItems()
        {
            menuItemsList.Add(new MenuItemsModel("Appointment List", "fas fa-bed", "/Appointment/ViewAppointments"));
            menuItemsList.Add(new MenuItemsModel("My Appointments", "fas fa-notes-medical", "/Appointment/NurseAppointments"));
            menuItemsList.Add(new MenuItemsModel("Payment", "fas fa-rupee-sign", "/Invoice/Payment"));
            menuItemsList.Add(new MenuItemsModel("Invoice List", "fas fa-clipboard-list", "/Invoice/ListInvoices"));
        }
        private void SetPatientMenuItems()
        {
            menuItemsList.Add(new MenuItemsModel("Appointment List", "fas fa-bed", "/Appointment/ViewAppointments"));

            MenuItemsModel menuItem = new MenuItemsModel("Manage Appointments", "glyphicon glyphicon-earphone", "ManageAppointments");
            menuItem.ChildMenuItems = new List<MenuItemsModel>();
            menuItem.ChildMenuItems.Add(new MenuItemsModel("Add Appointment", "glyphicon glyphicon-plus", "/Appointment/CreateAppointmentView"));
            menuItemsList.Add(menuItem);

            menuItemsList.Add(new MenuItemsModel("Invoice List", "fas fa-file-invoice-dollar", "/Invoice/ListInvoices"));

            menuItem = new MenuItemsModel("Manage History", "fas fa-th-list", "ManageHistory");
            menuItem.ChildMenuItems = new List<MenuItemsModel>();
            menuItem.ChildMenuItems.Add(new MenuItemsModel("Medical History", "fas fa-book-medical", "/MedicalHistory/PatientDetails"));
            menuItemsList.Add(menuItem);
        }

    }
}
