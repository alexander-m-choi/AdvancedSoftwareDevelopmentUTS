using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;

namespace ASDAssignmentUTS.Controllers
{
    public class ArtistSearchController : Controller
    {
        public IActionResult ArtistSearch()
        {
            List<Artist> artists = ArtistDBManager.GetArtists();
            return View("/Views/Artist/ArtistSearch.cshtml", artists);
        }
    }
}
