using ASDAssignmentUTS.Models;
using System.Data.SqlClient;

namespace ASDAssignmentUTS.Services
{
    //this is a helper class i am going to use for my features.
    public static class DBManager
    {
        static readonly string connectionStr = DBConnector.GetConnectionString();

        static public void AddArtist(Artist artist)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"INSERT INTO Artist (name, genre, country, description) VALUES (@name, @genre, @country, @description)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", artist.name);
                cmd.Parameters.AddWithValue("@genre", artist.genre);
                cmd.Parameters.AddWithValue("@country", artist.country);
                cmd.Parameters.AddWithValue("@description", artist.description);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
