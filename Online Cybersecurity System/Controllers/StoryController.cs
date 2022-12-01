using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Cybersecurity_System.Models;
using System.Text.RegularExpressions;

namespace Online_Cybersecurity_System.Controllers
{
     public class StoryController : Controller
     {
          CybersecurityEntities db = new CybersecurityEntities();

          // GET: Story
          public ActionResult Index()
          {
               var model = db.Storys;
               return View(model);
          }

          [Route("Detail/{id}")]
          public ActionResult Detail()
          {
               return View("");
          }

          [HttpGet]
          public ActionResult Detail(Int32 id)
          {
               if (id == null)
               {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
               }

               Story s = db.Storys.Find(id);

               if (s == null)
               {
                    return HttpNotFound();
               }

               return View(s);
          }

          public ActionResult InsertStory()
          {
               return View();
          }

          [HttpPost]
          public ActionResult InsertStory(StoryVM model)
          {
               if (ModelState.IsValid)
               {
                    using (var databaseContext = new CybersecurityEntities())
                    {
                         Story s = new Story();

                         s.name = model.name;
                         s.src = model.src;
                         s.description = model.description;
                         s.storyStatus = model.storyStatus;
                         s.createDate = model.createDate;

                         databaseContext.Storys.Add(s);
                         databaseContext.SaveChanges();
                    }
                    ViewBag.Message = "Story details inserted!";
                    return RedirectToAction("Index", "Story");
               }
               else
               {
                    return View("Index", model);
               }
          }

          //Get: check id
          public ActionResult CheckId(Int32 id)
          {
               bool result = db.Storys.Find(id) == null;
               return Json(result, JsonRequestBehavior.AllowGet);
          }

          [Route("UpdateStory/{id}")]
          public ActionResult UpdateStory(Int32 id)
          {
               if (id == null)
               {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
               }

               Story s = db.Storys.Find(id);

               if (s == null)
               {
                    return HttpNotFound();
               }

               return View(s);
          }

          // POST: Home/Edit
          [HttpPost, ActionName("UpdateStory")]
          public ActionResult Update([Bind(Include = "id, name, src, description, storyStatus, createDate")] Story s)
          {
               if (ModelState.IsValid)
               {
                    db.Entry(s).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Story");
               }
               return View(s);
          }

          public ActionResult Delete(Int32 id)
          {
               if (id == null)
               {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
               }
               Story s = db.Storys.Find(id);
               if (s == null)
               {
                    return HttpNotFound();
               }
               return View(s);
          }

          // POST: Home/Delete
          [HttpPost, ActionName("Delete")]

          public ActionResult DeleteStory(Int32 id)
          {
               Story s = db.Storys.Find(id);
               db.Storys.Remove(s);
               db.SaveChanges();
               return RedirectToAction("Index");
          }
     }
}