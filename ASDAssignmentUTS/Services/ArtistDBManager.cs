using ASDAssignmentUTS.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace ASDAssignmentUTS.Services;
public static class ArtistDBManager
{
    private static readonly string connectionString = DBConnector.GetConnectionString();

    public static List<Artist> GetArtists()
    {
        string connectionString = DBConnector.GetConnectionString();
        List<Artist> artists = new List<Artist>();
        using (var connection = new SqlConnection(connectionString))
        {
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Artist";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Artist artist = new Artist()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        genre = reader.GetString(2),
                        country = reader.GetString(3),
                        description = reader.GetString(4)
                    };
                    artists.Add(artist);
                }
            }
        }
        return artists;

    }

    public static Artist GetArtistInformation(int artistID)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM Artist WHERE id = @artistID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@artistID", artistID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Artist artist = new Artist
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        genre = (string)reader["genre"],
                        country = (string)reader["country"],
                        description = (string)reader["description"]
                    };

                    return artist;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
