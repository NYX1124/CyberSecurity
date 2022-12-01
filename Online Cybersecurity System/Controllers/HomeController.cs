using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Cybersecurity_System.Models;
using System.Security.Cryptography;


namespace Online_Cybersecurity_System.Controllers
{

    public class HomeController : Controller
    {
        public string value = "";

        CybersecurityEntities db = new CybersecurityEntities();
        // GET: Home
        public ActionResult Index()
        {
            if(Session["Id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Quiz");
            }
        }

    }
}