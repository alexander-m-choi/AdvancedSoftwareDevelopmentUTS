using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ASDAssignmentUTS.Controllers
{
    public class SongSearchController : Controller
    {

        public ActionResult SongSearch()
        {
            List<Song> songs = SongDBManager.GetSongs();
            return View(songs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSongsByName(IFormCollection collection)
        {
            if (collection["name"] != "")
            {
                var song = new Song();
                //captures the data from the form that was imputed by the user.
                string name = collection["name"];
                List<Song> songs = SongDBManager.GetSongsByName(name);
                return View(songs);
            }
            return RedirectToAction(nameof(SongSearch));
        }
    }
}
