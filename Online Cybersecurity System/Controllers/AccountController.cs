using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// Additional using statements
using Online_Cybersecurity_System.Models;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Owin.Security;

namespace Online_Cybersecurity_System.Controllers
{
    public class AccountController : Controller
    {
        CybersecurityEntities db = new CybersecurityEntities();

        PasswordHasher ph = new PasswordHasher();

        private string HashPassword(string password)
        {
            return ph.HashPassword(password);
        }

        private bool VerifyPassword(string hash, string password)
        {
            return ph.VerifyHashedPassword(hash, password) == PasswordVerificationResult.Success;
        }

        private void SignIn (string Username, string Role, bool rememberMe)
        {
            // TODO(1): Identity and claims
            var iden = new ClaimsIdentity("AUTH");
            iden.AddClaim(new Claim(ClaimTypes.Name, Username));
            iden.AddClaim(new Claim(ClaimTypes.Role, Role));

            // TODO(2): Remember me
            var prop = new AuthenticationProperties
            {
                IsPersistent = rememberMe
            };

            // TODO(3): Sign in
            Request.GetOwinContext()
                   .Authentication
                   .SignIn(prop, iden);
        }
        private void SignOut()
        {
            // TODO: Sign out
            Request.GetOwinContext()
                   .Authentication
                   .SignOut();
        }
        private string ValidatePhoto(HttpPostedFileBase f)
        {
            var reType = new Regex(@"^image\/(jpeg|png)$", RegexOptions.IgnoreCase);
            var reName = new Regex(@"^.+\.(jpg|jpeg|png)$", RegexOptions.IgnoreCase);

            if (f == null)
            {
                return "No photo.";
            }
            else if (!reType.IsMatch(f.ContentType) || !reName.IsMatch(f.FileName))
            {
                return "Only JPG or PNG photo is allowed.";
            }
            else if (f.ContentLength > 1 * 1024 * 1024)
            {
                return "Photo size cannot more than 1MB.";
            }

            return null;
        }

        private string SavePhoto(HttpPostedFileBase f)
        {
            string name = Guid.NewGuid().ToString("n") + ".jpg";
            string path = Server.MapPath($"~/Photo/{name}");

            var img = new WebImage(f.InputStream);

            if (img.Width > img.Height)
            {
                int px = (img.Width - img.Height) / 2;
                img.Crop(0, px, 0, px);
            }
            else
            {
                int px = (img.Height - img.Width) / 2;
                img.Crop(px, 0, px, 0);
            }

            img.Resize(201, 201)
               .Crop(1, 1)
               .Save(path, "jpeg");

            return name;
        }
        private void DeletePhoto(string name)
        {
            name = System.IO.Path.GetFileName(name);
            string path = Server.MapPath($"~/Photo/{name}");
            System.IO.File.Delete(path);
        }

        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(LoginVM model, string returnURL = "")
        {
            if (ModelState.IsValid)
            {
                // TODO: Get user record based on username
                var employee = db.Employees.Find(model.Username);

                // TODO: AND if password matches
                if (employee != null && VerifyPassword(employee.Hash, model.Password))
                //if (user != null)
                {
                    // TODO: Sign in user + session
                    SignIn(employee.Username, employee.Hash, model.RememberMe);
                    Session["Image"] = employee.Image;
                    Session["Login"] = true;

                    // TODO: Handle return URL
                    if (returnURL == "")
                    {
                        return RedirectToAction("SelectQuizz", "Quizz");
                    }
                }
                else if (ModelState.IsValid)
                {
                    // TODO: Get user record based on username
                    var admin = db.Admins.Find(model.Username);

                    // TODO: AND if password matches
                    if (admin != null && VerifyPassword(admin.Hash, model.Password))
                    //if (user != null)
                    {
                        // TODO: Sign in user + session
                        SignIn(admin.Username, admin.Hash, model.RememberMe);
                        Session["Image"] = admin.Image;
                        Session["Login"] = true;

                        // TODO: Handle return URL
                        if (returnURL == "")
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("Password", "Email and Password not matched.");
                }
            }

            return View(model);
        }

        // GET: Account/AdminLogout
        public ActionResult Logout()
        {
            // TODO: Sign out user + session
            SignOut();
            Session.Remove("Image");
            Session.Remove("Login");
            return RedirectToAction("AdminRegister", "Account");
        }

        // GET: Account/CheckUsername
        public ActionResult CheckUsername(string Username)
        {
            // TODO: Check if username not duplicated.
            bool valid = db.Employees.Find(Username) == null;
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        //GET: Account/Register
        public ActionResult AdminRegister()
        {
            return View("Register");
        }

        //public ActionResult AdminRegister(int ID = 0)
        //{
        //    Admin a = new Admin();
        //    var lastAdmin = db.Admins.OrderByDescending(r => r.No).FirstOrDefault();
        //    if (ID != 0)
        //    {
        //        a = db.Admins.Where(x => x.No == ID).FirstOrDefault<Admin>();
        //    }
        //    else if (lastAdmin == null)
        //    {
        //        a.Id = "A0001";
        //    }
        //    else
        //    {
        //        a.Id = "A" + (Convert.ToInt32(lastAdmin.Id.Substring(4, lastAdmin.Id.Length - 4)) + 1).ToString("D3");
        //    }
        //    return View(a);
        //}


        // POST: Account/Register
        [HttpPost]
        public ActionResult AdminRegister(RegisterVM model)
        {
            // TODO: AND if username duplicated
            if (ModelState.IsValidField("Username") && db.Employees.Find(model.Username) != null)
            {
                ModelState.AddModelError("Username", "Duplicated username.");
            }

            string err = ValidatePhoto(model.Image);
            if (err != null)
            {
                ModelState.AddModelError("Image", err);
            }

            if (ModelState.IsValid)
            {
                var r = new Employee
                {
                    Id = model.Id,
                    Username = model.Username,
                    Email = model.Email,
                    Hash = HashPassword(model.Password), // TODO: Generate password hash
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Image = SavePhoto(model.Image),
                    RegisterDate = model.RegisterDate,
                };

               
                db.Employees.Add(r);
                db.SaveChanges();

                TempData["Info"] = "Account registered. Please login.";
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }


        // TODO: Authorize(Member)
        //POST: Account/Detail
       //[Authorize(Roles = "Employee")]
       //[HttpPost]
       // public ActionResult Detail(DetailVM model)
       // {
       // //TODO: Get member record of the current member
       //var m = db.Employees.Find(User.Identity.Name);

       //     if (m == null)
       //     {
       //         return RedirectToAction("Index", "Home");
       //     }

       //     if (model.Photo != null)
       //     {
       //         string err = ValidatePhoto(model.Photo);
       //         if (err != null)
       //         {
       //             ModelState.AddModelError("Photo", err);
       //         }
       //     }

       //     if (ModelState.IsValid)
       //     {
       //         if (model.Photo != null)
       //         {
       //             DeletePhoto(m.Image);
       //         //TODO: Update session
       //             Session["PhotoURL"] = m.Image = SavePhoto(model.Photo);
       //         }

       //         m.Username = model.Username;
       //         m.Email = model.Email;
       //         db.SaveChanges();

       //         TempData["Info"] = "Detail updated.";
       //         return RedirectToAction(null);
       //     }

       //     model.Image = m.Image;
       //     return View(model);
       // }

       // // TODO: Authorize
       // // GET: Account/Password
       // [Authorize]
       // public ActionResult Password()
       // {
       //     return View();
       // }

       // //TODO: Authorize
       // //POST: Account/Password
       // [Authorize]
       // [HttpPost]
       // public ActionResult Password(PasswordVM model)
       // {
       //     // TODO: Get user record of the current user
       //     var user = db.Users.Find(User.Identity.Name);

       //     // TODO: OR if password not matches
       //     if (user == null || VerifyPassword(user.Hash, model.Current) == false)
       //     {
       //         ModelState.AddModelError("Current", "Current Password not matched.");
       //     }

       //     if (ModelState.IsValid)
       //     {
       //         // TOOD: Generate password hash
       //         string hash = HashPassword(model.New);

       //         // TODO: Update member or admin record
       //         if (user.Role == "Admin")
       //         {
       //             db.Admins.Find(user.Username).Hash = hash;
       //         }
       //         else if (user.Role == "Employee")
       //         {
       //             db.Employees.Find(user.Username).Hash = hash;
       //         }

       //         db.SaveChanges();

       //         TempData["Info"] = "Password updated.";
       //         return RedirectToAction(null);
       //     }

       //     return View(model);
       // }

        // GET: Account/Reset
        public ActionResult Reset()
        {
            return View();
        }

        //public ActionResult AdminLogin()
        //{
        //    return View();
        //}

        //public ActionResult AdminLogin(LoginVM model, string returnURL = "")
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // TODO: Get user record based on username
        //        var admin = db.Admins.Find(model.Username);

        //        // TODO: AND if password matches
        //        if (admin != null && VerifyPassword(admin.Hash, model.Password))
        //        //if (user != null)
        //        {
        //            // TODO: Sign in user + session
        //            SignIn(admin.Username, admin.Hash, model.RememberMe);
        //            Session["Image"] = admin.Image;
        //            Session["Login"] = true;

        //            // TODO: Handle return URL
        //            if (returnURL == "")
        //            {
        //                return RedirectToAction("Index", "Dashboard");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Password", "Email and Password not matched.");
        //        }
        //    }

        //    return View(model);
        //}
    }
}