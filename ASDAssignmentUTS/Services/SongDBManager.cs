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
        public static string? GetArtistName(int id)
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
            catch (SqlException e)
            {
                throw new QueryErrorException(e.Message);
            }
            return artistName;
        }

        public static void DeleteSong(int id)
        {
            try
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
            catch (SqlException e)
            {
                throw new QueryErrorException(e.Message);
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
            try
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
            catch (SqlException e)
            {
                throw new QueryErrorException(e.Message);
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
            catch (SqlException e)
            {
                throw new QueryErrorException(e.Message);
            }
            catch
            {
                //an exception is thrown if the song is not found.
                throw new SongNotFoundException();
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
                    if (artist.id != id)
                    {
                        throw new ArtistNotFoundException();
                    }
                    return artist;
                }
            }
            catch (SqlException e)
            {
                throw new QueryErrorException(e.Message);
            }
            catch
            {
                throw new ArtistNotFoundException();
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

        //used for unit testing
        public static Artist GetArtistByName(string name)
        {
            try
            {
                Artist artist = new Artist();
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    string sql = @"SELECT * FROM Artist WHERE name = @name";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        artist.id = Convert.ToInt32(reader["id"]);
                        artist.name = reader["name"].ToString();
                        artist.genre = reader["genre"].ToString();
                        artist.country = reader["country"].ToString();
                        artist.description = reader["description"].ToString();
                        //this will ensure that an exception is thrown if the artist is not found so it can be picked up during unit testing..

                    }
                    if (artist.name == "" || artist.name == null || artist.name != name)
                    {
                        throw new ArtistNotFoundException();
                    }
                    conn.Close();

                }

                return artist;
            }
            //catches the exception if there is something wrong with the SQL server.
            catch (SqlException e)
            {
                throw new QueryErrorException(e.Message);
            }
            catch (ArtistNotFoundException)
            {
                throw new ArtistNotFoundException();
            }
        }
        //used for unit testing to see if the song is added.
        public static Song GetSongByNameAndArtist(string name, int artistId)
        {
            try
            {
                Song song = new Song();
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    string sql = @"SELECT * FROM Song WHERE name = @name AND artist_id = @artist_id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@artist_id", artistId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        song.id = Convert.ToInt32(reader["id"]);
                        song.name = reader["name"].ToString();
                        song.artistId = Convert.ToInt32(reader["artist_id"]);
                        song.genre = reader["genre"].ToString();
                        song.description = reader["description"].ToString();

                    }
                    if (song.name == "" || song.name == null || song.name != name)
                    {
                        throw new SongNotFoundException();
                    }
                    conn.Close();
                }

                return song;
            }
            catch (SqlException e)
            {
                throw new QueryErrorException(e.Message);
            }
            catch (SongNotFoundException)
            {
                throw new SongNotFoundException();
            }
        }

        static public Song GetSongByName(string name)
        {
            try
            {
                Song song = new Song();
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    string sql = @"SELECT * FROM Song WHERE name = @name";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        song.id = Convert.ToInt32(reader["id"]);
                        song.name = reader["name"].ToString();
                        song.artistId = Convert.ToInt32(reader["artist_id"]);
                        song.genre = reader["genre"].ToString();
                        song.description = reader["description"].ToString();

                    }
                    if (song.name == "" || song.name == null || song.name != name)
                    {
                        throw new SongNotFoundException();
                    }
                    conn.Close();

                }
                return song;

            }
            catch (SqlException e)
            {
                throw new QueryErrorException(e.Message);
            }
            catch (SongNotFoundException)
            {
                throw new SongNotFoundException();
            }
        }

        //this is used for unit testing purposes
        public static void DeleteArtistByName(string name)
        {
            try
            {
                Artist artist = new Artist();
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    string sql = @"DELETE FROM Artist WHERE name = @name";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException)
            {
                //this will not do anything if there is an exception.
                return;
            }
        }

    }    
    

    //this is a class to throw an exception if the song or artist is not found.
    public class SongNotFoundException : Exception
    {
        public SongNotFoundException() : base ()
        {
        }
    }
    public class ArtistNotFoundException : Exception
    {
        public ArtistNotFoundException() : base()
        {
        }
    }
    //due to SQL exception is sealed, i have to create a new exception class to throw an exception related to SQL Server.
    public class QueryErrorException : Exception
    {
        public QueryErrorException(string message) : base(message)
        {
        }
    }
}
