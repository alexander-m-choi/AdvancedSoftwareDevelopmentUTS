using ASDAssignmentUTS.Models;
using System.Data.SqlClient;

namespace ASDAssignmentUTS.Services
{
    //this is a helper class i am going to use for my features.
    public static class SongDBManager
    {
        static readonly string connectionStr = DBConnector.GetConnectionString();

        static public void AddArtist(Artist artist)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"INSERT INTO Artist (id, name, genre, country, description) VALUES ((SELECT COALESCE(MAX(id) + 1, 1) FROM Artist), @name, @genre, @country, @description)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", artist.name);
                cmd.Parameters.AddWithValue("@genre", artist.genre);
                cmd.Parameters.AddWithValue("@country", artist.country);
                cmd.Parameters.AddWithValue("@description", artist.description);
                
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Song> GetSongs()
        {
            List<Song> songs = new List<Song>();
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"SELECT * FROM Song";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Song song = new Song();
                    song.id = Convert.ToInt32(reader["id"]);
                    song.name = reader["name"].ToString();
                    song.artistId = Convert.ToInt32(reader["artist_id"]);
                    song.genre = reader["genre"].ToString();
                    song.description = reader["description"].ToString();
                    songs.Add(song);
                }
            }
            return songs;
        }

        public static void AddSong(Song song)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"INSERT INTO Song (id, name, artist_id, genre, description) VALUES ((SELECT COALESCE(MAX(id) + 1, 1) FROM Song), @name, @artist_id, @genre, @description)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", song.name);
                cmd.Parameters.AddWithValue("@artist_id", song.artistId);
                cmd.Parameters.AddWithValue("@genre", song.genre);
                cmd.Parameters.AddWithValue("@description", song.description);
                
                cmd.ExecuteNonQuery();
            }
        }
    }

}
