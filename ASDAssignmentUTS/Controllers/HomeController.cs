using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
namespace ASDAssignmentUTS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Home(int id)
        {
            User user = UserDBManager.GetUserById(id);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ArtistSearch()
        {             
            return View();
        }

        public IActionResult SongSearch()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AdminMenu()
        {
            return View();
        }
    }
}