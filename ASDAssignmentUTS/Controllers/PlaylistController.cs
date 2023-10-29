
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ASDAssignmentUTS.Models;
using System.Diagnostics;

namespace ASDAssignmentUTS.Controllers
{
    public class PlaylistController : Controller
    {

        //Method to get the playlist for the logged in user
        //which we will determine by pulling the userid from the session
        //and then using that to get the playlist from the database
        public ActionResult PlaylistManagement()
        {
            //get the user id from the session
            int? userId = HttpContext.Session.GetInt32("LoggedInUser");
            //get the playlist from the database
            List<Playlist> playlists = PlaylistDBManager.GetPlaylistsByUserId(userId);
            //return the view with the playlist
            return View(playlists);
        }

        /*public ActionResult PlaylistManagement()
        {
            List<Playlist> playlists = PlaylistDBManager.GetPlaylists();
            return View(playlists);
        }
        // GET: PlaylistController/Create
        public ActionResult AddPlaylist()
        {
            return View();
        }*/

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
        public IActionResult UpdatePlaylist(int id)
        {
            Playlist playlist = PlaylistDBManager.GetPlaylistById(id);
            return View(playlist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePlaylist(int id, Playlist playlist)
        {
            try
            {
                PlaylistDBManager.UpdatePlaylist(playlist);
                return RedirectToAction("ViewPlaylist", new { id = playlist.id });
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

        public IActionResult ViewPlaylist(int id)
        {
            var playlist = PlaylistDBManager.GetPlaylistById(id);
            return View(playlist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSongFromPlaylist(int playlistId, int songId)
        {
            try
            {
                Debug.WriteLine($"Deleting song {songId} from playlist {playlistId}.");
                PlaylistDBManager.RemoveSongFromPlaylist(playlistId, songId);
                return RedirectToAction("ViewPlaylist", new { id = playlistId });
            }
            catch
            {
                return View();
            }
        }
    }
}