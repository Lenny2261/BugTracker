using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketTypes
    {
        public int id { get; set; }
        public string name { get; set; }

        public ICollection<Tickets> tickets;

        public TicketTypes()
        {
            this.tickets = new HashSet<Tickets>();
        }
    }
}