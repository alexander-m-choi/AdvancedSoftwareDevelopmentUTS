using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
                TempData["SongId"] = song.id; // store the song ID in TempData
                return View(song);
            }
            else
            {
                return RedirectToAction("SongSearch", "SongSearch");
            }
        }

        public IActionResult ShowPlaylists()
        {
            string? username = HttpContext.Session.GetString("LoggedInUser");

            if (string.IsNullOrEmpty(username))
            {
                // Handle the case where the user is not logged in
                return RedirectToAction("Login"); // Redirect to the login page or handle it as needed
            }

            ViewBag.Username = username;
            ViewBag.SongId = TempData["SongId"]; // get the song ID from TempData

            int? userId = PlaylistDBManager.GetIDByUsername(username);

            if (userId.HasValue)
            {
                List<Playlist> playlists = PlaylistDBManager.GetPlaylistsByUserId(userId.Value);
                return View(playlists);
            }
            else
            {
                // Handle the case where the user ID couldn't be found
                return RedirectToAction("Error"); // Redirect to an error page or handle it as needed
            }
        }

        [HttpPost]
        public IActionResult AddToPlaylist(int songId, int playlistId)
        {
            string? username = HttpContext.Session.GetString("LoggedInUser");
            if (username != null)
            {
                int? ownerId = PlaylistDBManager.GetIDByUsername(username); // replace GetUserIdByUsername with your actual method
                PlaylistDBManager.AddSongToPlaylist(songId, playlistId); // replace AddSongToPlaylist with your actual method
                return RedirectToAction("SongDetails");
            }
            else
            {
                // handle the case where there is no logged-in user
                return RedirectToAction("Login", "Account"); // redirect to login page if no user is logged in
            }
        }

    }

}




