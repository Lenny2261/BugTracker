using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProjectUsers
    {
        public int id { get; set; }
        public int projectId { get; set; }
        public string userId { get; set; }

        public virtual Projects project { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}