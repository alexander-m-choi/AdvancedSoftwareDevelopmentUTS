using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Repositories;
using System.Security.Claims;
using System.Collections.Generic;


namespace ASDAssignmentUTS.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;

        [TempData]
        public string StatusMessage { get; set; }

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
    if (User?.Identity?.IsAuthenticated != true) // Checking for null and if the user is authenticated
    {
        return RedirectToAction("Login");
    }

    return View(); 
}




        [HttpPost]
public async Task<IActionResult> Login(LoginModel model)
{
    if (ModelState.IsValid)
    {
        // Validate user with UserRepository
        if (_userRepository.ValidateUser(model))
        {
            if (model.Username is not null) // Added null-check
            {
                // User is validated successfully
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username)
                    // You can add other claims as needed
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Home", "Home");
            }
            else
            {
                // Handle the null username scenario
                ModelState.AddModelError("", "Username cannot be null.");
            }
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
        // Check if the username exists
    if (_userRepository.UsernameExists(model.Username))
    {
        ModelState.AddModelError("Username", "This username is already taken. Please choose another.");
    }
        // Check if the email exists
    if (_userRepository.EmailExists(model.Email)) 
    {
        ModelState.AddModelError("Email", "This email address is already registered. Please use another or log in.");
    }

    if (ModelState.IsValid)
    {
        try
        {
            // Use UserRepository to register the user
            _userRepository.AddUser(model);
            return RedirectToAction("Login");
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while registering. Please try again.");
        }
    }

    return View("~/Views/UserRegistration/UserRegistration.cshtml", model);

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
        string? enteredUsername = collection["username"];
        string? password = collection["password"];

        // Check if the entered username matches the currently authenticated user's username
            if (enteredUsername != User?.Identity?.Name)
            {
                StatusMessage = "You can only delete your own account.";
                return RedirectToAction("Settings"); 
            }

            if (string.IsNullOrEmpty(enteredUsername) || string.IsNullOrEmpty(password))
            {
                StatusMessage = "Username or password is missing.";
                return RedirectToAction("Settings"); // Return to Settings with the message
            }

        // Validate the entered username and password.
        var validUser = _userRepository.ValidateUser(new LoginModel
        {
            Username = enteredUsername,
            Password = password 
        });

        // If the credentials are valid, proceed with deletion.
        if (validUser)
        {
            int? userId = _userRepository.GetUserIdByUsername(enteredUsername);
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
                StatusMessage = "Invalid username or password.";
                return RedirectToAction("Settings");
            }
        }
        catch
        {
            StatusMessage = "An error occurred while processing your request. Please try again later.";
            return RedirectToAction("Settings");
        }
    }


[HttpPost]
public async Task<IActionResult> Logout()
{
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Login");
}
        
    }
}