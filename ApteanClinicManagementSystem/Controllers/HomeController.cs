using ClinicManagementSystemModels.Models;
using ClinicManagementBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace ApteanClinicManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpContext.Session["Role"] = "Admin";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        public PartialViewResult GetMenuForUser()
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            string Role = ticket.UserData.ToString();
           
            List<MenuItemsModel> model = MenuItems.Instance.GetMenuItems(Role);
            return PartialView("_MenuItems", model);
        }
    }
}