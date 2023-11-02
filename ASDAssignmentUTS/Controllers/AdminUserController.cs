using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ASDAssignmentUTS.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using ASDAssignmentUTS.Repositories;

namespace ASDAssignmentUTS.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminUserController : Controller
    {
        private readonly UserRepository _userRepository;

        public AdminUserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
            string connectionString = DBConnector.GetConnectionString();
            _userRepository = new UserRepository(connectionString);
        }
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
                //creates a new user object instance.
                User user = new User
                (
                    collection["username"],
                    collection["password"],
                    collection["email"]
                );

                // Check if the username exists
                if (_userRepository.UsernameExists(user.username))
                {
                    ModelState.AddModelError("username", "This username is already taken. Please choose another.");
                }
                // Check if the email exists
                if (_userRepository.EmailExists(user.email))
                {
                    ModelState.AddModelError("emailAddress", "This email address is already registered. Please use another or log in.");
                }

                //adds the user to the database.
                UserDBManager.AddUser(user);
                return RedirectToAction(nameof(UserManagement));
            }
            catch
            {
                return View();
            }
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

        // POST: AdminUserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                UserDBManager.DeleteUser(Convert.ToInt32(collection["id"]));
                return RedirectToAction(nameof(UserManagement));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ResetPassword(int id)
        {
            User user = UserDBManager.GetUserById(id);
            //generates random password
            Random random = new Random();
            string password = "";
            for (int i = 0; i < 8; i++)
            {
                //this is an array that contains all chracter range for alphanumeric characters
                char[] charRange =
                {
                    (char)random.Next(48, 58),
                    (char)random.Next(65, 91),
                    (char)random.Next(97, 123)
                };

                int randomCharRange = random.Next(0, 3);
                //this will regenerate a random number if it is 3 to prevent an index out of range exception.
                if (randomCharRange == 3)
                {
                    randomCharRange = random.Next(0, 3);
                }

                //this will randomly select a character range from the array and add it to the password string
                password += charRange[randomCharRange];
                Debug.WriteLine(randomCharRange);
            }
            //shows the new password to the user
            ViewBag.password = password;
            //updates the user's password
            UserDBManager.ResetPassword(id, password);
            return View(user);
        }
    }
}
