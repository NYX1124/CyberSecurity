using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace Online_Cybersecurity_System.Models
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class RegisterVM
    {

        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        [Remote("CheckUsername", "Account", ErrorMessage = "Duplicated {0}.")]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^(.+)@(.+)$", ErrorMessage = "Invalid {0} format.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        [RegularExpression(@"(?=^.{8,}$)(?=.*\d)(?=.*[!@#$%^&*]+)(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$", ErrorMessage = "Invalid {0} format. Password must be contains at least one alphabet, one number and one special characters")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^(\+?6?01)[02-46-9]-*[0-9]{7}$|^(\+?6?01)[1]-*[0-9]{8}$", ErrorMessage = "Invalid {0} format. E.g. +6011-99999999 / +6012-9999999")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        [Required]
        [Display(Name = "Image")]
        public HttpPostedFileBase Image { get; set; }


        [Display(Name = "Registration Date")]
        public System.DateTime RegisterDate { get; set; }

    }

    public class PasswordVM
    {
        [Display(Name = "Current Password")]
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Current { get; set; }

        [Display(Name = "New Password")]
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string New { get; set; }

        [Display(Name = "Confirm Password")]
        [Required]
        [StringLength(20, MinimumLength = 5)]
        // TODO: Compare
        [Compare("New")]
        public string Confirm { get; set; }
    }

    public class QuizVM
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Quiz ID")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Question")]
        [StringLength(50)]
        public string description { get; set; }

        [Required]
        [Display(Name = "Difficulty")]
        public string difficulty { get; set; }

        [Required]
        [Display(Name = "Answer A")]
        public string choiceA { get; set; }

        [Required]
        [Display(Name = "Answer B")]
        public string choiceB { get; set; }

        [Required]
        [Display(Name = "Answer C")]
        public string choiceC { get; set; }

        [Required]
        [Display(Name = "Answer D")]
        public string choiceD { get; set; }

        [Required]
        [Display(Name = "Correct Answer")]
        public string correct_answer { get; set; }

        [Required]
        [Display(Name = "Quiz Status")]
        public string quizStatus { get; set; }


        [Required]
        [Display(Name = "Created Date")]
        public System.DateTime createDate { get; set; }

        [Display(Name = "Select Category")]
        [Required(ErrorMessage = "*")]
        public Nullable<int> CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }

    }

    public class AchievementBadgeVM
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Achievement Badge ID")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Achievement Badge Title")]
        public string achievementTitle { get; set; }

        [Required]
        [Display(Name = "Achievement Badge Description")]
        public string achievementDescription { get; set; }

        [Required]
        [Display(Name = "Achievement Badge Status")]
        public string achievementStatus { get; set; }

        [Display(Name = "Achievement Badge Created Date")]
        public System.DateTime createDate { get; set; }
    }

    public class CategoryVM
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Display(Name = "Topic")]
        [Required]
        public string CategoryName { get; set; }

        public List<SelectListItem> ListOfQuizz { get; set; }
    }

    public class ExamVM
    {
        public int Id { get; set; }
        public string Idnetifier { get; set; }
        public System.DateTime Date { get; set; }
        public int Score { get; set; }
        public int QuestionNo { get; set; }
        public int TrueQuesNo { get; set; }
        public int FalseQuesNo { get; set; }
        public int ExamTime { get; set; }
    }

    public class StoryVM
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string src { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string storyStatus { get; set; }
        public System.DateTime createDate { get; set; }
    }

    public class QuestionVM
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        [Display(Name = "Question")]
        public string QuestionText { get; set; }

        public string QuestionType { get; set; }
        [Required]
        public string Anwser { get; set; }
        public ICollection<ChoiceVM> Choices { get; set; }
        public Nullable<int> QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        public virtual Choice Choice { get; set; }

        public virtual Answer Answer { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }

    public class ChoiceVM
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ChoiceId { get; set; }

        [Required]
        [Display(Name = "Choice")]
        public string ChoiceText { get; set; }
    }

    public class QuizAnswersVM
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        public string QuestionText { get; set; }

        public string AnswerQ { get; set; }

        public bool isCorrect { get; set; }
    }

    public class ScoreVM
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int userId { get; set; }

        [Required]
        public int score1 { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public System.DateTime createDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual Employee Employee { get; set; }
    }
}