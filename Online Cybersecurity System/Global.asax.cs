using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using Online_Cybersecurity_System.Models;

namespace Online_Cybersecurity_System
{
    public class MvcApplication : System.Web.HttpApplication
    {
        CybersecurityEntities db = new CybersecurityEntities();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        // TODO: Session_Start()
        //protected void Session_Start()
        //{
        //    if (User.IsInRole("Admin"))
        //    {
        //        Session["image"] = db.Admins.Find(User.Identity.Name).image;
        //    }
        //    else if (User.IsInRole("Employee"))
        //    {
        //        Session["image"] = db.Employees.Find(User.Identity.Name).image;
        //    }


        //}
    }
}
