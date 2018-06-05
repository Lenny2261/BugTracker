using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketPriorities
    {
        public int id { get; set; }
        public string name { get; set; }

        public ICollection<Tickets> tickets;

        public TicketPriorities()
        {
            this.tickets = new HashSet<Tickets>();
        }
    }
}