using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string avatar { get; set; }

        public virtual ICollection<Projects> Projects { get; set; }

        public ApplicationUser()
        {
            this.Projects = new HashSet<Projects>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Projects> projects { get; set; }
        public DbSet<TicketAttachments> ticketAttachments { get; set; }
        public DbSet<TicketComments> ticketComments { get; set; }
        public DbSet<TicketHistories> ticketHistories { get; set; }
        public DbSet<TicketNotifications> ticketNotifications { get; set; }
        public DbSet<TicketPriorities> ticketPriorities { get; set; }
        public DbSet<Tickets> tickets { get; set; }
        public DbSet<TicketStatuses> ticketStatuses { get; set; }
        public DbSet<TicketTypes> ticketTypes { get; set; }
    }
}