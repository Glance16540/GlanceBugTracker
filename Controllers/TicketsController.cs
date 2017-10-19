using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlanceBugTracker.Models;
using GlanceBugTracker.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using System.IO;
using GlanceBugTracker.Models.Helpers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Configuration;

namespace GlanceBugTracker.Controllers
{
    public class TicketsController : Universal
    {
        // GET: Tickets
        [Authorize(Roles = "Admin")]
        public ActionResult Archive()
        {
            var tickets = db.Tickets.Include(t => t.AssignToUser).Include(t => t.Owneruser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }


        // GET: Tickets
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignToUser).Include(t => t.Owneruser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);

            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                return View(ticket);
            }
            
          
            var user = db.Users.Find(User.Identity.GetUserId());
            
            if (!User.IsInRole("Admin") && user.Id == ticket.AssignToUserId)
            {
                return View(ticket);
            }

            if (!User.IsInRole("Admin") && user.Id == ticket.OwnerUserId)
            {
                return View(ticket);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            

         
           
        }

        // GET: Tickets/AssignDeveloper/
        [Authorize(Roles =("Admin , Project Manager"))]
        public ActionResult AssignDeveloper(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            
            var user = db.Users.Find(User.Identity.GetUserId());
            UserRoleHelper userRoleHelper = new UserRoleHelper();
            var developers = userRoleHelper.UsersInRole("Developer");
            var devsOnProj = developers.Where(d => d.Projects.Any(p => p.Id == ticket.ProjectId));
            ViewBag.AssignToUserId = new SelectList(devsOnProj, "Id", "FullName", ticket.AssignToUserId);
         
            return View(ticket);
        }

        //POST: Tickets/AssignDeveloper/
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignDeveloper(string AssignToUserId, int id, EmailModel model)
        {
            Ticket ticket = db.Tickets.Find(id);
            ticket.AssignToUserId = AssignToUserId;
            ticket.TicketStatusid = db.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned").Id;
            var user = db.Users.Find(User.Identity.GetUserId());
            HistoryHelper helper = new HistoryHelper();
            Ticket oldTicket = db.Tickets.AsNoTracking().First(t => t.Id == ticket.Id);
            if (oldTicket.AssignToUserId != ticket.AssignToUserId)
            {
                helper.AssignChange(ticket, user.Id);
            }



            db.SaveChanges();


            try
            {
                var body = "<p>{0}</p><p>({1})</p>";
                var from = "BugTrackerServerNOREPLY<noreplybugtracker@gmail.com>";
                //model.Body = "This is a message from your bugtacker notification system. You have been assigned to a ticket! ";
                var email = new MailMessage(from, db.Users.Find(ticket.AssignToUserId).Email)
                {
                    Subject = "Notification of Ticket Assignment",
                    Body = string.Format(body, "subject", "This is a message from your bugtacker notification system. You have been assigned to a ticket!  Please do not respond to this message as you will not get a reply back. "),
                    IsBodyHtml = true
                };
                var svc = new PersonalEmail();
                await svc.SendAsync(email);
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }

            

            return RedirectToAction("UserProjects", "Projects");
        }

        // GET: Tickets/Create
           [Authorize(Roles ="Submitter")]
        public ActionResult Create(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.Project = id;

            ViewBag.ProjectId = new SelectList(db.Projects.Where(p => p.Users.Any(u => u.Id == user.Id)), "Id", "Title");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritites, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles ="Submitter")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusid,OwnerUserId,AssignToUserId")] Ticket ticket)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.ProjectId = new SelectList(db.Projects.Where(p => p.Users.Any(u => u.Id == user.Id)), "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritites, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusid = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusid);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            
            
            if (ModelState.IsValid)
            {
                
                ticket.TicketStatusid = db.TicketStatuses.FirstOrDefault(t => t.Name == "Unassigned").Id;
                ticket.OwnerUserId = user.Id;
                ticket.Archive = false;
                ticket.Created = DateTimeOffset.UtcNow;
                ticket.Updated = DateTimeOffset.UtcNow;

                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("UserProjects","Projects");
            }


           
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            
            var user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritites, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusid = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusid);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);

            if(ticket.TicketStatusid == 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }

            if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                return View(ticket);
            }

            if (!User.IsInRole("Admin") && user.Id == ticket.AssignToUserId)
            {
                return View(ticket);
            }

            if (user.Id == ticket.OwnerUserId)
            {
                return View(ticket);
            }
            
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusid,OwnerUserId,AssignToUserId")] Ticket ticket)
        {
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritites, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusid = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusid);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            var user = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
               
                ticket.Updated = DateTimeOffset.UtcNow;
                HistoryHelper helper = new HistoryHelper();
                
                TicketHistory ticketHistory = new TicketHistory();
                Ticket oldTicket = db.Tickets.AsNoTracking().First(t => t.Id == ticket.Id);
                if (oldTicket.Title != ticket.Title)
                {
                    helper.AssignTicketTitle(ticket, user.Id);
                }
                if (oldTicket.Description != ticket.Description)
                {
                    helper.AssignTicketDescription(ticket, user.Id);
                }
                if(oldTicket.TicketPriorityId != ticket.TicketPriorityId)
                {
                    helper.AssignTicketPriority(ticket, user.Id);
                }
                if(oldTicket.TicketStatusid != ticket.TicketStatusid)
                {
                    helper.AssignTicketStatus(ticket, user.Id);
                }
                if(oldTicket.TicketTypeId != ticket.TicketTypeId)
                {
                    helper.AssignTickettype(ticket, user.Id);

                }

                if (ticket.TicketStatusid == 1)
                {
                    ticket.Archive = true;
                }
                 
               

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                




                return RedirectToAction("Details", new { id = ticket.Id });
            }
           
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        //[Authorize]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Ticket ticket = db.Tickets.Find(id);
        //    if (ticket == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticket);
        //}







        //// POST: Tickets/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Ticket ticket = db.Tickets.Find(id);
        //    db.Tickets.Remove(ticket);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        //POST: Create Comment
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "Id,Body,TicketId,AuthorId,Created,Updated")] TicketComment comment, int TicketId)
        {
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            if (ModelState.IsValid)
            {

                comment.TicketId = TicketId;
                comment.AuthorId = User.Identity.GetUserId();
                
                comment.Created = DateTimeOffset.Now;
                db.TicketComments.Add(comment);
                HistoryHelper helper = new HistoryHelper();



                db.SaveChanges();
                return RedirectToAction("Details", new { id = TicketId });
            }

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FullName", comment.AuthorId);
            return View(comment);
        }

        // GET: Tickets/EditComment/5
        [Authorize]
        public ActionResult EditComment(int? id)
        {
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            var user = db.Users.Find(User.Identity.GetUserId());
            Ticket ticket = db.Tickets.Find(id);
            TicketComment ticketcomment = db.TicketComments.Find(id);


            if ((User.IsInRole("Admin") || (User.IsInRole("ProjectManager") && ticket.OwnerUserId == user.Id) || ticketcomment.AuthorId == user.Id))

            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (ticketcomment == null)
                {
                    return HttpNotFound();
                }

            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticketcomment.AuthorId);
            ViewBag.CommentId = new SelectList(db.TicketComments, "Id", "Title", ticketcomment.TicketId);
            return View(ticketcomment);

        }



        // POST: Tickets/EditComment/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment([Bind(Include = "Id,TicketId,AuthorId,Body,Created")] TicketComment ticketcomment)
        {
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            var user = db.Users.Find(User.Identity.GetUserId());
            Ticket ticket = db.Tickets.Find(ticketcomment.TicketId);
           
            if ((User.IsInRole("Admin") || (User.IsInRole("ProjectManager") && ticket.OwnerUserId == user.Id) || ticketcomment.AuthorId == user.Id))
            {
                if (ModelState.IsValid)
                {
                    ticketcomment.Updated = DateTimeOffset.UtcNow;
                    db.Entry(ticketcomment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = ticket.Id });
                }
                ViewBag.AuthorId = new SelectList(/*db.ApplicationUsers,*/ "Id", "FirstName", ticketcomment.AuthorId);
                ViewBag.CommentId = new SelectList(db.TicketComments, "Id", "Title", ticketcomment.Id);
                return RedirectToAction("Details", new { id = ticket.Id });
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }



       

           


        }



        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment comment = db.TicketComments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(int id)
        {
            Ticket ticket = new Ticket();
            TicketComment ticketcomment = db.TicketComments.Find(id);
            

            db.TicketComments.Remove(ticketcomment);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("UserProjects", "Projects");
        }

        //POST: Tickets/CreateAttachment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAttachment(
            [Bind(Include = "Id,Description,TicketId")] TicketAttachment ticketattachment, HttpPostedFileBase attachFile) //Bind Attribute tells it to add these properties when it sends to view
        {
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            var user = db.Users.Find(User.Identity.GetUserId());
            if (attachFile != null)
            {

                var ext = Path.GetExtension(attachFile.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    ModelState.AddModelError("image", "Invalid Format.");
            }
            if (ModelState.IsValid) //makes sure all the properties are bound
            {
                if (attachFile != null)

                {
                    ticketattachment.Created = DateTimeOffset.Now;
                    ticketattachment.AuthorId = user.Id;
                    var filePath = "/Content/Assest/UserUploads/";
                    var absPath = Server.MapPath("~" + filePath);
                    ticketattachment.FileUrl = filePath + attachFile.FileName;
                    attachFile.SaveAs(Path.Combine(absPath, attachFile.FileName));

                }
                db.TicketAttachments.Add(ticketattachment);
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Tickets", new { id = ticketattachment.TicketId });

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
