using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASDAssignmentUTS.Controllers
{
    public class SongInformationController : Controller
    {

        // GET: SongInformationController/Details/5
        public IActionResult SongDetails(int? id)
        {
            if (id.HasValue)
            {
                Song song = SongDBManager.GetSongById(id.Value);
                return View(song);
            }
            else
            {
                return RedirectToAction("SongSearch", "SongSearch");
            }
        } 


    }
}




