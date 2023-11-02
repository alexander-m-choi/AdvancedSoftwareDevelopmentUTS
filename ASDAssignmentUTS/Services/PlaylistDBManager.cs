using ASDAssignmentUTS.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.Identity.Client;

namespace ASDAssignmentUTS.Services;
public static class PlaylistDBManager
{
    private static readonly string connectionString = DBConnector.GetConnectionString();

    public static List<Playlist> GetPlaylists()
    {
        List<Playlist> playlists = new List<Playlist>();
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Playlist";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Playlist playlist = new Playlist
                    (
                        reader.GetInt32(0), // id
                        reader.GetString(1), // name
                        reader.GetString(2), // description
                        reader.GetInt32(3) // ownerId
                    );
                    playlists.Add(playlist);
                }
            }
        }
        return playlists;
    }

    public static List<Playlist> GetPlaylistsByUserId(int? userId)
    {
        List<Playlist> playlists = new List<Playlist>();
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Playlist WHERE ownerId = @userId";
                command.Parameters.AddWithValue("@userId", userId);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Playlist playlist = new Playlist
                    (
                        reader.GetInt32(0), // id
                        reader.GetString(1), // name
                        reader.GetString(2), // description
                        reader.GetInt32(3) // ownerId
                    );
                    playlists.Add(playlist);
                }
            }
        }
        return playlists;
    }

    //I want to pass the username in first from the session, to get the ID of the user. Then we can pass that ID into the GetPlaylistsByUserId method
    public static int? GetIDByUsername(string username)
    {
        int? userId = null;
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT ID FROM RowanUsers WHERE username = @username";
                command.Parameters.AddWithValue("@username", username);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    userId = reader.GetInt32(0);
                }
            }
        }
        return userId;
    }



    public static List<Playlist> GetPlaylistsByUserId(int userId)
    {
        List<Playlist> playlists = new List<Playlist>();
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Playlist WHERE ownerId = @ownerId";
                command.Parameters.AddWithValue("@ownerId", userId);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Playlist playlist = new Playlist
                    (
                        reader.GetInt32(0), // id
                        reader.GetString(1), // name
                        reader.GetString(2), // description
                        reader.GetInt32(3) // ownerId
                    );
                    playlists.Add(playlist);
                }
            }
        }
        return playlists;
    }
    //add playlist

    public static void AddPlaylist(Playlist playlist)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Playlist (ID, name, description, ownerId) VALUES (@ID, @name, @description, @ownerId)";
            SqlCommand command = new SqlCommand(query, connection);

            // Retrieve the last ID value from the Playlist table and increment it by one.
            int lastId = 0;
            string getLastIdQuery = "SELECT TOP 1 ID FROM Playlist ORDER BY ID DESC";
            using (SqlCommand getLastIdCommand = new SqlCommand(getLastIdQuery, connection))
            {
                connection.Open();
                var reader = getLastIdCommand.ExecuteReader();
                if (reader.Read())
                {
                    lastId = reader.GetInt32(0);
                }
                connection.Close();
            }
            int newId = lastId + 1;

            command.Parameters.AddWithValue("@ID", newId);
            command.Parameters.AddWithValue("@name", playlist.name);
            command.Parameters.AddWithValue("@description", playlist.description);
            command.Parameters.AddWithValue("@ownerId", playlist.ownerId);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // Log or display the exception details for debugging.
                Console.WriteLine("SQL Exception: " + ex.Message);
                Console.WriteLine("Error Number: " + ex.Number);
                // Additional error handling as needed.
                throw; // Rethrow the exception to handle it in the controller if necessary.
            }
        }
    }


    //delete playlist

    public static void DeletePlaylist(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM Playlist WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }
    }

    //update playlist

    public static void UpdatePlaylist(Playlist playlist)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE Playlist SET name = @name, description = @description, ownerId = @ownerId WHERE id = @id";
                command.Parameters.AddWithValue("@id", playlist.id);
                command.Parameters.AddWithValue("@name", playlist.name);
                command.Parameters.AddWithValue("@description", playlist.description);
                command.Parameters.AddWithValue("@ownerId", playlist.ownerId);
                command.ExecuteNonQuery();
            }
        }
    }

    //get playlist by id

    public static Playlist GetPlaylistById(int id)
    {
        var playlist = new Playlist();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT name, description, ownerId FROM Playlist WHERE id = @playlistId";
                command.Parameters.AddWithValue("@playlistId", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        playlist.id = id;
                        playlist.name = reader.GetString(0);
                        playlist.description = reader.GetString(1);
                        playlist.ownerId = reader.GetInt32(2);
                    }
                }
            }

            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT SongToPlaylist.songID, Song.name, Song.artist_id FROM SongToPlaylist JOIN Song ON SongToPlaylist.songID = Song.id WHERE SongToPlaylist.playlistID = @playlistId";
                command.Parameters.AddWithValue("@playlistId", id);

                using (var reader = command.ExecuteReader())
                {
                    playlist.Songs = new List<Song>();
                    while (reader.Read())
                    {
                        var song = new Song();
                        song.id = reader.GetInt32(0);
                        song.name = reader.GetString(1);
                        song.artistId = reader.GetInt32(2);
                        playlist.Songs.Add(song);
                    }
                }
            }
        }
        return playlist;
    }
    public static void RemoveSongFromPlaylist(int playlistID, int songID)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "DELETE FROM SongToPlaylist WHERE playlistID = @playlistID AND songID = @songID";
                command.Parameters.AddWithValue("@playlistID", playlistID);
                command.Parameters.AddWithValue("@songID", songID);
                command.ExecuteNonQuery();
            }
        }
    }
}