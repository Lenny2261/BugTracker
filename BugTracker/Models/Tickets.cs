using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Tickets
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTimeOffset created { get; set; }
        public DateTimeOffset? updated { get; set; }
        public int ProjectID { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketStatusId { get; set; }
        public int TicketPriorityId { get; set; }
        public string AssignedId { get; set; }
        public string OwnerId { get; set; }

        public virtual ICollection<TicketAttachments> ticketAttachments { get; set; }
        public virtual ICollection<TicketComments> ticketComments { get; set; }
        public virtual ICollection<TicketHistories> ticketHistories { get; set; }
        public virtual ICollection<TicketNotifications> ticketNotifications { get; set; }

        public Tickets()
        {
            this.ticketAttachments = new HashSet<TicketAttachments>();
            this.ticketComments = new HashSet<TicketComments>();
            this.ticketHistories = new HashSet<TicketHistories>();
            this.ticketNotifications = new HashSet<TicketNotifications>();
        }

        public virtual Projects Project { get; set; }
        public virtual TicketTypes TicketType { get; set; }
        public virtual TicketStatuses TicketStatus { get; set; }
        public virtual TicketPriorities TicketPriority { get; set; }
        public virtual ApplicationUser Assigned { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }
}