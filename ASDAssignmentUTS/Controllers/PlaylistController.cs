
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ASDAssignmentUTS.Models;

namespace ASDAssignmentUTS.Controllers
{
    public class PlaylistController : Controller
    {

        public ActionResult PlaylistManagement()
        {
            List<Playlist> playlists = PlaylistDBManager.GetPlaylists();
            return View(playlists);
        }
        // GET: PlaylistController/Create
        public ActionResult AddPlaylist()
        {
            return View();
        }

        // POST: PlaylistController/AddPlaylist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPlaylist(Playlist playlist)
        {
            try
            {
                //adds the playlist to the database.
                PlaylistDBManager.AddPlaylist(playlist);
                return RedirectToAction(nameof(PlaylistManagement));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlaylistController/Edit/5
        public ActionResult Update(int id)
        {
            Playlist playlist = PlaylistDBManager.GetPlaylistById(id);
            return View(playlist);
        }

        // POST: PlaylistController/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, IFormCollection collection)
        {
            try
            {
                //creates a new playlist object instance.
                Playlist playlist = new Playlist
                (
                    int.Parse(collection["id"]),
                    collection["name"],
                    collection["description"],
                    int.Parse(collection["ownerId"])
                );
                //updates the playlist in the database.
                PlaylistDBManager.UpdatePlaylist(playlist);
                return RedirectToAction(nameof(PlaylistManagement));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlaylistController/Delete/5

        public ActionResult Delete(int id)
        {
            Playlist playlist = PlaylistDBManager.GetPlaylistById(id);
            return View(playlist);
        }

        // POST: PlaylistController/Delete/5
        [HttpPost]
        public IActionResult DeletePlaylist(Playlist playlist)
        {
            // Delete the playlist with the specified ID from the database
            PlaylistDBManager.DeletePlaylist(playlist.id);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ViewPlaylist(int id)
        {
            Playlist playlist = PlaylistDBManager.GetPlaylistById(id);
            return View(playlist);
        }
    }
}