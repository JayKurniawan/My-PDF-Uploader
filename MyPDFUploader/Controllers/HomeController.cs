using MyPDFUploader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;

namespace MyPDFUploader.Controllers
{
    public class HomeController : Controller
    {
        private UploaderDatabaseEntities db = new UploaderDatabaseEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            User userModel = new User();
            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User userRegister) 
        {
            using(UploaderDatabaseEntities dbModel = new UploaderDatabaseEntities())
            {
                if (dbModel.Users.Any(x => x.Username == userRegister.Username))
                {
                    ViewBag.UserExistMessage = "Username already exist.";
                    return View("Register", userRegister);
                }

                dbModel.Users.Add(userRegister);
                dbModel.SaveChanges();
            }

            ModelState.Clear();
            ViewBag.RegisterSuccessMessage = "Your registration is successful.";

            return View("Register", new User());
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User userLogin)
        {
            if (ModelState.IsValid)
            {
                var details = (from userlist in db.Users
                               where userlist.Username == userLogin.Username && userlist.Password == userLogin.Password
                               select new
                               {
                                   userlist.UserId,
                                   userlist.Username
                               }).ToList();
                if(details.FirstOrDefault() != null)
                {
                    Session["UserId"] = details.FirstOrDefault().UserId;
                    Session["Username"] = details.FirstOrDefault().Username;
                    return RedirectToAction("Create", "Articles");
                }
                else
                {
                    ViewBag.InvalidLoginMessage = "Username or password is incorrect.";
                }
            }
           
            return View(userLogin);
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}