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
    public class LearnerController : Controller
    {
        private edulearnEntities db = new edulearnEntities();

        // GET: Learner
        public ActionResult Index()
        {
            return View(db.learners.ToList());
        }

        // GET: Learner/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learner learner = db.learners.Find(id);
            if (learner == null)
            {
                return HttpNotFound();
            }
            return View(learner);
        }

        // GET: Learner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Learner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LearnerID,username,email,password")] learner learner)
        {
            if (ModelState.IsValid)
            {
                db.learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(learner);
        }

        // GET: Learner/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learner learner = db.learners.Find(id);
            if (learner == null)
            {
                return HttpNotFound();
            }
            return View(learner);
        }

        // POST: Learner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LearnerID,username,email,password")] learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(learner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(learner);
        }

        // GET: Learner/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            learner learner = db.learners.Find(id);
            if (learner == null)
            {
                return HttpNotFound();
            }
            return View(learner);
        }

        // POST: Learner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            learner learner = db.learners.Find(id);
            db.learners.Remove(learner);
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
