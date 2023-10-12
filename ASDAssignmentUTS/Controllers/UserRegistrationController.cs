using Microsoft.AspNetCore.Mvc;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using ASDAssignmentUTS.Repositories;


namespace ASDAssignmentUTS.Controllers
{
    public class UserRegistrationController : Controller
    {
        private readonly UserRepository _userRepository;

        public UserRegistrationController()
        {
            string connectionString = DBConnector.GetConnectionString();
            _userRepository = new UserRepository(connectionString);
        }

        [HttpGet]
        public IActionResult UserRegistration()
        {
            return View(new RegisterModel()); 
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


    }
}
