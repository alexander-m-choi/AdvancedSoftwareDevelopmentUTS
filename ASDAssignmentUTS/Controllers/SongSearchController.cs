using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace ASDAssignmentUTS.Controllers
{
    public class SongSearchController : Controller
    {

        public ActionResult SongSearch()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchResult(IFormCollection collection)
        {   
            Song song = new Song();
            //captures the data from the form that was imputed by the user.
            string name = collection["name"];
            song = SongDBManager.GetSongByName(name);
            //if song does not match with any song with that name in the database, it will return the song search view else return the search result view.
            if (song == null)
            {
                return View();
            }
            else
            {
                return View(song);
            }



        }

      //method to return the view of the song details
      public ActionResult SongDetails(int id)
        {
            Song song = SongDBManager.GetSongById(id);
            return View(song);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
