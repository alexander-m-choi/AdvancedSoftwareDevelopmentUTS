using Microsoft.AspNetCore.Mvc;

namespace ASDAssignmentUTS.Controllers
{
    public class AccountController:Controller{
        public IActionResult Login()
        {
            return View();
        }
    }
}
