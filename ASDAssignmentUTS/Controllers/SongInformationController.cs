using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASDAssignmentUTS.Controllers
{
    public class SongInformationController : Controller
    {
        public IActionResult SongDetails(int id)
        {
            //Update the ID to be able to be parsed in from song search :))

            id = 2;
            Song song = SongDBManager.GetSongById(2);
            return View(song);
        }


    }
}




