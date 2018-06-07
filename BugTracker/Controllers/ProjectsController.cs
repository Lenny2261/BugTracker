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
using BugTracker.Helpers;

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
            RoleHelper roleHelp = new RoleHelper();

            var model = roleHelp.UsersNotInRole("Submitter").ToList();

            if (User.IsInRole("Admin"))
            {
                model = roleHelp.UsersInRole("ProjectManager").ToList();
            }
            else if (User.IsInRole("ProjectManager"))
            {
                model = roleHelp.UsersInRole("Developer").ToList();
            }

            

            return View(model);
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult ShowAll()
        {
            return View(db.projects.ToList());
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult AssignUsers(string userId)
        {
            var userIn = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            var projectsId = new List<int>();

            var model = new AssignProjects
            {
                user = userIn,
                projects = new MultiSelectList(db.projects, "Id", "name", projectsId)
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult AssignUsers(AssignProjects model, string userId)
        {
            ProjectHelper projectHelp = new ProjectHelper();
            model.user = db.Users.Where(u => u.Id == userId).FirstOrDefault();

            foreach(var project in projectHelp.ListUserProjects(userId))
            {
                Projects projectDel = db.projects.Find(project.Id);
                projectDel.projectUsers.Remove(model.user);
                db.Entry(projectDel).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (model.selectedProjects == null)
                return RedirectToAction("ManageUsers");

            for(int index = 0; index < model.selectedProjects.Length; index++)
            {
                var projId = model.selectedProjects[index];
                model.user.Projects.Add(db.projects.Find(projId));
            }

            db.SaveChanges();
            return RedirectToAction("ManageUsers");
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
