using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketAttachments
    {
        public int id { get; set; }
        public int ticketId { get; set; }
        public string filePath { get; set; }
        public string description { get; set; }
        public DateTimeOffset created { get; set; }
        public string userId { get; set; }
        public string fileURL { get; set; }

        public virtual Tickets ticket { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}