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
    public class HeadAdminController : Controller
    {
        private edulearnEntities db = new edulearnEntities();
        private edulearnEntities1 database = new edulearnEntities1();

        // GET: HeadAdmin
        public ActionResult Index()
        {
            return View(db.headAdmins.ToList());
        }

        // GET: HeadAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            headAdmin headAdmin = db.headAdmins.Find(id);
            if (headAdmin == null)
            {
                return HttpNotFound();
            }
            return View(headAdmin);
        }

        // GET: HeadAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HeadAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HeadAdminID,firstname,lastname,username,email,password")] headAdmin headAdmin, [Bind(Include = "HeadAdminID,username,email,password")] user user)
        {
            if (ModelState.IsValid)
            {
                db.headAdmins.Add(headAdmin);
                database.users.Add(user);
                db.SaveChanges();
                database.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(headAdmin);
        }

        // GET: HeadAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            headAdmin headAdmin = db.headAdmins.Find(id);
            if (headAdmin == null)
            {
                return HttpNotFound();
            }
            return View(headAdmin);
        }

        // POST: HeadAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HeadAdminID,firstname,lastname,username,email,password")] headAdmin headAdmin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(headAdmin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(headAdmin);
        }

        // GET: HeadAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            headAdmin headAdmin = db.headAdmins.Find(id);
            if (headAdmin == null)
            {
                return HttpNotFound();
            }
            return View(headAdmin);
        }

        // POST: HeadAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            headAdmin headAdmin = db.headAdmins.Find(id);
            db.headAdmins.Remove(headAdmin);
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
