using GlanceBugTracker.Models.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlanceBugTracker.Models.Helpers
{
    public class ProjectAssignHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var userBool = project.Users.Any(u => u.Id == userId);
            return userBool;
        }

        public void AddUserToProject(string userId, int projectId)
        {
            var user = db.Users.Find(userId);
            var project = db.Projects.Find(projectId);
            project.Users.Add(user);
            db.SaveChanges();
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            var user = db.Users.Find(userId);
            var project = db.Projects.Find(projectId);
            project.Users.Remove(user);
            db.SaveChanges();
        }

        public List<Project> ListUserProjects(string userId)
        {
            var user = db.Users.Find(userId);
            return user.Projects.ToList();
        }

        public List<ApplicationUser> ListUsersOnProject(int projectId)
        {
            var project = db.Projects.Find(projectId);
            return project.Users.ToList();

        }

        public List<ApplicationUser> ListUserNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }
    }

}