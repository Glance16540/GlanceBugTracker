namespace GlanceBugTracker.Migrations
{
    using GlanceBugTracker.Models;
    using GlanceBugTracker.Models.CodeFirst;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GlanceBugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GlanceBugTracker.Models.ApplicationDbContext context)
        {
            {
                var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
                if (!context.Roles.Any(r => r.Name == "Admin"))
                {
                    roleManager.Create(new IdentityRole { Name = "Admin" });
                }
                if (!context.Roles.Any(r => r.Name == "Project Manager"))
                {
                    roleManager.Create(new IdentityRole { Name = "Project Manager" });
                }
                if (!context.Roles.Any(r => r.Name == "Developer"))
                {
                    roleManager.Create(new IdentityRole { Name = "Developer" });
                }
                if (!context.Roles.Any(r => r.Name == "Submitter"))
                {
                    roleManager.Create(new IdentityRole { Name = "Submitter" });
                }


                var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
                if (!context.Users.Any(u => u.Email == "glance16540@gmail.com"))
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "glance16540@gmail.com",
                        Email = "glance16540@gmail.com",
                        FirstName = "Garrett",
                        LastName = "Lance",
                    }, "Tuckerandhobbes1");
                }

                var userId = userManager.FindByEmail("glance16540@gmail.com").Id;
                userManager.AddToRole(userId, "Admin");


                if (!context.Users.Any(u => u.Email == "rchapman@coderfoundry.com"))
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "rchapman@coderfoundry.com",
                        Email = "rchapman@coderfoundry.com",
                        FirstName = "Ryan",
                        LastName = "Chapman",
                    }, "Password1!");
                }
                var modId1 = userManager.FindByEmail("rchapman@coderfoundry.com").Id;
                userManager.AddToRole(modId1, "Admin");



                if (!context.Users.Any(u => u.Email == "mjaang@coderfoundry.com"))
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "mjaang@coderfoundry.com",
                        Email = "mjaang@coderfoundry.com",
                        FirstName = "Mark",
                        LastName = "Jaang",
                    }, "Password1!");
                }
                var modId2 = userManager.FindByEmail("mjaang@coderfoundry.com").Id;
                userManager.AddToRole(modId2, "Admin");



                if (!context.Users.Any(u => u.Email == "admindemo@gmail.com"))
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "admindemo@gmail.com",
                        Email = "admindemo@gmail.com",
                        FirstName = "Admin",
                        LastName = "Demo",
                    }, "Password1!");
                }
                var Admindemo = userManager.FindByEmail("admindemo@gmail.com").Id;
                userManager.AddToRole(Admindemo, "Admin");



                if (!context.Users.Any(u => u.Email == "demoprojectmanager@gmail.com"))
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "demoprojectmanager@gmail.com",
                        Email = "demoprojectmanager@gmail.com",
                        FirstName = "Project",
                        LastName = "Demo",
                    }, "Password1!");
                }
                var demoprojectmanager = userManager.FindByEmail("demoprojectmanager@gmail.com").Id;
                userManager.AddToRole(demoprojectmanager, "Project Manager");


                if (!context.Users.Any(u => u.Email == "demodeveloper@gmail.com"))
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "demodeveloper@gmail.com",
                        Email = "demodeveloper@gmail.com",
                        FirstName = "Developer",
                        LastName = "Demo",
                    }, "Password1!");
                }
                var demodeveloper = userManager.FindByEmail("demodeveloper@gmail.com").Id;
                userManager.AddToRole(demodeveloper, "Developer");



                if (!context.Users.Any(u => u.Email == "demosubmitter@gmail.com"))
                {
                    userManager.Create(new ApplicationUser
                    {
                        UserName = "demosubmitter@gmail.com",
                        Email = "demosubmitter@gmail.com",
                        FirstName = "Submitter",
                        LastName = "Demo",
                    }, "Password1!");
                }
                var demosubmitter = userManager.FindByEmail("demosubmitter@gmail.com").Id;
                userManager.AddToRole(demosubmitter, "Submitter");

                if (!context.TicketPrioritites.Any(p => p.Name == "Low"))
                {
                    var priority = new TicketPriority();
                    priority.Name = "Low";
                    context.TicketPrioritites.Add(priority);
                }
                if (!context.TicketPrioritites.Any(p => p.Name == "Medium"))
                {
                    var priority = new TicketPriority();
                    priority.Name = "Medium";
                    context.TicketPrioritites.Add(priority);
                }
                if (!context.TicketPrioritites.Any(p => p.Name == "High"))
                {
                    var priority = new TicketPriority();
                    priority.Name = "High";
                    context.TicketPrioritites.Add(priority);
                }
                if (!context.TicketPrioritites.Any(p => p.Name == "Urgent"))
                {
                    var priority = new TicketPriority();
                    priority.Name = "Urgent";
                    context.TicketPrioritites.Add(priority);
                }

                if (!context.TicketStatuses.Any(p => p.Name == "Completed"))
                {
                    var status = new TicketStatus();
                    status.Name = "Completed";
                    context.TicketStatuses.Add(status);
                }
                if (!context.TicketStatuses.Any(p => p.Name == "In Progress"))
                {
                    var status = new TicketStatus();
                    status.Name = "In Progress";
                    context.TicketStatuses.Add(status);
                }
                if (!context.TicketStatuses.Any(p => p.Name == "Assigned"))
                {
                    var status = new TicketStatus();
                    status.Name = "Assigned";
                    context.TicketStatuses.Add(status);
                }
                if (!context.TicketStatuses.Any(p => p.Name == "Unassigned"))
                {
                    var status = new TicketStatus();
                    status.Name = "Unassigned";
                    context.TicketStatuses.Add(status);
                }

                if (!context.TicketTypes.Any(p => p.Name == "Hardware"))
                {
                    var type = new TicketType();
                    type.Name = "Hardware";
                    context.TicketTypes.Add(type);
                }
                if (!context.TicketTypes.Any(p => p.Name == "Software"))
                {
                    var type = new TicketType();
                    type.Name = "Software";
                    context.TicketTypes.Add(type);
                }
            }
        }
    }
}
