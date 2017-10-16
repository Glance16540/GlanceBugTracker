using GlanceBugTracker.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GlanceBugTracker.Models.Helpers
{
    public class HistoryHelper 
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void AssignChange(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Tickets.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.AssignToUser.FullName;
            ticketHistory.NewValue = db.Users.Find(ticket.AssignToUserId).FullName;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "AssignToUserId";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistories.Add(ticketHistory);
            db.SaveChanges();
        }

        public void AssignTicketTitle(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Tickets.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.Title;
            ticketHistory.NewValue = ticket.Title;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Title";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistories.Add(ticketHistory);
            db.SaveChanges();
        }

        public void AssignTicketPriority(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Tickets.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.TicketPriorityId.ToString();
            ticketHistory.NewValue = ticket.TicketPriorityId.ToString();
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Priority";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistories.Add(ticketHistory);
            db.SaveChanges();
        }

        public void AssignTicketStatus(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Tickets.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.TicketStatus.Name;
            ticketHistory.NewValue = db.TicketStatuses.Find(ticket.TicketStatusid).Name;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Status";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistories.Add(ticketHistory);
            db.SaveChanges();
        }

        public void AssignTickettype(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Tickets.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.TicketType.Name;
            ticketHistory.NewValue = db.TicketTypes.Find(ticket.TicketTypeId).Name;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Type";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistories.Add(ticketHistory);
            db.SaveChanges();
        }

        public void AssignTicketDescription(Ticket ticket, string userId)
        {
            TicketHistory ticketHistory = new TicketHistory();
            Ticket oldTicket = db.Tickets.AsNoTracking().First(t => t.Id == ticket.Id);
            ticketHistory.OldValue = oldTicket.Description;
            ticketHistory.NewValue = ticket.Description;
            ticketHistory.TicketId = ticket.Id;
            ticketHistory.Property = "Description";
            ticketHistory.Created = DateTimeOffset.UtcNow;
            ticketHistory.AuthorId = userId;
            db.TicketHistories.Add(ticketHistory);
            db.SaveChanges();
        }

        //public void AssignTicketattachment(TicketAttachment attachment, string userId)
        //{
        //    TicketHistory ticketHistory= new TicketHistory();
           
        //    Ticket oldTicket = db.TicketAttachments.AsNoTracking().First(t => t. == attachment.Id);
        //    ticketHistory.OldValue = oldTicket;
        //    ticketHistory.NewValue = ticket.Description;
        //    ticketHistory.TicketId = ticket.Id;
        //    ticketHistory.Property = "Description";
        //    ticketHistory.Created = DateTimeOffset.UtcNow;
        //    ticketHistory.AuthorId = userId;
        //    db.TicketHistories.Add(ticketHistory);
        //    db.SaveChanges();
        //}
    }
}