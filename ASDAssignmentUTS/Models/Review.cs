using ASDAssignmentUTS.Services;
using System.Data.SqlClient;

namespace ASDAssignmentUTS.Models
{
    public class Review
    {
        public int Review_ID { get; set; }
        public int Review_Star { get; set; }
        public string Review_Entry { get; set; }
        public int User_ID_FK { get; set; }
        public int Song_ID_FK { get; set; }

        public List<Review> GetReview()
        {
            string connectionString = DBConnector.GetConnectionString();
            List<Review> reviews = new List<Review>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM reviewAlex";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Review review = new Review()
                        {
                            Review_ID = reader.GetInt32(0),
                            Review_Star = reader.GetInt32(1),
                            Review_Entry = reader.GetString(2),
                            User_ID_FK = reader.GetInt32(3),
                            Song_ID_FK = reader.GetInt32(4)
                        };
                        reviews.Add(review);
                    }
                }
            }
            return reviews;

        }

        public void CreateReview()
        {
            // Create a new Review object with the desired values
            Review review = new Review()
            {
                Review_ID = 11111111,
                Review_Star = 4,
                Review_Entry = "The song is very nice.",
                User_ID_FK = 12345678,
                Song_ID_FK = 87654321
            };

            // Call the DBCreateReview method from the ReviewDBManager to insert the review into the database
            ReviewDBManager.DBCreateReview(review);
        }

    }
}
