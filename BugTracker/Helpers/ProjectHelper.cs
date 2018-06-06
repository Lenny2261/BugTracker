using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ICollection<Projects> ListUserProjects(string userId) { ApplicationUser user = db.Users.Find(userId); var projects = user.Projects.ToList(); return (projects); }
    }
}