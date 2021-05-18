using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using EdulearnWebsite.Models;

namespace EdulearnWebsite.Controllers
{
    public class LearnerController : Controller
    {
        private edulearnEntities db = new edulearnEntities();
        edulearnEntities1 database = new edulearnEntities1();

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
        public ActionResult Create([Bind(Include = "LearnerID,username,email,password")] learner learner, [Bind(Include = "LearnerID,username,email,password")] user users)
        {
            bool Status = false;
            string message = "";


            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(users.email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(learner);
                }

                users.ActivationCode = Guid.NewGuid();
                users.IsEmailVerified = false;

                db.learners.Add(learner);
                database.users.Add(users);
                db.SaveChanges();
                database.SaveChanges();

                //Send Email to User
                SendVerificationLinkEmail(users.email, users.ActivationCode.ToString());
                message = "Registration successfully done. Account activation link " +
                    " has been sent to your email id:" + users.email;
                Status = true;

                return RedirectToAction("Index");
            }
            else
            {
                message = "Invalid Request";
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
        public ActionResult Edit([Bind(Include = "LearnerID,username,email,password")] learner learner, [Bind(Include = "LearnerID,username,email,password")] user users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(learner).State = EntityState.Modified;
                database.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                database.SaveChanges();
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
            user userr = db.users.SingleOrDefault(user => learner.username == user.username);
            db.learners.Remove(learner);
            db.users.Remove(userr);
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

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (edulearnEntities db = new edulearnEntities())
            {
                var v = db.users.Where(a => a.email == emailID).FirstOrDefault();
                return v != null;
            }
        }


        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Login/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("owlhpm@gmail.com", "Edulearn Email Verification");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "Edulearn2021"; // Replace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";

                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                   " successfully created. Please click on the below link to verify your account" +
                   " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";

                body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                   " <br/><br/><a href='" + link + "'>Reset Password link </a> ";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}
