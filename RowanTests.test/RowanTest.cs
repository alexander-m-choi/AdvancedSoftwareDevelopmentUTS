using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;
using System.Data.SqlClient;

namespace RowanTests.test
{
    [TestClass]
    public class RowanTest
    {
        private string connectionString = DBConnector.GetConnectionString();

        [TestMethod]
        public void TestAddPlaylist()
        {
            // Arrange
            int maxId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT MAX(id) FROM Playlist", connection))
                {
                    maxId = (int)command.ExecuteScalar();
                }
            }
            Playlist playlistToAdd = new Playlist
            {
                id = maxId + 1,
                name = "Test Playlist",
                description = "Test Playlist Description",
                ownerId = 1
            };

            // Act
            PlaylistDBManager.AddPlaylist(playlistToAdd);

            // Assert
            Playlist playlistFromDb = PlaylistDBManager.GetPlaylistById(playlistToAdd.id);
            Assert.IsNotNull(playlistFromDb);
            Assert.AreEqual(playlistToAdd.name, playlistFromDb.name);
            Assert.AreEqual(playlistToAdd.description, playlistFromDb.description);
            Assert.AreEqual(playlistToAdd.ownerId, playlistFromDb.ownerId);
        }
    }
}