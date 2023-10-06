using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ASDAssignmentUTS.Models;

namespace ASDAssignmentUTS.Controllers
{
    public class AdminController : Controller
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
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminUserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
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
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
