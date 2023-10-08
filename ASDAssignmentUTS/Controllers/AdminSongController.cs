using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASDAssignmentUTS.Controllers
{
    public class AdminSongController : Controller
    {
        // GET: AdminController
        public ActionResult SongManagement()
        {
            List<Song> songs = SongDBManager.GetSongs();
            return View(songs);
        }

        public ActionResult ArtistManagement()
        {
            var artists = new Artist().GetArtists();
            return View(artists);
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/AddArtist
        public ActionResult AddArtist()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddArtist(IFormCollection collection)
        {
            try
            {
                var artist = new Artist();
                //captures the data from the form that was imputed by the user.
                artist.name = collection["name"];
                artist.genre = collection["genre"];
                artist.country = collection["country"];
                artist.description = collection["description"];
                SongDBManager.AddArtist(artist);
                return RedirectToAction(nameof(ArtistManagement));
            }
            catch
            {
                return View();
            }
        }
        //GET: AdminController/AddSong
        public ActionResult AddSong()
        {
            var artists = new Artist().GetArtists();
            ViewBag.Artists = artists;

            return View();
        }
        // POST: AdminController/AddSong
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSong(IFormCollection collection)
        {
            try
            {
                var song = new Song();
                //captures the data from the form that was imputed by the user.
                song.name = collection["name"];
                song.artistId = Convert.ToInt32(collection["artistId"]);
                song.genre = collection["genre"];
                song.description = collection["description"];
                SongDBManager.AddSong(song);
                return RedirectToAction(nameof(SongManagement));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult DeleteSong(int id)
        {
            return View();
        }

        public ActionResult DeleteArtist(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteArtist(int id, IFormCollection collection)
        {
            try
            {   SongDBManager.DeleteArtist(id);
                return RedirectToAction(nameof(ArtistManagement));
            }
            catch
            {
                return View();
            }
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSong(int id, IFormCollection collection)
        {
            try
            {
                SongDBManager.DeleteSong(id);
                return RedirectToAction(nameof(SongManagement));
            }
            catch
            {
                return View();
            }
        }
    }
}
