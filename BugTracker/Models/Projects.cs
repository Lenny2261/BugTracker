﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Projects
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public virtual ICollection<Tickets> tickets { get; set; }
        public virtual ICollection<ProjectUsers> projectUsers { get; set; }

        public Projects()
        {
            this.tickets = new HashSet<Tickets>();
            this.projectUsers = new HashSet<ProjectUsers>();
        }
       
    }
}