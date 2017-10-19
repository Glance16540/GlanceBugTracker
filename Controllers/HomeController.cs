using GlanceBugTracker.Models;
using GlanceBugTracker.Models.CodeFirst;
using GlanceBugTracker.Models.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GlanceBugTracker.Controllers
{

    public class HomeController : Universal
    {
        [Authorize]
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            ProjectAssignHelper helper = new ProjectAssignHelper();
            var result = helper.ListUserProjects(id);
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;

            var thisMonth = System.DateTime.Now;
            var lastMonth = System.DateTime.Now.AddMonths(-1);
            var monthsAgo2 = System.DateTime.Now.AddMonths(-2);
            var monthsAgo3 = System.DateTime.Now.AddMonths(-3);
            var monthsAgo4 = System.DateTime.Now.AddMonths(-4);
            var monthsAgo5 = System.DateTime.Now.AddMonths(-5);
            ViewBag.ThisMonth = thisMonth.ToString("MMMM");
            ViewBag.LastMonth = lastMonth.ToString("MMMM");
            ViewBag.MonthsAgo2 = monthsAgo2.ToString("MMMM");
            ViewBag.MonthsAgo3 = monthsAgo3.ToString("MMMM");
            ViewBag.MonthsAgo4 = monthsAgo4.ToString("MMMM");
            ViewBag.MonthsAgo5 = monthsAgo5.ToString("MMMM");

            ViewBag.TicketsAddedThisMonth = db.Tickets.Where(t => t.Created.Month == thisMonth.Month && t.Created.Year == thisMonth.Year).Count();
            ViewBag.TicketsAddedLastMonth = db.Tickets.Where(t => t.Created.Month == lastMonth.Month && t.Created.Year == lastMonth.Year).Count();
            ViewBag.TicketsAdded2MonthsAgo = db.Tickets.Where(t => t.Created.Month == monthsAgo2.Month && t.Created.Year == monthsAgo2.Year).Count();
            ViewBag.TicketsAdded3MonthsAgo = db.Tickets.Where(t => t.Created.Month == monthsAgo3.Month && t.Created.Year == monthsAgo3.Year).Count();
            ViewBag.TicketsAdded4MonthsAgo = db.Tickets.Where(t => t.Created.Month == monthsAgo4.Month && t.Created.Year == monthsAgo4.Year).Count();
            ViewBag.TicketsAdded5MonthsAgo = db.Tickets.Where(t => t.Created.Month == monthsAgo5.Month && t.Created.Year == monthsAgo5.Year).Count();

            var completedThisMonth = new List<Ticket>();
            var completedLastMonth = new List<Ticket>();
            var completed2MonthsAgo = new List<Ticket>();
            var completed3MonthsAgo = new List<Ticket>();
            var completed4MonthsAgo = new List<Ticket>();
            var completed5MonthsAgo = new List<Ticket>();

            var completedTickets = db.Tickets.Where(t => t.TicketStatus.Name == "Completed").ToList();

            foreach (var ticket in completedTickets)
            {
                var ticketCompletedHistory = ticket.Histories.Last(t => t.Property == "Status" && t.NewValue == "Completed");
                if (ticketCompletedHistory != null)
                {
                    if (ticketCompletedHistory.Created.Month == thisMonth.Month && ticketCompletedHistory.Created.Year == thisMonth.Year)
                    {
                        completedThisMonth.Add(ticket);
                    }
                    else if (ticketCompletedHistory.Created.Month == lastMonth.Month && ticketCompletedHistory.Created.Year == lastMonth.Year)
                    {
                        completedLastMonth.Add(ticket);
                    }
                    else if (ticketCompletedHistory.Created.Month == monthsAgo2.Month && ticketCompletedHistory.Created.Year == monthsAgo2.Year)
                    {
                        completed2MonthsAgo.Add(ticket);
                    }
                    else if (ticketCompletedHistory.Created.Month == monthsAgo3.Month && ticketCompletedHistory.Created.Year == monthsAgo3.Year)
                    {
                        completed3MonthsAgo.Add(ticket);
                    }
                    else if (ticketCompletedHistory.Created.Month == monthsAgo4.Month && ticketCompletedHistory.Created.Year == monthsAgo4.Year)
                    {
                        completed4MonthsAgo.Add(ticket);
                    }
                    else if (ticketCompletedHistory.Created.Month == monthsAgo5.Month && ticketCompletedHistory.Created.Year == monthsAgo5.Year)
                    {
                        completed5MonthsAgo.Add(ticket);
                    }
                }
            }

            ViewBag.TicketsCompletedThisMonth = completedThisMonth.Count();
            ViewBag.TicketsCompletedLastMonth = completedLastMonth.Count();
            ViewBag.TicketsCompleted2MonthsAgo = completed2MonthsAgo.Count();
            ViewBag.TicketsCompleted3MonthsAgo = completed3MonthsAgo.Count();
            ViewBag.TicketsCompleted4MonthsAgo = completed4MonthsAgo.Count();
            ViewBag.TicketsCompleted5MonthsAgo = completed5MonthsAgo.Count();

            return View(result);           
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold>({1})</p><p>Message: </p><p>{2}</p>";
                    var from = "MyPortfolio<antonio@raynor.com>";
                    model.Body = "This is a message from your portfolio site. The name and the email of the contacting person is above.";
                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = "Portfolio Contact Email",
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };
                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    return RedirectToAction("Sent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }

        public ActionResult Landing()
        {
            return View();
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




    }
}