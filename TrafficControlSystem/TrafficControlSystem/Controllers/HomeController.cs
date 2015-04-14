using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using TCS.Model;
using TrafficControlSystem.Models;
using System.Web.Security;

namespace TrafficControlSystem.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                using (DataModel db = new DataModel())
                {
                    string username = model.UserName;
                    string password = model.Password;

                    // Now if our password was enctypted or hashed we would have done the
                    // same operation on the user entered password here, But for now
                    // since the password is in plain text lets just authenticate directly
                    var user = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
                    bool userValid = user != null;

                    // User found in the database
                    if (userValid)
                    {

                        user.LastVisit = DateTime.Now;
                        db.SaveChanges();
                        db.Dispose();
                        FormsAuthentication.SetAuthCookie(username, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            switch ((UserRole)user.Role)
                            {
                                case UserRole.Manager:
                                    return Redirect("~/Manager");

                                case UserRole.IT:
                                    return Redirect("~/");

                                case UserRole.Admin:
                                    return Redirect("~/Users");
                            }
                            return Redirect("~/");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        

    }
}