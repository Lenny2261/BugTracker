namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(r => r.Email == "jmahoney2261@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jmahoney2261@Mailinator.com",
                    Email = "jmahoney2261@Mailinator.com",
                    firstName = "John",
                    lastName = "Mahoney"
                }, "penguins82");
            }

            var userId = userManager.FindByEmail("jmahoney2261@Mailinator.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(r => r.Email == "jTwichell@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jTwichell@Mailinator.com",
                    Email = "jTwichell@Mailinator.com",
                    firstName = "Jason",
                    lastName = "Twichell"
                }, "Abc&123!");
            }

            userId = userManager.FindByEmail("jTwichell@Mailinator.com").Id;
            userManager.AddToRole(userId, "ProjectManager");

            if (!context.Users.Any(r => r.Email == "brentdavis56@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "brentdavis56@Mailinator.com",
                    Email = "brentdavis56@Mailinator.com",
                    firstName = "Brent",
                    lastName = "Davis"
                }, "password");
            }

            userId = userManager.FindByEmail("brentdavis56@Mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            if (!context.Users.Any(r => r.Email == "codeboys@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "codeboys@Mailinator.com",
                    Email = "codeboys@Mailinator.com",
                    firstName = "Bobby",
                    lastName = "Davis"
                }, "password");
            }

            userId = userManager.FindByEmail("codeboys@Mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");
        }
    }
}
