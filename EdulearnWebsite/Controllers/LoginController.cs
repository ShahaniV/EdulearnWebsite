using EdulearnWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EdulearnWebsite.Controllers
{
    public class LoginController : Controller
    {
        edulearnEntities db = new edulearnEntities();
        edulearnEntities1 database = new edulearnEntities1();
        // GET: Login
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(userModel objchk)
        {
            if (ModelState.IsValid)
            {
                using (edulearnEntities db = new edulearnEntities())
                {
                    var obj = db.users.Where(a => a.username.Equals(objchk.username) && a.password.Equals(objchk.password)).FirstOrDefault();

                    //If the user is head admin
                  var headadminObj = db.headAdmins.Where(a => a.username.Equals(objchk.username) && a.password.Equals(objchk.password)).FirstOrDefault();

                    //If the user is head admin
                    var adminObj = db.admins.Where(a => a.username.Equals(objchk.username) && a.password.Equals(objchk.password)).FirstOrDefault();

                    if (obj != null)
                    {
                        Session["UserID"] = obj.UserID.ToString();
                        Session["UserName"] = obj.username.ToString();

                        if (headadminObj != null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if (adminObj != null)
                        {
                            return RedirectToAction("AdminHome", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Home", "Home");
                        }
                    }
                    else
                    {

                        ModelState.AddModelError("", "The UserName or Password is Incorrect");

                    }
                }
            }



            return View(objchk);
        }


        public ActionResult SignUp()
        {
            ViewBag.AdminID = new SelectList(db.admins, "AdminID", "firstname");
            ViewBag.HeadAdminID = new SelectList(db.headAdmins, "HeadAdminID", "firstname");
            ViewBag.LearnerID = new SelectList(db.learners, "LearnerID", "username");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "UserID,username,password,HeadAdminID,LearnerID,AdminID,confirmPass,email")] user user, [Bind(Include = "LearnerID,username,email,password")] learner learner)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                database.learners.Add(learner);
                db.SaveChanges();
                database.SaveChanges();
                return RedirectToAction("Index","Login");
            }

            ViewBag.AdminID = new SelectList(db.admins, "AdminID", "firstname", user.AdminID);
            ViewBag.HeadAdminID = new SelectList(db.headAdmins, "HeadAdminID", "firstname", user.HeadAdminID);
            ViewBag.LearnerID = new SelectList(db.learners, "LearnerID", "username", user.LearnerID);
            return View(user);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}