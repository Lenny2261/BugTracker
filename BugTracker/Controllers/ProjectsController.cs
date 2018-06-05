using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        [Authorize(Roles = "Admin, ProjectManager, Developer, Submitter")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            //var users = db.projects.SelectMany(p => p.projectUsers).Where(p => p.Id == userId);

            var model = db.projects.Where(p => p.projectUsers.Select(u => u.Id).Contains(userId));
            return View(model);
        }

        // GET: Projects/Details/5
        [Authorize(Roles = "Admin, ProjectManager, Developer, Submitter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, ProjectManager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,description")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, ProjectManager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,description")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, ProjectManager")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.projects.Find(id);
            db.projects.Remove(projects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult ManageUsers()
        {

            var model = db.Users.ToList();

            return View(model);
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult AssignUsers()
        {

            var model = db.Users.Where(u => u.Id == User.Identity.GetUserId()).FirstOrDefault();

            ViewBag.Projects = db.projects.Where(p => p.projectUsers.FirstOrDefault().Id != model.Id).ToList();

            return View(model);
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
