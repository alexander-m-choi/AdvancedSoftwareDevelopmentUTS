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
                conn.Close();
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
                conn.Close();
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
                conn.Close();
                
            }
        }

        //returns the Artist Name
        public static string GetArtistName(int id)
        {
            string artistName = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    string sql = @"SELECT name FROM Artist WHERE id = @id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        artistName = reader["name"].ToString();
                    }
                    conn.Close();
                }
            }
            catch (Exception e) //this is error handling in case if the artist has been deleted.
            {
                artistName = "";
            }
            
            return artistName;
        }

        public static void DeleteSong(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"DELETE FROM Song WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        
        public static List<Song> GetSongsByArtist(int artistId)
        {
            List<Song> songs = new List<Song>();
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"SELECT * FROM Song WHERE artist_id = @artist_id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@artist_id", artistId);
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
                conn.Close();
            }
            return songs;
        }

        //deletes all the songs that are associated with the artist.
        public static void DeleteSongsByArtist(int artistId)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"DELETE FROM Song WHERE artist_id = @artist_id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@artist_id", artistId);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static void DeleteArtist(int id)
        {
            //deletes the songs that is associated with the artist.
            DeleteSongsByArtist(id);
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"DELETE FROM Artist WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static Song GetSongById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    string sql = @"SELECT * FROM Song WHERE id = @id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Song song = new Song();
                    while (reader.Read())
                    {
                        song.id = Convert.ToInt32(reader["id"]);
                        song.name = reader["name"].ToString();
                        song.artistId = Convert.ToInt32(reader["artist_id"]);
                        song.genre = reader["genre"].ToString();
                        song.description = reader["description"].ToString();
                    }
                    conn.Close();
                    return song;
                }
            }
            catch 
            {
                throw new Exception("Song not found");
            }
        }

        public static Artist GetArtistById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    string sql = @"SELECT * FROM Artist WHERE id = @id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Artist artist = new Artist();
                    while (reader.Read())
                    {
                        artist.id = Convert.ToInt32(reader["id"]);
                        artist.name = reader["name"].ToString();
                        artist.genre = reader["genre"].ToString();
                        artist.country = reader["country"].ToString();
                        artist.description = reader["description"].ToString();
                    }
                    conn.Close();
                    return artist;
                }
            }
            catch
            {
                throw new Exception("Artist not found");
            }
        }

        public static void UpdateSong(Song song)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"UPDATE Song SET name = @name, artist_id = @artist_id, genre = @genre, description = @description WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", song.name);
                cmd.Parameters.AddWithValue("@artist_id", song.artistId);
                cmd.Parameters.AddWithValue("@genre", song.genre);
                cmd.Parameters.AddWithValue("@description", song.description);
                cmd.Parameters.AddWithValue("@id", song.id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static void UpdateArtist(Artist artist)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"UPDATE Artist SET name = @name, genre = @genre, country = @country, description = @description WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", artist.name);
                cmd.Parameters.AddWithValue("@genre", artist.genre);
                cmd.Parameters.AddWithValue("@country", artist.country);
                cmd.Parameters.AddWithValue("@description", artist.description);
                cmd.Parameters.AddWithValue("@id", artist.id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }

}
