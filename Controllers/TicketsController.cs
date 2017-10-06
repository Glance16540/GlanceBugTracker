﻿using System;
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

namespace GlanceBugTracker.Controllers
{
    public class TicketsController : Universal
    {


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
            return View(ticket);
        }

        // GET: Tickets/AssignDeveloper/
        [Authorize]
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
            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignToUserId);
            return View(ticket);
        }

        //POST: Tickets/AssignDeveloper/
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AssignDeveloper(string AssignToUserId, int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            ticket.AssignToUserId = AssignToUserId;
            ticket.TicketStatusid = db.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned").Id;
            db.SaveChanges();
            return RedirectToAction("UserProjects", "Projects");
        }

        // GET: Tickets/Create
  
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

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
        [Authorize(Roles ="Submitter , Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusid,OwnerUserId,AssignToUserId")] Ticket ticket)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
              
               
                ticket.TicketStatusid = db.TicketStatuses.FirstOrDefault(t => t.Name == "Unassigned").Id;
                ticket.OwnerUserId = user.Id;
                ticket.Created = DateTimeOffset.Now;
                ticket.Updated = DateTimeOffset.Now;

                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("UserProjects","Projects");
            }


            ViewBag.ProjectId = new SelectList(db.Projects.Where(p => p.Users.Any(u => u.Id == user.Id)), "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritites, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusid = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusid);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles ="Admin")]
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
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritites, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusid = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusid);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusid,OwnerUserId,AssignToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPrioritites, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusid = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusid);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
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
            return View(ticket);
        }







        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //POST: Create Comment
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "Id,Body,TicketId,AuthorId,Created,Updated")] TicketComment comment, int TicketId)
        {
            if (ModelState.IsValid)
            {

                comment.TicketId = TicketId;
                comment.AuthorId = User.Identity.GetUserId();
                
                comment.Created = DateTimeOffset.Now;
                db.TicketComments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            return View(comment);
        }

        // GET: Tickets/EditComment/5
        public ActionResult EditComment(int? id)
        {
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
            var user = db.Users.Find(User.Identity.GetUserId());
           
            if ((User.IsInRole("Admin") || (User.IsInRole("ProjectManager")) || ticketcomment.AuthorId == user.Id))
            {
                if (ModelState.IsValid)
                {
                    ticketcomment.Updated = DateTimeOffset.UtcNow;
                    db.Entry(ticketcomment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.AuthorId = new SelectList(/*db.ApplicationUsers,*/ "Id", "FirstName", ticketcomment.AuthorId);
                ViewBag.CommentId = new SelectList(db.TicketComments, "Id", "Title", ticketcomment.Id);
                return RedirectToAction("Details", new { id = ticketcomment.Id });
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

            TicketComment ticketcomment = db.TicketComments.Find(id);
          

            db.TicketComments.Remove(ticketcomment);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Details","Tickets", new {id = ticketcomment.TicketId });
        }

        //POST: Tickets/CreateAttachment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAttachment(
            [Bind(Include = "Id,Description,TicketId")] TicketAttachment ticketattachment, HttpPostedFileBase attachFile) //Bind Attribute tells it to add these properties when it sends to view
        {
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
