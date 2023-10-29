using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Repositories;
using System.Configuration;

namespace ASDAssignmentUTS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;

        public AccountController(UserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpGet]
        public IActionResult Delete()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Validate user with UserRepository
                if (_userRepository.ValidateUser(model))
                {
                    int? userId = _userRepository.GetUserIdByUsername(model.Username);

                    // Store the userId int in the session
                    HttpContext.Session.SetInt32("LoggedInUser", userId.Value);

                    // User is validated successfully
                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Use UserRepository to register the user
                _userRepository.AddUser(model);
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Settings()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(IFormCollection collection)
        {
            try
            {
                string username = collection["username"];
                string password = collection["password"];

                // First, validate the username and password.
                var validUser = _userRepository.ValidateUser(new LoginModel
                {
                    Username = username,
                    Password = password
                });

                // If the credentials are valid, proceed with deletion.
                if (validUser)
                {
                    int? userId = _userRepository.GetUserIdByUsername(username);
                    if (userId.HasValue)
                    {
                        _userRepository.DeleteUser(userId.Value);
                        return RedirectToAction("Login"); // Redirects to Logout after successful deletion
                    }
                    else
                    {
                        ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                        return View(); // Might need to redirect to a specific view for error feedback
                    }
                }
                else
                {
                    // Handle invalid credentials
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(); // Might need to redirect to a specific view for invalid credentials
                }
            }
            catch
            {
                // Handle general exceptions
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again later.");
                return View();
            }
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            return RedirectToAction("Login");
        }



    }
}