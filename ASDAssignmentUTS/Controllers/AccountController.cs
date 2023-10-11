using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Repositories; // Ensure you add this using directive

namespace ASDAssignmentUTS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;

        // Injecting UserRepository into the controller
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

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Validate user with UserRepository
                if (_userRepository.ValidateUser(model))
                {
                    // User is validated successfully
                    // Set up session/cookie and redirect to settings or desired page
                    return RedirectToAction("Index", "Home");
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

            return View(); // Ensure you have a corresponding Settings view
        }

        [HttpPost]
        public IActionResult DeleteAccount()
        {
            // Ensure user is logged in
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            // Use UserRepository to delete user account from database
            _userRepository.DeleteUser(User.Identity.Name); // You might need to implement this method in UserRepository

            // Handle session/cookie logout here
            // Then redirect to a confirmation or home page
            return RedirectToAction("Index", "Home"); // Adjust this as necessary
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           
            return RedirectToAction("Login"); // or redirect to login page, or wherever you want
        }


        
    }
}
