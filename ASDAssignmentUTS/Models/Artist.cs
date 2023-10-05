using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ASDAssignmentUTS.Models;

    public class Artist
    {
        public int id { get; set; }
        public string name { get; set; }
        public string genre { get; set; }
        public string country { get; set; }
        public string description { get; set; }

            public List<Artist> GetArtists()
            {
                string connectionString = "Data Source=asdassignmentuts.database.windows.net;Initial Catalog=SongManagementSystem;Persist Security Info=True;User ID=azureuser;Password=ASDAssignment1";
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
        }