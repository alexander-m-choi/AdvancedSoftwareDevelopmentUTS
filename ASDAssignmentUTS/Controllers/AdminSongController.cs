using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASDAssignmentUTS.Controllers
{
    public class AdminSongController : Controller
    {
        // GET: AdminController
        public ActionResult SongManagement(int? id)
        {
            List<Song> songs;
            if (id != null)
            {
                songs = SongDBManager.GetSongsByArtist(id ?? 0);
                ViewBag.ArtistId = id;
                return View(songs);
            }
            else
            {
                songs = SongDBManager.GetSongs();
            }
             
            return View(songs);
        }

        public ActionResult SongManagementByArtist(int id)
        {
            List<Song> songs = SongDBManager.GetSongsByArtist(id);
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
       
        //adds a song that is from the artist that is selected.
        public ActionResult AddSong(int? id)
        {
            var artists = new Artist().GetArtists();
            ViewBag.Artists = artists;
            ViewBag.ArtistId = id;
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
        public ActionResult UpdateArtist(int id)
        {
            var artist = SongDBManager.GetArtistById(id);
            return View(artist);
        }
        [HttpGet]
        public ActionResult UpdateSong(int id)
        {
            var song = SongDBManager.GetSongById(id);
            Artist artist = new Artist();
            var allArtist = artist.GetArtists();
            //this will list all the artists in the view bag.
            ViewBag.Artists = allArtist;
            ViewBag.artistId = song.artistId;
            return View(song);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSong(int id, IFormCollection collection)
        {
            try
            {
                var song = new Song();
                //captures the data from the form that was imputed by the user.
                song.id = id;
                song.name = collection["name"];
                song.artistId = Convert.ToInt32(collection["artistId"]);
                song.genre = collection["genre"];
                song.description = collection["description"];
                SongDBManager.UpdateSong(song);
                return RedirectToAction(nameof(SongManagement));
            }
            catch
            {
                return View();
            }
        }
        // POST: AdminController/UpdateArtist/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateArtist(int id, IFormCollection collection)
        {
            try
            {
                var artist = new Artist();
                //captures the data from the form that was imputed by the user.
                artist.id = id;
                artist.name = collection["name"];
                artist.genre = collection["genre"];
                artist.country = collection["country"];
                artist.description = collection["description"];
                SongDBManager.UpdateArtist(artist);
                return RedirectToAction(nameof(ArtistManagement));
            }
            catch
            {
                return View();
            }
        }

        // POST: AdminController/Delete/5
        [HttpPost]
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
        
        public ActionResult DeleteSong(IFormCollection collection)
        {
            try
            {
                SongDBManager.DeleteSong(Convert.ToInt32(collection["id"]));
                return RedirectToAction(nameof(SongManagement));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SongsByArtist(int id)
        {
            List<Song> songs = SongDBManager.GetSongsByArtist(id);
            ViewBag.ArtistId = id;
            return View(songs);
        }
    }
}
