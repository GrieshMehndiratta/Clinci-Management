using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Services.Description;

namespace ApteanClinicManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null&&authCookie.Value.Length>0)
            {
                var value = FormsAuthentication.Decrypt(authCookie.Value);
                if (string.IsNullOrEmpty(value.ToString()))
                    return;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    //var roles = authTicket.UserData.Split(',');
                    //HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
                    string[] roles = authTicket.UserData.Split(new Char[] { ',' });
                    GenericPrincipal userPrincipal =
                                     new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
                    Context.User = userPrincipal;
                }
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            System.Web.Helpers.AntiForgeryConfig.RequireSsl = HttpContext.Current.Request.IsSecureConnection;
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs args)
        {
            if (Context.User != null)
            {
                //IEnumerable<Role> roles = new UsersService.UsersClient().GetUserRoles(
                //                                        Context.User.Identity.Name);


                //string[] rolesArray = new string[roles.Count()];
                //for (int i = 0; i < roles.Count(); i++)
                //{
                //    rolesArray[i] = roles.ElementAt(i).RoleName;
                //}

                string[] rolesArray = new string[] { "Admin", "Doctor", "Nurse", "Patient" };

                GenericPrincipal gp = new GenericPrincipal(Context.User.Identity, rolesArray);
                Context.User = gp;
            }
        }
        protected void Application_EndRequest(Object sender,EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context.Response.Status.Substring(0, 3).Equals("401"))
            {
                context.Response.ClearContent();
                context.Response.Write("<script language="+"javascript"+">" +
                "self.location='/Login/UnAuthorized';</script>");
            }
            else if(context.Response.StatusCode == 404)
            {
                context.Response.ClearContent();
                context.Response.Write("<script language=" + "javascript" + ">" +
                "self.location='/Login/Error';</script>");
            }
        }

        protected void Application_End(Object sender,EventArgs e)
        {
            FormsAuthentication.SignOut();
        }
        protected void Session_Start(object src, EventArgs e)
        {
            if (Context.Session != null && Context.Session.IsNewSession)
            {
                string sCookieHeader = Request.Headers["Cookie"];
                if (null != sCookieHeader && sCookieHeader.IndexOf("ASP.NET_SessionId") >= 0)
                    Session.Abandon();
            }
        }
        protected void Session_End(Object sender,EventArgs e)
        {
            FormsAuthentication.SignOut();
        }
        protected void Session_OnEnd(Object sender, EventArgs e)
        {
            if(Session.IsNewSession)
            {
                Response.Clear();
                FormsAuthentication.SignOut();
                Response.Clear();
                Response.Redirect("~/Login/Index");
            }
        }

        protected void Application_Error(Object sender,EventArgs e)
        {
            Response.Redirect("~/Login/ServerError");
        }
    }
}
