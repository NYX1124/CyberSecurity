using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Cybersecurity_System.Models;

namespace Online_Cybersecurity_System.Controllers
{
    public class AchievementBadgesController : Controller
    {
        CybersecurityEntities db = new CybersecurityEntities();
        // GET: AchievementBadges
        public ActionResult Index()
        {
            var model = db.AchievementBadges;
            return View(model);
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost, ActionName("Insert"), ValidateAntiForgeryToken]
        public ActionResult Insert([Bind(Include = "id, achievementTitle, achievementDescription, achievementStatus, createDate")] AchievementBadge AchievementBadge)
        {
            if (ModelState.IsValid)
            {
                //using (var databaseContext = new CybersecurityEntities())
                //{
                //    AchievementBadge a = new AchievementBadge();

                //    a.id = model.id;
                //    a.achievementTitle = model.achievementTitle;
                //    a.achievementDescription = model.achievementDescription;
                //    a.achievementStatus = model.achievementStatus;
                //    a.createDate = model.createDate;

                    db.AchievementBadges.Add(AchievementBadge);
                    db.SaveChanges();
                return RedirectToAction("Index");
            }
                //ViewBag.Message = "Details inserted!";
                

            //}
            //else
            //{
                return View(AchievementBadge);
            //}
        }

        public ActionResult Detail(int id)
        {
            var model = db.AchievementBadges.Find(id);

            if (model == null)
            {
                return RedirectToAction("Index", "AchievementBadges");
            }
            return View();
        }

        public ActionResult CheckId(Int32 id)
        {
            bool result = db.AchievementBadges.Find(id) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("UpdateAchievementBadge/{id}")]
        public ActionResult UpdateAchievementBadge(Int32 id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            AchievementBadge a = db.AchievementBadges.Find(id);

            if(a == null)
            {
                return HttpNotFound();
            }

            return View(a);
            //var model = db.AchievementBadges.Find(id);

            //if (model == null)
            //{
            //    return RedirectToAction("Index", "AchievementBadges");
            //}
            //return View();
        }

        // POST: Home/Edit
        [HttpPost, ActionName("UpdateAchievementBadge")]
        public ActionResult Update([Bind(Include = "id, achievementTitle, achievementDescription, achievementStatus, createDate")] AchievementBadge a)
        {
            if (ModelState.IsValid)
            {
                db.Entry(a).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(a);

            //var a = db.AchievementBadges.Find(model.id);

            //if (a == null)
            //{
            //    return RedirectToAction("Index", "AchievementBadges");
            //}

            //if (ModelState.IsValid)
            //{
            //    using (var databaseContext = new CybersecurityEntities())
            //    {
            //        a.achievementTitle = model.achievementTitle;
            //        a.achievementDescription = model.achievementDescription;
            //        a.achievementStatus = model.achievementStatus;
            //        a.createDate = model.createDate;

            //        databaseContext.SaveChanges();
            //    }
            //    TempData["Info"] = "Achievement Badge record edited.";
            //    return View(model);
            //}
            //else
            //{
            //    return View("Index", model);
            //}
        }

        public ActionResult Delete(Int32 id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            AchievementBadge a = db.AchievementBadges.Find(id);
            if (a == null)
            {
                return HttpNotFound();
            }
            return View(a);
        }

        // POST: Home/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAchievementBadge(Int32 id)
        {
            AchievementBadge a = db.AchievementBadges.Find(id);
            db.AchievementBadges.Remove(a);
            db.SaveChanges();
            return RedirectToAction("Index");
            //var a = db.AchievementBadges.Find(id);

            //if (a != null)
            //{
            //    db.AchievementBadges.Remove(a);
            //    db.SaveChanges();
            //    TempData["Info"] = "Achievement Badge record deleted.";
            //}

            //var url = Request.UrlReferrer?.AbsolutePath ?? "/";
            //return Redirect(url);
        }
    }
}