using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Cybersecurity_System.Models;
using System.Text.RegularExpressions;

namespace Online_Cybersecurity_System.Controllers
{
    public class QuizController : Controller
    {
        CybersecurityEntities db = new CybersecurityEntities();

        // GET: Quiz
        public ActionResult Index()
        {
            var model = db.Quizs;
            return View(model);
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Detail()
        {
            return View("");
        }

        [HttpGet]
        public ActionResult Detail(string quizId)
        {
            var model = db.Quizs.Find(quizId);

            if (model == null)
            {
                return RedirectToAction("Index", "Quiz");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult InsertQuiz(Category model)
        {
            //int sid = Convert.ToInt32(Session["Username"]);
            //List<Category> li = db.Categories.Where(x => x.Id == sid).ToList();
            List<Category> li = db.Categories.ToList();
            ViewBag.list = new SelectList(li, "Id", "Name");
            populateDropDowns();
            return View();
        }

        private void populateDropDowns()
        {
            List<string> difficulty = new List<string>();
            difficulty.Add("Easy"); difficulty.Add("Intermediate"); difficulty.Add("Hard");

            List<string> quizStatus = new List<string>();
            quizStatus.Add("Active"); quizStatus.Add("Inactive");

            ViewBag.difficulty = new SelectList(difficulty);
            ViewBag.status = new SelectList(quizStatus);
        }

        [HttpPost]
        public ActionResult InsertQuiz(QuizVM model)
        {
            if (ModelState.IsValid)
            {
                using (var databaseContext = new CybersecurityEntities())
                {
                    List<Category> li = db.Categories.ToList();
                    ViewBag.list = new SelectList(li, "Id", "Name");

                    Quiz q = new Quiz();

                    q.id = model.id;
                    q.description = model.description;
                    q.difficulty = model.difficulty;
                    q.choiceA = model.choiceA;
                    q.choiceB = model.choiceB;
                    q.choiceC = model.choiceC;
                    q.choiceD = model.choiceD;
                    q.correct_answer = model.correct_answer;
                    q.quizStatus = model.quizStatus;
                    q.createDate = model.createDate;
                    q.CategoryId = (int)model.CategoryId;

                    databaseContext.Quizs.Add(q);
                    databaseContext.SaveChanges();
                }
                populateDropDowns();
                ViewBag.Message = "Quiz details inserted!";
                return View("Index", "Quiz");
            }
            else
            {
                return View("Index", model);
            }



            //if (db.Quizs.Find(model.quizId) != null)
            //{

            //}

            //if (ModelState.IsValid)
            //{
            //    var q = new Quiz
            //    {
            //        quizId = model.quizId,
            //        description = model.description,
            //        choiceA = model.choiceA,
            //        choiceB = model.choiceB,
            //        choiceC = model.choiceC,
            //        choiceD = model.choiceD,
            //        correct_answer = model.correct_answer,
            //        quizStatus = model.quizStatus,
            //        createDate = model.createDate,
            //    };

            //    db.Quizs.Add(q);
            //    db.SaveChanges();
            //}
            //return View(model);

            //HttpPostedFileBase img;
            //string path;
            //string filename;




            //using (var context = new CybersecurityEntities())
            //{
            //    var q = new Quiz
            //    {
            //        description = model.description,
            //        choiceA = model.choiceA,
            //        choiceB = model.choiceB,
            //        choiceC = model.choiceC,
            //        choiceD = model.choiceD,
            //        correct_answer = model.correct_answer,
            //        quizStatus = model.quizStatus,
            //        createDate = model.createDate,
            //    };
            //    context.Quizs.Add(q);
            //    context.SaveChanges();
            //}
            //return View();
        }

        //Get: check id
        public ActionResult CheckId(Int32 id)
        {
            bool result = db.Quizs.Find(id) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("UpdateQuiz/{id}")]
        public ActionResult UpdateQuiz(Int32 id)
        {
            var list = db.Categories.ToList();
            ViewBag.CategoryId = new SelectList(list, dataValueField: "Id", dataTextField: "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Quiz q = db.Quizs.Find(id);

            if (q == null)
            {
                return HttpNotFound();
            }

            return View(q);
            //var model = db.Quizs.Find(id);

            //if (model == null)
            //{
            //    return RedirectToAction("Index", "Quiz");
            //}
            //return View();
        }

        // POST: Home/Edit
        [HttpPost, ActionName("UpdateQuiz")]
        public ActionResult Update([Bind(Include = "id, description, difficulty, choiceA, choiceB, choiceC, choiceD, correct_answer, quizStatus, createDate, CategoryId")] Quiz q)
        {
            if (ModelState.IsValid)
            {
                var list = db.Categories.ToList();
                ViewBag.CategoryId = new SelectList(list, dataValueField: "Id", dataTextField: "Name");

                db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(q);
            //var q = db.Quizs.Find(model.id);

            //if (q == null)
            //{
            //    return RedirectToAction("Index", "Quiz");
            //}

            //if (ModelState.IsValid)
            //{
            //    using (var databaseContext = new CybersecurityEntities())
            //    {
            //        q.description = model.description;
            //        q.choiceA = model.choiceA;
            //        q.choiceB = model.choiceB;
            //        q.choiceC = model.choiceC;
            //        q.choiceD = model.choiceD;
            //        q.correct_answer = model.correct_answer;
            //        q.quizStatus = model.quizStatus;
            //        q.createDate = model.createDate;

            //        databaseContext.SaveChanges();
            //    }
            //    ViewBag.Message = "Quiz details updated!";
            //    return View(model);
            //}
            //else
            //{
            //    return View("Index", model);
            //}

            //if (ModelState.IsValid)
            //{
            //    q.description = model.description;
            //    q.choiceA = model.choiceA;
            //    q.choiceB = model.choiceB;
            //    q.choiceC = model.choiceC;
            //    q.choiceD = model.choiceD;
            //    q.correct_answer = model.correct_answer;
            //    q.quizStatus = model.quizStatus;
            //    q.createDate = model.createDate;

            //    db.SaveChanges();
            //    TempData["Info"] = "Quiz record edited.";
            //    return RedirectToAction("Index", "Quiz");
            //}
            //return View(model);
        }

        public ActionResult Delete(Int32 id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Quiz q = db.Quizs.Find(id);
            if (q == null)
            {
                return HttpNotFound();
            }
            return View(q);
        }

        // POST: Home/Delete
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteQuiz(Int32 id)
        {
            Quiz q = db.Quizs.Find(id);
            db.Quizs.Remove(q);
            db.SaveChanges();
            return RedirectToAction("Index");
            //var s = db.Quizs.Find(id);

            //if (s != null)
            //{
            //    db.Quizs.Remove(s);
            //    db.SaveChanges();
            //    TempData["Info"] = "Quiz record deleted.";
            //}

            //var url = Request.UrlReferrer?.AbsolutePath ?? "/";
            //return Redirect(url);
        }

        //public ActionResult IndexCategory()
        //{
        //    var model = db.Categories;
        //    return View(model);
        //}

        public ActionResult InsertCategory()
        {
            List<Category> cat1 = db.Categories.OrderByDescending(x => x.CategoryId).ToList();
            ViewData["list"] = cat1;

            return View();
        }

        [HttpPost]
        public ActionResult InsertCategory(Category model)
        {

            List<Category> cat1 = db.Categories.OrderByDescending(x => x.CategoryId).ToList();
            ViewData["list"] = cat1;

            Category c = new Category();
            c.CategoryId = model.CategoryId;
            c.CategoryName = model.CategoryName;

            db.Categories.Add(model);
            db.SaveChanges();

            return RedirectToAction("InsertCategory");
        }

        //Get: check id
        public ActionResult CheckCategoryId(Int32 Id)
        {
            bool result = db.Categories.Find(Id) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("UpdateQuiz/{id}")]
        public ActionResult UpdateCategory(Int32 Id)
        {
            var list = db.Categories.ToList();
            ViewBag.CategoryId = new SelectList(list, dataValueField: "Id", dataTextField: "Name");

            if (Id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Category c = db.Categories.Find(Id);

            if (c == null)
            {
                return HttpNotFound();
            }

            return View(c);

        }

        [HttpPost, ActionName("UpdateCategory")]
        public ActionResult Edit([Bind(Include = "Id, Name")] Category c)
        {
            if (c == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            AchievementBadge a = db.AchievementBadges.Find(c);

            if (c == null)
            {
                return HttpNotFound();
            }
            return View(c);

        }

        public ActionResult Delete2(Int32 Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Category c = db.Categories.Find(Id);
            if (c == null)
            {
                return HttpNotFound();
            }
            return View(c);
        }

        // POST: Home/Delete
        [HttpPost, ActionName("DeleteCategory")]

        public ActionResult DeleteCategory(Int32 Id)
        {
            Category c = db.Categories.Find(Id);
            db.Categories.Remove(c);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}