using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketComments
    {
        public int id { get; set; }
        public int ticketId { get; set; }
        public string body { get; set; }
        public DateTimeOffset created { get; set; }
        public string userId { get; set; }

        public virtual Tickets ticket { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}