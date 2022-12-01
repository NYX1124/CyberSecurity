using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Cybersecurity_System.Models;

namespace Online_Cybersecurity_System.Controllers
{
    public class LeaderboardController : Controller
    {
        public string value = "";

        CybersecurityEntities db = new CybersecurityEntities();
        // GET: Leaderboard
        public ActionResult Index()
        {
            var model = db.Scores;
            return View(model);
        }

        public ActionResult AdminIndex()
        {
            var model = db.Scores;
            return View(model);
        }
    }
}