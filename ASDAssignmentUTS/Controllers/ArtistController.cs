using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASDAssignmentUTS.Models;

namespace ASDAssignmentUTS.Controllers
{
    public class ArtistController : Controller
    {
        public IActionResult Index()
        {
            var artists = new Artist().GetArtists();
            return View(artists);
        }
    }
}
