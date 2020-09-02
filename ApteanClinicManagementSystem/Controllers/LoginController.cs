using ApteanClinicManagementSystem.Models;
using System.Web.Mvc;
using ClinicManagementBusinessLogic;
using System.Web.Security;
using ClinicManagementSystemModels.Models;
using System.Collections.Generic;
using System.Collections;
using System.Web.Caching;
using System;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace ApteanClinicManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            try
            {
                var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                ViewBag.Role = ticket.UserData.ToString();
                return RedirectToAction("Dashboard", "Login");
            }
            catch (Exception ex)
            {
                return PartialView("Index");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserValidation validation = new UserValidation();
                UserModel user = new UserModel();
                user.UserName = model.UserName;
                user.Password = Utility.UtilityClass.ComputeSha256Hash(model.Password);
                LoginStatus status = validation.ValidateUser(user);
                if (status == LoginStatus.Successfull)
                {
                    string UserRoles = validation.GetRole(user.UserName);
                    user.UserId = validation.GetUserId(user.UserName);
                    int minutes = 30;
                    if (model.RememberMe)
                        minutes = 1440 * 7;
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(user.UserId, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(minutes), model.RememberMe, UserRoles, "/");
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                   FormsAuthentication.Encrypt(authTicket));
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Dashboard", "Login");
                }
                else
                {
                    ViewBag.ErrorMessage = status.ToString();
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult DashBoard()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

            if (ticket != null)
            {
                StatisticsModelView view = new StatisticsModelView();
                ManageUsers users = new ManageUsers();
                List<int> stats = users.getStats();
                view.TotalPatientsCount = stats[0];
                view.PatientsCount = stats[1];
                view.TotalAppoinmentsCount = stats[2];
                view.AppointmentsCount = stats[3];
                view.TotalDoctorsCount = stats[4];
                view.DoctorsCount = stats[5];
                view.TotalUsersCount = stats[6];
                view.ActiveUsers = stats[7];

                ViewBag.Role = ticket.UserData.ToString();
                if (ticket.UserData.ToString().Equals("Patient"))
                {
                    return View("PatientDashboard");
                }
                else
                {
                    return View(view);
                }
            }
            return RedirectToAction("Index", "Login");
        }

        //[Authorize]
        public ActionResult LogOff()
        {
            ClearApplicationCache();
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        public void ClearApplicationCache()
        {
            List<string> keys = new List<string>();

            Cache cache = new Cache();
            IDictionaryEnumerator enumerator = cache.GetEnumerator();

            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            for (int i = 0; i < keys.Count; i++)
            {
                cache.Remove(keys[i]);
            }
        }

        //[HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string EmailID)
        {
            UserValidation validation = new UserValidation();
            UserModel user = validation.VerifyEmail(EmailID);
            if (user == null)
            {
                ViewBag.Message = "UserName/Email Doesn't Exists";
                return View();
            }
            ViewBag.Message = "Reset Password Email has been sent to Registered Email Account";
            string resetCode = Guid.NewGuid().ToString();
            validation.SetResetCode(user.UserName, resetCode);
            SendVerificationLinkEmail(user.EmailId, resetCode);
            return View();
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/Login/" + "ChangePassword" + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("oeeoeeoee41@gmail.com", "Hacked");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "oeeoee@41"; // Replace with actual password

            string subject = "";
            string body = "";
            subject = "Reset Password";
            body = "Hi,<br/>br/>We got request for reset your account password. Please click on the below link to reset your password" +
                "<br/><br/><a href=" + link + ">Reset Password link</a>";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        [HttpGet]
        public ActionResult ChangePassword(string id)
        {
            if (id == null)
                return HttpNotFound("Page Not Found");
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.ResetCode = id;
            return View(model);
        }

        [Authorize]
        public ActionResult UserChangePassword()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            string UserName = ticket.Name;
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.UserName = UserName;
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UserChangePassword(ChangePasswordViewModel model)
        {
            UserValidation validate = new UserValidation();
            if (!(string.IsNullOrEmpty(model.OldPassword) && string.IsNullOrEmpty(model.NewPassword)))
            {
                model.OldPassword = Utility.UtilityClass.ComputeSha256Hash(model.OldPassword);
                model.NewPassword = Utility.UtilityClass.ComputeSha256Hash(model.NewPassword);
            }
            if (validate.ChangePasswordByUser(model.OldPassword, model.NewPassword, model.UserName))
                ViewBag.Message = "Password Updated Successfully";
            else
                ViewBag.Error = "Incorrect Password Was Provided";
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserValidation validation = new UserValidation();
                if (validation.ChangePassword(model.ResetCode, Utility.UtilityClass.ComputeSha256Hash(model.Password)))
                    return RedirectToAction("Index", "Login");
                ViewBag.Message = "Error while Changing Password";
            }
            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            return View("MyErrorPage");
        }
    }
}