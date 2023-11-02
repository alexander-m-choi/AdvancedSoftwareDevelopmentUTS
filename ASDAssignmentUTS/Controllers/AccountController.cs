using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Repositories;
using System.Security.Claims;
using System.Collections.Generic;
using ASDAssignmentUTS.Services;


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
            string connectionString = DBConnector.GetConnectionString();
            _userRepository = new UserRepository(connectionString);
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
        public IActionResult UserRegistration()
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
            if (model.Username is not null) // Added null-check for Username
            {
                // Fetch the user's role from the database.
                string userRole = _userRepository.GetUserRoleByUsername(model.Username);
                
                // User is validated successfully
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username)
                };

                // Add userRole to claims only if it's not null or empty
                if (!string.IsNullOrEmpty(userRole))
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Redirect based on the role
                if (string.Equals(userRole, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("AdminMenu", "Home");
                }
                else
                {
                    return RedirectToAction("Home", "Home");
                }
            }
            else
            {
                // Handle the null username scenario using TempData
                TempData["StatusMessage"] = "Username cannot be null.";
            }
        }
        else
        {
            // Handle the invalid username or password scenario using TempData
            TempData["StatusMessage"] = "Invalid username or password.";
        }
    }
    return View(model);
}






[HttpPost]
public IActionResult UserRegistration(RegisterModel model)
{
    if (ModelState.IsValid)
    {
        try
        {
            // Add user to database using the repository
            _userRepository.AddUser(model);

            // Provide a success message to the user
            ViewBag.SuccessMessage = "Registration successful! You can now log in.";
            return View("Login"); 
        }
        catch (Exception ex) // Catch any exceptions that occur during registration
        {
            // Provide an error message to the user
            ViewBag.ErrorMessage = "Registration failed: " + ex.Message;
            return View(model); // Redisplay the registration form with the error message
        }
    }

    // If model state is invalid, redisplay the form
    ViewBag.ErrorMessage = "Please correct the errors and try again.";
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
        // Check if Role is selected
        if (string.IsNullOrWhiteSpace(model.Role))
    {
        ModelState.AddModelError("Role", "Role is required.");
        return View("~/Views/Account/UserRegistration.cshtml", model);
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

    return View("~/Views/Account/UserRegistration.cshtml", model);

}


        public IActionResult Settings()
        {
            bool isAdmin = User.IsInRole("Admin");
            ViewData["IsAdmin"] = isAdmin;
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
public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
{
    if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
    {
        TempData["StatusMessage"] = "Please enter current password, new password, and confirm new password.";
        return RedirectToAction("Settings");
    }

    if(newPassword != confirmNewPassword)
    {
        TempData["StatusMessage"] = "New password and confirm new password do not match.";
        return RedirectToAction("Settings");
    }

    var storedPassword = _userRepository.GetCurrentPassword(User.Identity.Name);
    if (storedPassword != currentPassword)
    {
        TempData["StatusMessage"] = "The current password you entered is incorrect.";
        return RedirectToAction("Settings");
    }

    _userRepository.UpdatePassword(User.Identity.Name, newPassword);
    TempData["StatusMessage"] = "Password successfully changed.";
    return RedirectToAction("Settings");
}



[HttpPost]
public async Task<IActionResult> Logout()
{
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    TempData.Clear(); 
    HttpContext.Session.Clear();
    
    // Clear all cookies
    foreach (var cookie in Request.Cookies.Keys)
    {
        Response.Cookies.Delete(cookie);
    }
    return RedirectToAction("Login");
}
        
    }
}
