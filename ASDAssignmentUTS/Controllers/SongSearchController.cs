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
                

            return View(song);
        }
    }
}
