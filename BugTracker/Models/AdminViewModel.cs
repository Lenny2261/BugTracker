using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class AdminViewModel
    {

    }

    public class AssignProjects
    {
        public ApplicationUser user { get; set; }
        public MultiSelectList projects { get; set; }
        public int[] selectedProjects { get; set; }
    }

    public class EditName
    {
        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }
    }
}