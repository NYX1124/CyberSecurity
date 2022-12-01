using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Online_Cybersecurity_System.Models;
using Online_Cybersecurity_System.Controllers;

namespace Online_Cybersecurity_System.Controllers
{
    public class Home2Controller : Controller
    {
        CybersecurityEntities db = new CybersecurityEntities();
        private static int currentQuestion = 0;
        private List<int> list;

        public Home2Controller()
        {
            this.list = new List<int>();
        }

        // GET: Home2
        public ActionResult Index()
        {
            //Employee emp = (Employee)Session["Employee"];
            return View();
        }

        [HttpGet]
        public ActionResult Quiz()
        {
            List<string> name = new List<string>();
            foreach (Category c in db.Categories.ToList())
                name.Add(c.CategoryName);

            List<string> difficulty = new List<string>();
            difficulty.Add("Easy"); difficulty.Add("Intermediate"); difficulty.Add("Difficult");

            List<int> questNumber = new List<int>();
            questNumber.Add(1);

            ViewBag.name = new SelectList(name);
            ViewBag.difficulty = new SelectList(difficulty);
            ViewBag.quizQuestNumber = new SelectList(questNumber);
            return View();
        }

        [HttpGet]
        public ActionResult Exam(string categoryName)
        {
            //Category filter1 = db.Categories.Where(c => c.Name == categoryName).FirstOrDefault();

            //Quiz filter2 = db.Quizs.Where(q => q.difficulty == difficulty).FirstOrDefault();
            //Quiz quiz1 = db.Quizs.Where(q => q.CategoryId == filter1.Id).FirstOrDefault();
            //var model = quiz1.Where(q => q.difficulty == filter2.difficulty);

            //return View(quiz1);


            //QuizVM quizSelected = Session["SelectedQuiz"] as QuizVM;
            //IQueryable<QuizVM> quiz = null;
            
            var quiz2 = db.Quizs.Where(q => q.Category.Equals(categoryName)).Select(q => new QuizVM
                //from q in db.Quizs
                //join c in db.Categories on q.CategoryId equals c.Id
                //     select new QuizVM
                       {
                           id = q.id,
                           description = q.description,
                           choiceA = q.choiceA,
                           choiceB = q.choiceB,
                           choiceC = q.choiceC,
                           choiceD = q.choiceD
                       });
            return View(quiz2);

        }

        //[HttpGet]
        //public ActionResult Quiz(Category category)
        //{
        //    CategoryVM quizSelected = db.Categories.Where(q => q.Id == category.Id).Select(q => new CategoryVM
        //    {
        //        Id = category.Id,
        //        Name = category.Name

        //    }).FirstOrDefault();

        //    if (quizSelected != null)
        //    {
        //        Session["Quiz"] = quizSelected;


        //        return RedirectToAction("Exam");
        //    }


        //    return View();
        //}

        //var topicId = category.Id;
        //var getQuizByCategory = db.Quizs.Where(x => x.CategoryId == topicId)
        //    .Join(db.Categories, u => u.id, s => s.Id, (u, s) => s)
        //    .ToList();

        //List<Category> li = db.Categories.ToList();
        //ViewBag.list = new SelectList(li, "Id", "Name");
        //return View();





        [HttpPost]
        public ActionResult Exam(List<QuizVM> resultQuiz)
        {
            List<QuizVM> finalResultQuiz = new List<QuizVM>();

            foreach (QuizVM answser in resultQuiz)
            {
                QuizVM result = db.Quizs.Where(a => a.correct_answer == answser.correct_answer).Select(a => new QuizVM
                {
                    id = a.id,
                    correct_answer = a.correct_answer,
                    //isCorrect = (answser.correct_answer.ToLower().Equals(a.correct_answer.ToLower()))

                }).FirstOrDefault();

                finalResultQuiz.Add(result);
            }

            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public ActionResult Quiz(QuizVM model)
        //{
        //    List<Category> li = db.Categories.ToList();
        //    ViewBag.list = new SelectList(li, "Id", "Name");

        //    return RedirectToAction("Quiz", "Home2");
        //}

        //private List<Quiz> generate(string Name, string difficulty, int numOfQuestion)
        //{
        //    Random rand = new Random();
        //    List<int> result = new List<int>();
        //    HashSet<int> check = new HashSet<int>();
        //    List<Quiz> Quizs = GetQuesCount(Name, difficulty);

        //    for (int i = 0; i < numOfQuestion; i++)
        //    {
        //        int curValue = rand.Next(1, Quizs.Count());
        //        while (check.Contains(curValue))
        //        {
        //            curValue = rand.Next(1, Quizs.Count());
        //        }
        //        result.Add(curValue);
        //        check.Add(curValue);
        //    }
        //    List<Quiz> generatedList = new List<Quiz>();
        //    for (int i = 0; i < result.Count(); i++)
        //    {
        //        generatedList.Add(Quizs[result[i]]);
        //    }
        //    return generatedList;

        //}

        //private List<Quiz> GetQuesCount(string Name, string difficulty)
        //{
        //    List<Quiz> Quizs = new List<Quiz>();
        //    foreach (Quiz q in db.Quizs.ToList())
        //    {
        //        if (q.Category.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase) &&
        //            q.difficulty.Equals(difficulty, StringComparison.CurrentCultureIgnoreCase) 
        //            && q.quizStatus.Equals("Active", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            Quizs.Add(q);
        //        }
        //    }
        //    return Quizs;
        //}

        //[HttpGet]
        //public ActionResult StartQuiz(string name, string difficulty, string quizQuestNumber)
        //{
        //    currentQuestion = 0;

        //    List<Quiz> Quizs = generate(name, difficulty, Int32.Parse(quizQuestNumber));
        //    Session["list"] = Quizs;
        //    Quiz quest = Quizs[currentQuestion];
        //    return Json(quest, JsonRequestBehavior.AllowGet);


        //}

        //public ActionResult ShowExamDetails(int? id)
        //{
        //    Exam exam;
        //    if (id == null)
        //        exam = (Exam)Session["exam"];
        //    else{
        //        exam = db.Exams.Find(id);
        //    }

        //    return View(exam);
        //}



        //[HttpPost]
        //public ActionResult QuizTest(List<QuizVM> resultQuiz)
        //{
        //    List<QuizVM> finalResultQuiz = new List<QuizVM>();

        //    foreach (QuizVM answser in resultQuiz)
        //    {
        //        QuizVM result = db.Quizs.Where(a => a.id == answser.id).Select(a => new QuizVM
        //        {
        //            QuestionID = a.QuestionID.Value,
        //            AnswerQ = a.AnswerText,
        //            isCorrect = (answser.AnswerQ.ToLower().Equals(a.AnswerText.ToLower()))

        //        }).FirstOrDefault();

        //        finalResultQuiz.Add(result);
        //    }

        //    return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult NextQuestion(string name, string difficulty, string quizQuestNumber)
        //{
        //    List<Quiz> Quizs = generate(name, difficulty, Int32.Parse(quizQuestNumber));
        //    if (currentQuestion < list.Count() - 1)
        //        currentQuestion++;
        //    Quiz quest = Quizs[currentQuestion];

        //    return Json(quest, JsonRequestBehavior.AllowGet);
        //}

    public ActionResult AchievementBadge()
    {
        return View();
    }

    public ActionResult Leaderboard()
    {
        return View();
    }

    public ActionResult ManageAccount()
    {
        return View();
    }

        [HttpGet]
        public ActionResult SelectQuiz()
        {
            CategoryVM category = new CategoryVM();
            category.ListOfQuizz = db.Categories.Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString()

            }).ToList();

            return View(category);
        }

        public ActionResult SelectQuiz(CategoryVM category)
        {
            CategoryVM quizSelected = db.Categories.Where(c => c.CategoryId == category.CategoryId).Select(c => new CategoryVM
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
        public ActionResult QuizExam()
        {
            CategoryVM quizSelected = Session["SelectedQuiz"] as CategoryVM;
            IQueryable<QuestionVM> questions = null;

            if (quizSelected != null)
            {
                questions = db.Questions.Where(q => q.Category.CategoryId == quizSelected.CategoryId)
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
        public ActionResult QuizExam(List<QuizAnswersVM> resultQuiz)
        {
            List<QuizAnswersVM> finalResultQuiz = new List<QuizAnswersVM>();

            foreach (QuizAnswersVM answser in resultQuiz)
            {
                QuizAnswersVM result = db.Answers.Where(a => a.QuestionId == answser.QuestionId).Select(a => new QuizAnswersVM
                {
                    QuestionId = a.QuestionId,
                    AnswerQ = a.AnswerText,
                    isCorrect = (answser.AnswerQ.ToLower().Equals(a.AnswerText.ToLower()))

                }).FirstOrDefault();

                finalResultQuiz.Add(result);
            }

            return Json(new { result = finalResultQuiz }, JsonRequestBehavior.AllowGet);
        }
    }

}