using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistories
    {
        public int id { get; set; }
        public int ticketId { get; set; }
        public string property { get; set; }
        public string oldValue { get; set; }
        public string newValue { get; set; }
        public DateTimeOffset? changed { get; set; }
        public string userId { get; set; }

        public virtual Tickets ticket { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}