using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Cybersecurity_System.Models;

namespace QuizGamification.Controllers
{
    public class QuizzController : Controller
    {
        public CybersecurityEntities dbContext = new CybersecurityEntities();
        // GET: Quizz
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SelectQuizz()
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            CategoryVM category = new CategoryVM();
            category.ListOfQuizz = dbContext.Categories.Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString()

            }).ToList();

            return View(category);
        }

        [HttpPost]
        public ActionResult SelectQuizz(CategoryVM category)
        {
            CategoryVM quizSelected = dbContext.Categories.Where(c => c.CategoryId == category.CategoryId).Select(c => new CategoryVM
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,

            }).FirstOrDefault();

            if (quizSelected != null)
            {
                Session["SelectedQuiz"] = quizSelected;

                return RedirectToAction("QuizTest");
            }

            return View();
        }

        [HttpGet]
        public ActionResult QuizTest()
        {
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            CategoryVM quizSelected = Session["SelectedQuiz"] as CategoryVM;
            IQueryable<QuestionVM> questions = null;

            if (quizSelected != null)
            {
                questions = dbContext.Questions.Where(q => q.Category.CategoryId == quizSelected.CategoryId)
                   .Select(q => new QuestionVM
                   {
                       QuestionId = q.QuestionId,
                       QuestionText = q.QuestionText,
                       Choices = q.Choices.Select(c => new ChoiceVM
                       {
                           ChoiceId = c.ChoiceId,
                           ChoiceText = c.ChoiceText
                       }).ToList()

                   }).AsQueryable();
            }
            return View(questions);
        }

        [HttpPost]
        public ActionResult QuizTest(List<QuizAnswersVM> resultQuiz)
        {
           List<QuizAnswersVM> finalResultQuiz = new List<QuizAnswersVM>();

            foreach (QuizAnswersVM answser in resultQuiz)
            {
                QuizAnswersVM result = dbContext.Answers.Where(a => a.QuestionId == answser.QuestionId).Select(a => new QuizAnswersVM
                {
                    QuestionId = a.QuestionId,
                    AnswerQ = a.AnswerText,
                    isCorrect = (answser.AnswerQ.ToLower().Equals(a.AnswerText.ToLower())),

                }).FirstOrDefault();

                finalResultQuiz.Add(result);
            }

            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult QuizTest(int countCorrect)
        //{
        //    var result = countCorrect;
        //    var response = new { result = result };
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
    }
}