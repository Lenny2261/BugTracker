using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistories
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string property { get; set; }
        public string oldValue { get; set; }
        public string newValue { get; set; }
        public DateTimeOffset? changed { get; set; }
        public string UserId { get; set; }

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}