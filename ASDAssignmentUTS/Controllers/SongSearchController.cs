using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASDAssignmentUTS.Controllers
{
    public class SongSearchController
    {
        public ActionResult SongManagement()
        {
            List<Song> songs = SongDBManager.GetSongs(string songname);
            return View(songs);
        }
    }
}
