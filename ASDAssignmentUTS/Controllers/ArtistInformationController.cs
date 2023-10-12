using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;

namespace ASDAssignmentUTS.Controllers
{
    public class ArtistInformationController : Controller
    {
        public IActionResult ArtistInformation(int artistID)
        {
            Artist artist = ArtistDBManager.GetArtistInformation(artistID);
            if (artist == null)
            {
                return NotFound();
            }

            return View("/Views/Artist/ArtistInformation.cshtml", artist);
        }
    }
}