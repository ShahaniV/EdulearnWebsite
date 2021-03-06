using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EdulearnWebsite.Models;

namespace EdulearnWebsite.Controllers
{
    public class ModulesController : Controller
    {
        private edulearnEntities db = new edulearnEntities();

        // GET: Modules
        public ActionResult Index()
        {
            return View(db.Modules.ToList());
        }

        public ActionResult Kindergarten()
        {
            return View(db.Modules.ToList());
        }
        public ActionResult Elementary()
        {
            return View(db.Modules.ToList());
        }
        public ActionResult Junior_high()
        {
            return View(db.Modules.ToList());
        }
        public ActionResult Senior_high()
        {
            return View(db.Modules.ToList());
        }

        // GET: Modules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Module module)
        {
            var ModuleNum = db.Modules.Where(a => a.gradeLevel == module.gradeLevel && a.subject == module.subject && a.quarterNo == module.quarterNo).OrderByDescending(p => p.moduleNo).First();
            int ModuleNumber = (int)ModuleNum.moduleNo;
            module.moduleNo = ModuleNumber + 1;

            var supportedTypes = new[] { "pdf", "PDF" };

            string fileName = Path.GetFileNameWithoutExtension(module.fileFile.FileName);
            string extension = Path.GetExtension(module.fileFile.FileName);
            var fileExt = Path.GetExtension(module.fileFile.FileName).Substring(1);
            if (!supportedTypes.Contains(fileExt))
            {
                ModelState.AddModelError("", "Incorrect file format, .pdf extension file only.");
                return View(module);
            }
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            module.files = "/Files/" + fileName;
            fileName = Path.Combine(Server.MapPath("../Files/"), fileName);
            module.fileFile.SaveAs(fileName);


            if (ModelState.IsValid)
            {
                //var isExist = IsUsernameExist(module.adminName);
                //if (!isExist)
                //{
                //    ModelState.AddModelError("", "Admin Username doesn't exist");
                //    return View(module);
                //}
                //else
                //{
                    var ModExist = IsModulesExist(module.gradeLevel, module.subject, module.quarterNo, module.moduleNo);
                    if (!ModExist)
                    {
                        


                        //IsAdminNameExist(module.adminName, module);
                        db.Modules.Add(module);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    ModelState.AddModelError("", "Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                    return View(module);
                                }
                            }
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Module already exists" + module.moduleNo);
                        return View(module);
                    }
                   
                
                

            }

            return View(module);
        }

       

        // GET: Modules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Module module)
        {
            if (ModelState.IsValid)
            {
                
                if (module.fileFile.ContentLength > 0)
                {
                    var supportedTypes = new[] { "pdf", "PDF" };
                    string fileName = Path.GetFileNameWithoutExtension(module.fileFile.FileName);
                    string extension = Path.GetExtension(module.fileFile.FileName);
                    var fileExt = Path.GetExtension(module.fileFile.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        ModelState.AddModelError("", "Incorrect file format, .pdf extension file only.");
                        return View(module);
                    }
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    module.files = "/Files/" + fileName;
                    fileName = Path.Combine(Server.MapPath("/Files/"), fileName);
                    module.fileFile.SaveAs(fileName);
                }

                //var isExist = IsUsernameExist(module.adminName);
                //if (!isExist)
                //{
                //    ModelState.AddModelError("", "Admin Username doesn't exist");
                //    return View(module);
                //}
                //else
                //{
                    db.Entry(module).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                ModelState.AddModelError("", "Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                return View(module);
                            }
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                
            }
            return View(module);
        }

        // GET: Modules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(db.Modules.Where(x => x.Id == id).FirstOrDefault());
        }



        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Where(x => x.Id == id).FirstOrDefault();
            RemoveFileFromServer(module.files);
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       

        private bool RemoveFileFromServer(string path)
        {
            var fullPath = Request.MapPath(path);
            if (!System.IO.File.Exists(fullPath)) return false;

            try //Maybe error could happen like Access denied or Presses Already User used
            {
                System.IO.File.Delete(fullPath);
                return true;
            }
            catch (Exception e)
            {
                //Debug.WriteLine(e.Message);
            }
            return false;
        }

        [NonAction]
        public static void IsAdminNameExist(string Username, Module module)
        {
            using (edulearnEntities db = new edulearnEntities())
            {
                var v = db.admins.Where(a => a.username == Username).FirstOrDefault();
                if (v == null)
                {
                    var s = db.headAdmins.Where(a => a.username == Username).FirstOrDefault();
                    var AdminName = s.firstname + " " + s.lastname;
                    module.adminName = AdminName;
                }
                else {
                    var AdminName = v.firstname + " " + v.lastname;
                    module.adminName = AdminName;
                }
               
            }
        }

        [NonAction]
        public bool IsUsernameExist(string Username)
        {
            using (edulearnEntities db = new edulearnEntities())
            {
                var v = db.admins.Where(a => a.username == Username).FirstOrDefault();
                if (v == null)
                {
                    var s = db.headAdmins.Where(a => a.username == Username).FirstOrDefault();
                    return s != null;
                }
                else
                {
                    return v != null;
                }
                
            }
        }

        [NonAction]
        public bool IsModulesExist(string GradeLevel, string Subject, int? quarter, int? moduleno)
        {
            using (edulearnEntities db = new edulearnEntities())
            {
                var v = db.Modules.Where(a => a.gradeLevel == GradeLevel && a.subject== Subject && a.quarterNo ==quarter && a.moduleNo ==moduleno).FirstOrDefault();
                return v != null;
            }
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
