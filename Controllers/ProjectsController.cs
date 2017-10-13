using System;

using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;

using System.Web.Mvc;
using GlanceBugTracker.Models;
using GlanceBugTracker.Models.CodeFirst;
using GlanceBugTracker.Models.Helpers;
using Microsoft.AspNet.Identity;

namespace GlanceBugTracker.Controllers
{
    public class ProjectsController : Universal
    {


        // GET Index: Projects
        [Authorize(Roles =("Admin , Projects Manager"))]
        public ActionResult Index()
        {
         
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            return View(db.Projects.ToList());
        }
        
        // GET Details: Projects/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectAssignHelper helper = new ProjectAssignHelper();
            var user = db.Users.Find(User.Identity.GetUserId());

            if (!User.IsInRole("Admin") && helper.IsUserOnProject(user.Id, project.Id) == false)
            {
               
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);           
            }
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin , Project Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin , Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Created,Updated,Title,Description,AuthorId")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Author = ViewBag.FullName;
                project.AuthorId = User.Identity.GetUserId();
                project.AuthorId = project.Author;
                project.Created = DateTimeOffset.UtcNow;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize (Roles = "Admin , Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin , Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Updated,Title,Description,AuthorId")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Created = project.Created;
                project.Updated = DateTimeOffset.UtcNow;
                project.Author = ViewBag.FullName;
                project.AuthorId = User.Identity.GetUserId();
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin , Project Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin , Project Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //GET: see project users
        [Authorize(Roles = "Admin , Project Manager")]
        public ActionResult ProjectUser(int id)
        {
            var project = db.Projects.Find(id);
            ProjectUserViewModel projectuserVM = new ProjectUserViewModel();
            projectuserVM.AssignProject = project;
            projectuserVM.AssignProjectId = id;
            projectuserVM.SelectedUsers = project.Users.Select(u => u.Id).ToArray();
            projectuserVM.Users = new MultiSelectList(db.Users.ToList(), "Id", "FullName", projectuserVM.SelectedUsers);
            
            return View(projectuserVM);
        }

        //POST: Add/Remove project Users 
        [HttpPost]
        [Authorize(Roles = "Admin , Project Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectUser(ProjectUserViewModel model)
        {
        
            ProjectAssignHelper projectAH = new ProjectAssignHelper();
            foreach(var userId in db.Users.Select(r => r.Id).ToList())
            {
                projectAH.RemoveUserFromProject(userId, model.AssignProjectId);
            }

            foreach(var userId in model.SelectedUsers)
            {
                projectAH.AddUserToProject(userId, model.AssignProjectId);
            }


            return RedirectToAction("Index");
        }

        //Get: 
        [Authorize]
        public ActionResult Userprojects()
        {
            if (ModelState.IsValid)
            {
                var id = User.Identity.GetUserId();
                ProjectAssignHelper helper = new ProjectAssignHelper();
                var result = helper.ListUserProjects(id);
                return View(result);
            }
            ViewBag.UserTimeZone = db.Users.Find(User.Identity.GetUserId()).TimeZone;

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
