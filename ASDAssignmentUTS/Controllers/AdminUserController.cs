﻿using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ASDAssignmentUTS.Models;

namespace ASDAssignmentUTS.Controllers
{
    public class AdminUserController : Controller
    {
        // GET: AdminUserController
        public ActionResult UserManagement()
        {
            List<User> users = UserDBManager.GetUsers();
            return View(users);
        }

        // GET: AdminUserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminUserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminUserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            //try
            //{
                //creates a new user object instance.
                User user = new User
                (
                    collection["username"],
                    collection["password"],
                    collection["email"]
                );
                //adds the user to the database.
                UserDBManager.AddUser(user);
                return RedirectToAction(nameof(UserManagement));
            //}
            //catch
            //{
              //  return View();
            //}
        }

        // GET: AdminUserController/Edit/5
        public ActionResult Update(int id)
        {
            User user = UserDBManager.GetUserById(id);
            return View(user);
        }

        // POST: AdminUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, IFormCollection collection)
        {
            try
            {
                User user = new User
                (
                    id,
                    collection["username"],
                    collection["password"],
                    collection["email"]
                );
                UserDBManager.UpdateUser(user);
                return RedirectToAction(nameof(UserManagement));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminUserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminUserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {   UserDBManager.DeleteUser(id);
                return RedirectToAction(nameof(UserManagement));
            }
            catch
            {
                return View();
            }
        }
    }
}