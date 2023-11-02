
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ASDAssignmentUTS.Models;
using System.Diagnostics;
using ASDAssignmentUTS.Repositories;
using System.Security.Claims;
namespace ASDAssignmentUTS.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly UserRepository _userRepository;

        public ActionResult PlaylistManagement()
        {
            string? username = HttpContext.Session.GetString("LoggedInUser");

            var name = HttpContext.Session.GetString("LoggedInUser");

            ViewBag.Username = name;


            int? id = PlaylistDBManager.GetIDByUsername(username);

            List<Playlist> playlists = PlaylistDBManager.GetPlaylistsByUserId(id.Value);
            //return the view with the playlist
            return View(playlists);
        }


        /*public ActionResult PlaylistManagement()
        {
            // Get the user username from UserRepository.GetUserIdByUsername() and then pass it to PlaylistDBManager.GetPlaylistsByUserId()
            // to get the playlists for the current user
            var userId = _userRepository.GetUserIdByUsername(User.Identity.Name);
            List<Playlist> playlists = PlaylistDBManager.GetPlaylistsByUserId(userId);
            return View(playlists);
        }*/

        // GET: PlaylistController/AddPlaylist
        // GET: PlaylistController/AddPlaylist
        public ActionResult AddPlaylist()
        {
            // Get the current user's username from the session
            string? username = HttpContext.Session.GetString("LoggedInUser");

            // Get the current user's ID
            int? id = PlaylistDBManager.GetIDByUsername(username);

            // Create a new Playlist object with the OwnerId set to the current user's ID
            var playlist = new Playlist { ownerId = id.Value };

            return View(playlist);
        }
        // POST: PlaylistController/AddPlaylist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPlaylist(Playlist playlist)
        {
            try
            {
                // Get the current session id
                string? username = HttpContext.Session.GetString("LoggedInUser");
                int? id = PlaylistDBManager.GetIDByUsername(username);

                // Set the OwnerId of the new playlist to the current session id
                playlist.ownerId = id.Value;

                // Add the playlist to the database
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

        public IActionResult DeleteSongFromPlaylist()
        {
            //get the playlist id and song id from the query string
            var playlistId = int.Parse(Request.Query["playlistId"]);
            var songId = int.Parse(Request.Query["songId"]);
            PlaylistDBManager.RemoveSongFromPlaylist(playlistId, songId);
            return View();
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