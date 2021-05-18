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
    public class AdminController : Controller
    {
        private edulearnEntities db = new edulearnEntities();
        private edulearnEntities1 database = new edulearnEntities1();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.admins.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminID,firstname,lastname,username,email,password")] admin admin, [Bind(Include = "AdminID,username,email,password")] user useradmin)
        {
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(useradmin.email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(admin);
                }

                useradmin.ActivationCode = Guid.NewGuid();
                useradmin.IsEmailVerified = false;

                db.admins.Add(admin);
                database.users.Add(useradmin);
                db.SaveChanges();
                database.SaveChanges();
                

                //Send Email to User
                SendVerificationLinkEmail(useradmin.email, useradmin.ActivationCode.ToString());
                message = "Registration successfully done. Account activation link " +
                    " has been sent to your email id:" + useradmin.email;
                Status = true;

                return RedirectToAction("Index");
            }
            else
            {
                message = "Invalid Request";
            }

            return View(admin);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminID,firstname,lastname,username,email,password")] admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            admin admin = db.admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            admin admin = db.admins.Find(id);
            string Name = admin.username;
            user userr = db.users.SingleOrDefault(user => admin.username == user.username);
            db.admins.Remove(admin);
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
