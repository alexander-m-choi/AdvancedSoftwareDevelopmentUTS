using ASDAssignmentUTS.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ASDAssignmentUTS.Services
{
    public static class UserDBManager
    {
        private static readonly string connectionString = DBConnector.GetConnectionString();

        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM RowanUsers";
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        User user = new User
                        (
                            reader.GetInt32(0), // id
                            reader.GetString(1), // username
                            reader.GetString(2), // password
                            reader.GetString(3) // email
                        );
                        users.Add(user);
                    }
                }
            }
            return users;
        }

        public static void AddUser(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.CommandText = "INSERT INTO RowanUsers VALUES ((SELECT COALESCE(MAX(id), + 1, 1) FROM RowanUsers), @username, @password, @email)";
                    command.Parameters.AddWithValue("@username", user.username);
                    command.Parameters.AddWithValue("@password", user.password);
                    command.Parameters.AddWithValue("@email", user.email);
                    command.ExecuteNonQuery();
                };
            }
        }
    }
}
