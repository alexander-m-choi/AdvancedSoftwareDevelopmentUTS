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

            id = 1;
            Song song = SongDBManager.GetSongById(id);
            return View(song);
        }


    }
}




