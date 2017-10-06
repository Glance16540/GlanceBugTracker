using GlanceBugTracker.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlanceBugTracker.Models.Helpers
{
    public class TicketUserViewModel
    {
        public int AssignProjectId { get; set; }
        public MultiSelectList Users { get; set; }
        public string[] SelectedUsers { get; set; }
        public Project AssignProject { get; set; }
        public string[] SelectedUsersName { get; set; }
    }
}