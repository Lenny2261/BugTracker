using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Tickets
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTimeOffset created { get; set; }
        public DateTimeOffset? updated { get; set; }
        public int projectID { get; set; }
        public int ticketTypeId { get; set; }
        public int ticketStatusId { get; set; }
        public int ticketPriorityId { get; set; }
        public string assignedId { get; set; }
        public string ownerId { get; set; }

        public virtual Projects project { get; set; }
        public virtual TicketTypes ticketType { get; set; }
        public virtual TicketStatuses ticketStatus { get; set; }
        public virtual TicketPriorities ticketPriority { get; set; }
        public virtual ApplicationUser assigned { get; set; }
        public virtual ApplicationUser owner { get; set; }
    }
}