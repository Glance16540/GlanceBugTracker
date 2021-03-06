﻿using GlanceBugTracker.Models;
using GlanceBugTracker.Models.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlanceBugTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Universal
    {
     

        private UserRoleHelper helper = new UserRoleHelper();
       
        public ActionResult Index()
        {
            UserRoleHelper helper = new UserRoleHelper();
            List<AdminUserViewModel> users = new List<AdminUserViewModel>();
            foreach(var user in db.Users.ToList())
            {
                var eachUser = new AdminUserViewModel();
                eachUser.User = user;
                eachUser.SelectedRoles = helper.ListUserRoles(user.Id).ToArray();

                users.Add(eachUser);

            }
            return View(users.OrderBy(u => u.User.LastName).ToList());
        }

        //GET: EdituserRoles
        [Authorize(Roles=("Admin"))]
        public ActionResult EditUserRoles(string id)
        {
            var user = db.Users.Find(id);
            var helper = new UserRoleHelper();
            var model = new AdminUserViewModel();
            model.User = user;
            model.SelectedRoles = helper.ListUserRoles(id).ToArray();
            model.Roles = new MultiSelectList(db.Roles, "Name", "Name", model.SelectedRoles);

            return View(model);
        }

        //POST: EditUserRoles
        [HttpPost]
        [Authorize(Roles =("Admin"))]
        public ActionResult EditUserRoles(AdminUserViewModel model)
        {
            var user = db.Users.Find(model.User.Id);
            UserRoleHelper helper = new UserRoleHelper();
            foreach (var role in db.Roles.Select(r => r.Name).ToList())
            {
                helper.RemoveUserFromRole(user.Id, role);
            }
            if (model.SelectedRoles != null)
            {
                foreach (var role in model.SelectedRoles)
                {
                    helper.AddUserToRole(user.Id, role);
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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