using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Cybersecurity_System.Models;

namespace Online_Cybersecurity_System.Controllers
{
    public class ManageQuizController : Controller
    {
        CybersecurityEntities db = new CybersecurityEntities();
        // GET: ManageQuiz
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InsertCategory()
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

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

        [HttpGet]
        public ActionResult InsertQuestion(Category model)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<Category> li = db.Categories.ToList();
            ViewBag.list = new SelectList(li, "CategoryId", "CategoryName");

            List<Question> ques1 = db.Questions.OrderByDescending(x => x.QuestionId).ToList();
            ViewData["list2"] = ques1;

            return View();
        }

        [HttpPost]
        public ActionResult InsertQuestion(Question model)
        {
            List<Category> li = db.Categories.ToList();
            ViewBag.list = new SelectList(li, "CategoryId", "CategoryName");

            List<Question> ques1 = db.Questions.OrderByDescending(x => x.QuestionId).ToList();
            ViewData["list2"] = ques1;

            Question q = new Question();
            q.QuestionId = model.QuestionId;
            q.QuestionText = model.QuestionText;
            q.CategoryId = (int)model.CategoryId;

            db.Questions.Add(model);
            db.SaveChanges();

            return RedirectToAction("InsertQuestion");

            //if (ModelState.IsValid)
            //{
            //    using (var databaseContext = new CybersecurityEntities())
            //    {
            //        List<Category> li = db.Categories.ToList();
            //        ViewBag.list = new SelectList(li, "CategoryId", "CategoryName");

            //        Question q = new Question();

            //        q.QuestionId = model.QuestionId;
            //        q.QuestionText = model.QuestionText;
            //        q.CategoryId = (int)model.CategoryId;

            //        databaseContext.Questions.Add(q);
            //        databaseContext.SaveChanges();
            //    }
            //    ViewBag.Message = "Question details inserted!";
            //    return View("Index");
            //}
            //else
            //{
            //    return View("Index", model);
            //}
        }

        [HttpGet]
        public ActionResult InsertChoice(Question model)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<Question> li = db.Questions.ToList();
            ViewBag.question = new SelectList(li, "QuestionId", "QuestionText");

            List<Choice> choice = db.Choices.OrderByDescending(x => x.ChoiceId).ToList();
            ViewData["list3"] = choice;
            return View();
        }

        [HttpPost]
        public ActionResult InsertChoice(Choice model)
        {
            List<Question> ques1 = db.Questions.OrderByDescending(x => x.QuestionId).ToList();
            ViewData["list2"] = ques1;

            List<Question> li = db.Questions.ToList();
            ViewBag.question = new SelectList(li, "QuestionId", "QuestionText");

            Choice c = new Choice();
            c.ChoiceId = model.ChoiceId;
            c.ChoiceText = model.ChoiceText;
            c.QuestionId = (int)model.QuestionId;

            db.Choices.Add(model);
            db.SaveChanges();

            return RedirectToAction("InsertChoice");
        }

        [HttpGet]
        public ActionResult InsertAnswer(Question model)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<Question> li = db.Questions.ToList();
            ViewBag.question = new SelectList(li, "QuestionId", "QuestionText");

            List<Answer> answer = db.Answers.OrderByDescending(x => x.AnswerId).ToList();
            ViewData["list4"] = answer;
            return View();
        }

        [HttpPost]
        public ActionResult InsertAnswer(Answer model)
        {
            int count = 0;
            List<Question> ques1 = db.Questions.OrderByDescending(x => x.QuestionId).ToList();
            ViewData["list2"] = ques1;

            List<Question> li = db.Questions.ToList();
            ViewBag.question = new SelectList(li, "QuestionId", "QuestionText");

            Answer a = new Answer();
            a.AnswerId = model.AnswerId;
            a.AnswerText = model.AnswerText;
            a.QuestionId = (int)model.QuestionId;
            

            db.Answers.Add(model);
            db.SaveChanges();

            return RedirectToAction("InsertAnswer");
        }

        public ActionResult CheckCategoryId(Int32 id)
        {
            bool result = db.Categories.Find(id) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("UpdateCategory/{id}")]
        public ActionResult UpdateCategory(Int32 id)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var list = db.Categories.ToList();
            ViewBag.CategoryId = new SelectList(list, dataValueField: "CategoryId", dataTextField: "CategoryName");

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
               
            }

            Category c = db.Categories.Find(id);

            if (c == null)
            {
                return HttpNotFound();
            }

            return View(c);
        }

        [HttpPost]
        public ActionResult UpdateCategory([Bind(Include = "CategoryId, CategoryName")] Category c)
        {
            if (ModelState.IsValid)
            {
                var list = db.Categories.ToList();
                ViewBag.CategoryId = new SelectList(list, dataValueField: "CategoryId", dataTextField: "CategoryName");

                //db.Entry(c).State = System.Data.Entity.EntityState.Unchanged;
                db.Set<Category>().AddOrUpdate(c);
                db.SaveChanges();
                return RedirectToAction("InsertCategory");
            }
            return View(c);
        }

        public ActionResult CheckQuestionId(Int32 id)
        {
            bool result = db.Questions.Find(id) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("UpdateQuestion/{id}")]
        public ActionResult UpdateQuestion(Int32 id)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var list = db.Categories.ToList();
            ViewBag.CategoryId = new SelectList(list, dataValueField: "CategoryId", dataTextField: "CategoryName");

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Question c = db.Questions.Find(id);

            if (c == null)
            {
                return HttpNotFound();
            }

            return View(c);
        }

        [HttpPost]
        public ActionResult UpdateQuestion([Bind(Include = "QuestionId, QuestionText, CategoryId")] Question c)
        {
            if (ModelState.IsValid)
            {
                var list = db.Categories.ToList();
                ViewBag.CategoryId = new SelectList(list, dataValueField: "CategoryId", dataTextField: "CategoryName");

                //db.Entry(c).State = System.Data.Entity.EntityState.Unchanged;
                db.Set<Question>().AddOrUpdate(c);
                db.SaveChanges();
                return RedirectToAction("InsertQuestion");
            }
            return View(c);
        }

        public ActionResult CheckChoiceId(Int32 id)
        {
            bool result = db.Choices.Find(id) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("UpdateChoice/{id}")]
        public ActionResult UpdateChoice(Int32 id)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }

            Choice c = db.Choices.Find(id);

            if (c == null)
            {
                return HttpNotFound();
            }

            return View(c);
        }

        [HttpPost]
        public ActionResult UpdateChoice([Bind(Include = "ChoiceId, ChoiceText, QuestionId")] Choice c)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(c).State = System.Data.Entity.EntityState.Unchanged;
                db.Set<Choice>().AddOrUpdate(c);
                db.SaveChanges();
                return RedirectToAction("InsertChoice");
            }
            return View(c);
        }

        public ActionResult CheckAnswerId(Int32 id)
        {
            bool result = db.Answers.Find(id) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("UpdateAnswer/{id}")]
        public ActionResult UpdateAnswer(Int32 id)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }

            Answer c = db.Answers.Find(id);

            if (c == null)
            {
                return HttpNotFound();
            }

            return View(c);
        }

        [HttpPost]
        public ActionResult UpdateAnswer([Bind(Include = "AnswerId, AnswerText, QuestionId")] Answer c)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(c).State = System.Data.Entity.EntityState.Unchanged;
                db.Set<Answer>().AddOrUpdate(c);
                db.SaveChanges();
                return RedirectToAction("InsertChoice");
            }
            return View(c);
        }

        public ActionResult DeleteCategory(Int32 id)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Category q = db.Categories.Find(id);
            if (q == null)
            {
                return HttpNotFound();
            }
            return View(q);
        }

        [HttpPost, ActionName("DeleteCategory")]
        public ActionResult DeleteCategoryConfirmed(Int32 id)
        {
            Category c = db.Categories.Find(id);
            db.Categories.Remove(c);
            db.SaveChanges();
            return RedirectToAction("InsertCategory");
        }

        public ActionResult DeleteQuestion(Int32 id)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Question q = db.Questions.Find(id);
            if (q == null)
            {
                return HttpNotFound();
            }
            return View(q);
        }

        [HttpPost, ActionName("DeleteQuestion")]
        public ActionResult DeleteQuestionConfirmed(Int32 id)
        {
            Question q = db.Questions.Find(id);
            db.Questions.Remove(q);
            db.SaveChanges();
            return RedirectToAction("InsertQuestion");
        }

        public ActionResult DeleteChoice(Int32 id)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Choice q = db.Choices.Find(id);
            if (q == null)
            {
                return HttpNotFound();
            }
            return View(q);
        }

        [HttpPost, ActionName("DeleteChoice")]
        public ActionResult DeleteChoiceConfirmed(Int32 id)
        {
            Choice q = db.Choices.Find(id);
            db.Choices.Remove(q);
            db.SaveChanges();
            return RedirectToAction("InsertChoice");
        }

        public ActionResult DeleteAnswer(Int32 id)
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Answer q = db.Answers.Find(id);
            if (q == null)
            {
                return HttpNotFound();
            }
            return View(q);
        }

        [HttpPost, ActionName("DeleteAnswer")]
        public ActionResult DeleteAnswerConfirmed(Int32 id)
        {
            Answer q = db.Answers.Find(id);
            db.Answers.Remove(q);
            db.SaveChanges();
            return RedirectToAction("InsertAnswer");
        }
    }
}