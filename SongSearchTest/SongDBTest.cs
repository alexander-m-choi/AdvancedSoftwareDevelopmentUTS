using ASDAssignmentUTS;
using ASDAssignmentUTS.Controllers;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SongSearchTest
{
    [TestClass]
    public class SongDBTest
    {
        [TestMethod]
        public void TestSongSearch()
        {
           /* Song song = new Song();
            song.name = "Fire";
            song.artistId = 1;
            song.genre = "Rock";
            song.description = "The latest music from Imagine Vampires.";
            //adds it directly to the database
            SongDBManager.AddSong(song);
            Song testsong = SongDBManager.GetSongByName("Fire");
            Assert.AreEqual("Fire", song.name);
            Assert.AreEqual("Rock", song.genre);
            Assert.AreEqual("The latest music from Imagine Vampires.", song.description);
            SongDBManager.DeleteSong(testsong.id);*/
        }
    }
}