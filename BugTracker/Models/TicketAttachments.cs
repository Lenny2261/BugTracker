using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketAttachments
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string filePath { get; set; }
        public string description { get; set; }
        public DateTimeOffset created { get; set; }
        public string UserId { get; set; }
        public string fileURL { get; set; }
        public string icon { get; set; }

        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}