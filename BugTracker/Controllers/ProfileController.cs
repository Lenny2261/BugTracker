using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin,Developer,ProjectManager,Submitter")]
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profile
        public ActionResult Index()
        {

            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("AdminDashboard");
            }

            if (User.IsInRole("Submitter"))
            {

                var modelIf = new DashboardViewModel
                {
                    user = db.Users.Find(userId),
                    projects = db.projects.Where(p => p.projectUsers.Select(u => u.Id).Contains(userId)).ToList(),
                    tickets = db.tickets.Where(t => t.OwnerId == userId).ToList()
                };

                return View(modelIf);
            }
            else if (User.IsInRole("Developer"))
            {

                var modelIf = new DashboardViewModel
                {
                    user = db.Users.Find(userId),
                    projects = db.projects.Where(p => p.projectUsers.Select(u => u.Id).Contains(userId)).ToList(),
                    tickets = db.tickets.Where(t => t.AssignedId == userId).ToList()
                };

                return View(modelIf);
            }

            var model = new DashboardViewModel
            {
                user = db.Users.Find(userId),
                projects = db.projects.Where(p => p.projectUsers.Select(u => u.Id).Contains(userId)).ToList(),
                tickets = db.tickets.Where(t => t.Project.projectUsers.Select(p => p.Id).Contains(userId)).ToList()
            };
            return View(model);
        }

        
        public ActionResult AdminDashboard()
        {


            var userId = User.Identity.GetUserId();

            var model = new DashboardViewModel
            {
                user = db.Users.Find(userId),
                projects = db.projects.ToList(),
                tickets = db.tickets.ToList()
            };

            return View(model);
        }
    }
}