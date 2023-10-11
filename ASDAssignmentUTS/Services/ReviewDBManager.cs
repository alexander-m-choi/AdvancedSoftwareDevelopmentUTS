using ASDAssignmentUTS.Models;
using System.Data.SqlClient;

namespace ASDAssignmentUTS.Services
{
    public class ReviewDBManager
    {
        static readonly string connectionStr = DBConnector.GetConnectionString();

        public static List<Review> GetReviews()
        {
            List<Review> reviews = new List<Review>();
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"SELECT * FROM reviewAlex";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Review review = new Review();
                    review.Review_ID = Convert.ToInt32(reader["Review_ID"]);
                    review.Review_Star = Convert.ToInt32(reader["Review_Star"]);
                    review.Review_Entry = reader["Review_Entry"].ToString();
                    review.User_ID_FK = Convert.ToInt32(reader["User_ID_FK"]);
                    review.Song_ID_FK = Convert.ToInt32(reader["Song_ID_FK"]);
                    reviews.Add(review);
                }
                conn.Close();
            }
            return reviews;
        }

        public static void DBCreateReview(Review review)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"INSERT INTO reviewAlex (Review_ID, Review_Star, Review_Entry, User_ID_FK, Song_ID_FK) 
                      VALUES ((SELECT COALESCE(MAX(Review_ID) + 1, 1) FROM reviewAlex), @Review_Star, @Review_Entry, @User_ID_FK, @Song_ID_FK)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Review_Star", review.Review_Star);
                cmd.Parameters.AddWithValue("@Review_Entry", review.Review_Entry);
                cmd.Parameters.AddWithValue("@User_ID_FK", review.User_ID_FK);
                cmd.Parameters.AddWithValue("@Song_ID_FK", review.Song_ID_FK);

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

        public static List<Review> GetReviewsBySongId(int songId)
        {
            List<Review> reviews = new List<Review>();
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"SELECT * FROM reviewAlex WHERE Song_ID_FK = @Song_ID_FK";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Song_ID_FK", songId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Review review = new Review();
                    review.Review_ID = Convert.ToInt32(reader["Review_ID"]);
                    review.Review_Star = Convert.ToInt32(reader["Review_Star"]);
                    review.Review_Entry = reader["Review_Entry"].ToString();
                    review.User_ID_FK = Convert.ToInt32(reader["User_ID_FK"]);
                    review.Song_ID_FK = Convert.ToInt32(reader["Song_ID_FK"]);
                    reviews.Add(review);
                }
                conn.Close();
            }
            return reviews;
        }
        public static Review GetReviewById(int reviewId)
        {
            Review review = null;
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                conn.Open();
                string sql = @"SELECT * FROM reviewAlex WHERE Review_ID = @Review_ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Review_ID", reviewId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    review = new Review();
                    review.Review_ID = Convert.ToInt32(reader["Review_ID"]);
                    review.Review_Star = Convert.ToInt32(reader["Review_Star"]);
                    review.Review_Entry = reader["Review_Entry"].ToString();
                    review.User_ID_FK = Convert.ToInt32(reader["User_ID_FK"]);
                    review.Song_ID_FK = Convert.ToInt32(reader["Song_ID_FK"]);
                }
                conn.Close();
            }
            return review;
        }

    }
}



