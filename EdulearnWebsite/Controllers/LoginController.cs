using EdulearnWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult Index(userModel objchk, string ReturnUrl = "")
        {
            string message = "";
            if (ModelState.IsValid)
            {
                using (edulearnEntities db = new edulearnEntities())
                {
                    var obj = db.users.Where(a => a.username.Equals(objchk.username) && a.password.Equals(objchk.password)).FirstOrDefault();
                        if (obj.IsEmailVerified != true)
                        {
                        ModelState.AddModelError("", "Please verify your email first");
                        ViewBag.Message = "Please verify your email first";
                            return View(objchk);
                        }
                    
                       

                    //If the user is head admin
                    var headadminObj = db.headAdmins.Where(a => a.username.Equals(objchk.username) && a.password.Equals(objchk.password)).FirstOrDefault();

                    //If the user is head admin
                    var adminObj = db.admins.Where(a => a.username.Equals(objchk.username) && a.password.Equals(objchk.password)).FirstOrDefault();

                    if (obj != null)
                    {
                        Session["UserID"] = obj.UserID.ToString();
                        Session["UserName"] = obj.username.ToString();

                        int timeout = objchk.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(objchk.email, objchk.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
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

                    }
                    else
                    {

                        ModelState.AddModelError("", "The Username or Password is Incorrect");

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
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(user.email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }

                user.ActivationCode = Guid.NewGuid();
                user.IsEmailVerified = false;

                db.users.Add(user);
                database.learners.Add(learner);
                db.SaveChanges();
                database.SaveChanges();
                

                //Send Email to User
                SendVerificationLinkEmail(user.email, user.ActivationCode.ToString());
                message = "Registration successfully done. Account activation link " +
                    " has been sent to your email id:" + user.email;
                Status = true;

                return RedirectToAction("Index", "Login");
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.AdminID = new SelectList(db.admins, "AdminID", "firstname", user.AdminID);
            ViewBag.HeadAdminID = new SelectList(db.headAdmins, "HeadAdminID", "firstname", user.HeadAdminID);
            ViewBag.LearnerID = new SelectList(db.learners, "LearnerID", "username", learner.LearnerID);
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }


        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (edulearnEntities db = new edulearnEntities())
            {
                db.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = db.users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();


                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
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
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/Login/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("sheMeows283@gmail.com", "Edulearn Email Verification");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "09068175032"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

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