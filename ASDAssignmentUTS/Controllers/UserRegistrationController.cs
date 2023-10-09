using Microsoft.AspNetCore.Mvc;

namespace ASDAssignmentUTS.Controllers
{
    public class UserRegistrationController : Controller
    {
        public IActionResult UserRegistration()
        {
             return View("~/Views/UserRegistration/UserRegistration.cshtml");
        }
    }
}
