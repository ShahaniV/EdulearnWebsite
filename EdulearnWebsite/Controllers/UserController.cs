using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EdulearnWebsite.Models;

namespace EdulearnWebsite.Controllers
{
    public class UserController : Controller
    {
        private edulearnEntities db = new edulearnEntities();

        // GET: User
        public ActionResult Index()
        {
            var users = db.users.Include(u => u.admin).Include(u => u.headAdmin).Include(u => u.learner);
            return View(users.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
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
        public ActionResult Create([Bind(Include = "UserID,username,password,HeadAdminID,LearnerID,AdminID,confirmPass,email")] user user)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminID = new SelectList(db.admins, "AdminID", "firstname", user.AdminID);
            ViewBag.HeadAdminID = new SelectList(db.headAdmins, "HeadAdminID", "firstname", user.HeadAdminID);
            ViewBag.LearnerID = new SelectList(db.learners, "LearnerID", "username", user.LearnerID);
            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.admins, "AdminID", "firstname", user.AdminID);
            ViewBag.HeadAdminID = new SelectList(db.headAdmins, "HeadAdminID", "firstname", user.HeadAdminID);
            ViewBag.LearnerID = new SelectList(db.learners, "LearnerID", "username", user.LearnerID);
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,username,password,HeadAdminID,LearnerID,AdminID,confirmPass,email")] user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminID = new SelectList(db.admins, "AdminID", "firstname", user.AdminID);
            ViewBag.HeadAdminID = new SelectList(db.headAdmins, "HeadAdminID", "firstname", user.HeadAdminID);
            ViewBag.LearnerID = new SelectList(db.learners, "LearnerID", "username", user.LearnerID);
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
