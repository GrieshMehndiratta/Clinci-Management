using System;
using System.Collections.Generic;
using System.Linq;
using ClinicManagementBusinessLogic;
using ClinicManagementSystemModels.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApteanClinicManagementSystem.Tests.Business
{
    [TestClass]
    public class DoctorTest
    {
        [TestMethod]
        public void TestSuccessFullAddDoctor()
        {
            ManageUsers manageUsers = new ManageUsers();
            DoctorModel doctorModel = AddDoctor();
            UserErrorStatus userErrorStatus = manageUsers.AddDoctor(doctorModel);
            Assert.IsTrue(userErrorStatus == UserErrorStatus.SuccessFull);
        }

        [TestMethod]
        public void TestUserNameExistAddDoctor()
        {
            ManageUsers manageUsers = new ManageUsers();
            DoctorModel doctorModel = AddDoctor();
            UserErrorStatus userErrorStatus = manageUsers.AddDoctor(doctorModel);
            Assert.IsTrue(userErrorStatus == UserErrorStatus.UserNameExists);
        }

        [TestMethod]
        public void TestDoctorList()
        {
            ManageUsers manageUsers = new ManageUsers();
            List<DoctorModel> list = manageUsers.DoctorList();
            Assert.IsTrue(list.Count == 2);
        }

        [TestMethod]
        public void TestGetDoctorsBySpecialization()
        {
            ManageUsers manageUsers = new ManageUsers();
            List<DoctorModel> list = manageUsers.GetDoctors(SpecializationType.Dermatologists.ToString());
            Assert.IsTrue(list.Count == 1);
        }

        [TestMethod]
        public void TestDoctorDetails()
        {
            ManageUsers manageUsers = new ManageUsers();
            DoctorModel doctorModel = manageUsers.doctorDetails(1);
            Assert.IsTrue(doctorModel != null);
        }

        [TestMethod]
        public void TestDoctorDelete()
        {
            ManageUsers manageUsers = new ManageUsers();
            List<DoctorModel> list = manageUsers.DoctorList();
            int id = list.Max(l => l.DoctorId);
            manageUsers.DoctorDeleteConfrimd(id);
            Assert.IsTrue(manageUsers.doctorDetails(id).UserDetails.AccountStatus == true);
        }

        [TestMethod]
        public void UpdateDoctor()
        {
            ManageUsers manageUsers = new ManageUsers();
            List<DoctorModel> list = manageUsers.DoctorList();
            int id = list.Max(l => l.DoctorId);
            DoctorModel doctorModel = UpdateDoctor(id);
            manageUsers.UpdateDoctor(doctorModel);
            Assert.IsTrue(manageUsers.doctorDetails(id).Fee == 10000);
        }
        private DoctorModel UpdateDoctor(int id)
        {
            ManageUsers manageUsers = new ManageUsers();
            DoctorModel doctorModel = manageUsers.doctorDetails(id);
            doctorModel.Fee = 10000;
            return doctorModel;
        }
        private DoctorModel AddDoctor()
        {
            DoctorModel doctorModel = new DoctorModel();
            doctorModel.Fee = 5000;
            doctorModel.Specialization = SpecializationType.Dermatologists;
            UserModel UserDetails = new UserModel();
            UserDetails.AccountStatus = false;
            UserDetails.Address = "kerla";
            UserDetails.City = City.Bangalore;
            UserDetails.EmailId = "Himanshu@gmail.com";
            UserDetails.FullName = "Himanshu";
            UserDetails.Gender = GenderType.Male;
            UserDetails.Password = "Password";
            UserDetails.PhoneNo = "0987654321";
            UserDetails.ResetCode = null;
            UserDetails.UserName = "Himanshu";
            doctorModel.UserDetails = UserDetails;
            return doctorModel;
        }
    }
}
