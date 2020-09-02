using ClinicManagementBusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystemModels.Models;

namespace ApteanClinicManagementSystem.Tests.Business
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void LoginByInvalidUserName()
        {
            UserValidation validate = new UserValidation();
            LoginStatus status = validate.ValidateUser(new UserModel { UserName = " " });
            Assert.AreEqual(status, LoginStatus.InvalidUserName);
        }

        [TestMethod]
        public void LoginByInvalidPassword()
        {
            UserValidation validate = new UserValidation();
            LoginStatus status = validate.ValidateUser(new UserModel { UserName = "Admin",Password = "Password" });
            Assert.AreEqual(status, LoginStatus.InvalidPassword);
        }

        [TestMethod]
        public void ResetPasswordCheck()
        {
            UserValidation validate = new UserValidation();
            bool status = validate.ChangePassword("", "SomeOne");
            Assert.IsFalse(status);
        }

        [TestMethod]
        public void ResetPasswordByInvalidOldPassword()
        {
            UserValidation validate = new UserValidation();
            bool result = validate.ChangePasswordByUser(" ", "Password", "Admin");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ResetPasswordByInvaliduserName()
        {
            UserValidation validate = new UserValidation();
            bool result = validate.ChangePasswordByUser(" ", "Password", " ");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SendResetLinkByInvalidEmailId()
        {
            UserValidation validate = new UserValidation();
            UserModel result = validate.VerifyEmail(" ");
            Assert.AreEqual(null, result);
        }
    }
}
