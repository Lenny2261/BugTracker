using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class ProfileViewModel
    {
    }

    public class ProjectDetailsViewModel
    {
        public ApplicationUser user { get; set; }
        public ICollection<Projects> projects { get; set; }
    }

    public class TicketDetailsViewModel
    {
        public ApplicationUser user { get; set; }
        public Tickets tickets { get; set; }
        public ICollection<TicketComments> ticketComments { get; set; }
        public ICollection<TicketAttachments> ticketAttachments { get; set; }
        public ICollection<TicketHistories> ticketHistories { get; set; }
    }

    public class DashboardViewModel
    {
        public ApplicationUser user { get; set; }
        public ICollection<Projects> projects { get; set; }
        public ICollection<Tickets> tickets { get; set; }
    }

    public class NotificationViewModel
    {
        public ICollection<TicketNotifications> notifications { get; set; }
        public ApplicationUser currentUser { get; set; }
    }
    //public class ManageUserViewModel
    //{
    //    public ICollection<ApplicationUser> users { get; set; }
    //    public ICollection<Projects> projects { get; set; }
    //}
}