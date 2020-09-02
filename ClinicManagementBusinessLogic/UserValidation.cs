using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementDataLayer;
using ClinicManagementSystemModels.Models;


namespace ClinicManagementBusinessLogic
{
    public class UserValidation
    {
        public LoginStatus ValidateUser(UserModel userDetails)
        {
            GetUserDetails Details = new GetUserDetails();
            try
            {
                UserModel user = Details.GetUser(userDetails.UserName);
                if (Object.ReferenceEquals(user, null))
                    return LoginStatus.InvalidUserName;
                if (string.Equals(user.Password, userDetails.Password, StringComparison.InvariantCultureIgnoreCase))
                    return LoginStatus.Successfull;
                return LoginStatus.InvalidPassword;
            }
            catch(Exception ex)
            { 
                return LoginStatus.InvalidUserName;
            }
        }
        public string GetRole(string UserName)
        {
            GetUserDetails details = new GetUserDetails();
            try
            {
                List<Roles> roles= details.GetRole(UserName);
                return roles.ElementAt(0).RoleName;
            }
            catch
            {
                return null;
            }
        }
        public UserModel VerifyEmail(string userName)
        {
            GetUserDetails details = new GetUserDetails();
            return details.verifyEmail(userName);
        }
        public void SetResetCode(string UserName,string Code)
        {
            GetUserDetails details = new GetUserDetails();
            details.SetResetCode(UserName, Code);
        }

        public bool ChangePassword(string Code,string Password)
        {
            GetUserDetails details = new GetUserDetails();
            return details.ChangePassword(Password, Code);
        }

        public int GetUserId(string UserName)
        {
            GetUserDetails details = new GetUserDetails();
            return details.GetUserId(UserName);
        }

        public bool ChangePasswordByUser(string oldPassword,string NewPassword,string UserName)
        {
            GetUserDetails userDetails = new GetUserDetails();
            string Password = userDetails.GetOldPassword(UserName);
            if (Password != oldPassword)
                return false;
            return userDetails.UpdatePassword(UserName, NewPassword);
        }
    }
}
