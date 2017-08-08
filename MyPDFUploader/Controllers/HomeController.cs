﻿using MyPDFUploader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User userRegister) 
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(userRegister);
                db.SaveChanges();
               
            }
            return View();
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
                    return RedirectToAction("Welcome", "Home");
                }
            }else
            {
                ModelState.AddModelError("", "Invalid data.");
            }

            return View(userLogin);
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}