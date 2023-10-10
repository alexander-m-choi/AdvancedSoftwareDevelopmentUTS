using ASDAssignmentUTS;
using ASDAssignmentUTS.Controllers;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdminSongTest
{
    [TestClass]
    public class SongManagementDBTest
    {
        [TestMethod]
        public void TestAddArtist()
        {
            
            Artist artist = new Artist();
            artist.name = "AbiDonut";
            artist.country = "Australia";
            artist.genre = "Rock";
            artist.description = "Abbey works as a support worker but in her spare time, she likes to produce Rock and Roll music.";
            //adds it directly to the database
            SongDBManager.AddArtist(artist);
            Artist artistFromDB = SongDBManager.GetArtistByName("AbiDonut");
            //performs the test cases to see if the artist is added to the database from the controller.
            Assert.AreEqual("AbiDonut", artistFromDB.name);
            Assert.AreEqual("Australia", artistFromDB.country);
            Assert.AreEqual("Rock", artistFromDB.genre);
            Assert.AreEqual("Abbey works as a support worker but in her spare time, she likes to produce Rock and Roll music.", artistFromDB.description);
        }
        [TestMethod]
        public void TestAddSong()
        {
            //gets the artist object so it can be used to add a song to the database related to the artist id.
            Artist artist = SongDBManager.GetArtistByName("AbiDonut");
            Song song = new Song();
            song.name = "What Ability Camp Song";
            song.artistId = artist.id;
            song.genre = "Rock";
            song.description = "This song is about the Ability Camp and how it is a great place to be.";
            SongDBManager.AddSong(song);
            Song songFromDB = SongDBManager.GetSongByNameAndArtist("What Ability Camp Song", artist.id);
            Assert.AreEqual("What Ability Camp Song", songFromDB.name);
            Assert.AreEqual("Rock", songFromDB.genre);
            Assert.AreEqual("This song is about the Ability Camp and how it is a great place to be.", songFromDB.description);
        }

        [TestMethod]
        //[ExpectedException(typeof(SongNotFoundException))] //this is the expected exception that will be thrown if the song is not found to verify if the song is deleted
        public void TestDeleteSong()
        {
            //this will add a new artist to specifically test deleting a song.
            Artist artist = new Artist();
            artist.name = "Samantha Stars";
            artist.country = "Australia";
            artist.genre = "Pop";
            artist.description = "Samantha is a pop singer who likes to sing about her life experiences.";
            SongDBManager.AddArtist(artist);
            //gets the artist object so it can be used to add a song to the database related to the artist id.
            Artist fetchedArtist = SongDBManager.GetArtistByName("Samantha Stars");
            Song newSong = new Song();
            newSong.name = "I love the moon";
            newSong.artistId = fetchedArtist.id;
            newSong.genre = "Pop";
            newSong.description = "This song is about how much Samantha loves the moon.";
            SongDBManager.AddSong(newSong);
            
            Song song = SongDBManager.GetSongByName("I love the moon");
            SongDBManager.DeleteSong(song.id);
            var deletedSong = () => SongDBManager.GetSongByName("I love the moon");
            Assert.ThrowsException<SongNotFoundException>(deletedSong);
            //delete the artist that was added for this test case.
            SongDBManager.DeleteArtist(fetchedArtist.id);
        }

        [TestMethod]
        public void TestDeleteArtist()
        {
            Artist artist = SongDBManager.GetArtistByName("AbiDonut");
            SongDBManager.DeleteArtist(artist.id);

            //verifys if the artist is deleted.
            var deletedArttst = () => SongDBManager.GetArtistByName("AbiDonut");

            Assert.ThrowsException<ArtistNotFoundException>(deletedArttst);
        }
    }
}