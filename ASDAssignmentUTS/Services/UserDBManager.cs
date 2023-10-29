using ASDAssignmentUTS.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;

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
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO RowanUsers (id, username, password, email) VALUES ((SELECT COALESCE(MAX(id), 0) + 1 FROM RowanUsers), @username, @password, @email)";
                    command.Parameters.AddWithValue("@username", user.username);
                    command.Parameters.AddWithValue("@password", user.password);
                    command.Parameters.AddWithValue("@email", user.email);
                    command.ExecuteNonQuery();
                };
            }
        }

        public static void UpdateUser(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"UPDATE RowanUsers SET username = @username, password = @password, email = @email WHERE id = @id";
                    command.Parameters.AddWithValue("@id", user.id);
                    command.Parameters.AddWithValue("@username", user.username);
                    command.Parameters.AddWithValue("@password", user.password);
                    command.Parameters.AddWithValue("@email", user.email);
                    command.ExecuteNonQuery();
                };
            }
        }
        public static void DeleteUser(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"DELETE FROM RowanUsers WHERE id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                };
            }
        }

        public static User GetUserById(int id)
        {
            try
            {
                User user = new User();
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = @"SELECT * FROM RowanUsers WHERE id = @id";
                        command.Parameters.AddWithValue("@id", id);
                        var reader = command.ExecuteReader();
                        while(reader.Read())
                        {
                            
                            user.id = reader.GetInt32(0);
                            user.username = reader.GetString(1);
                            user.password = reader.GetString(2);
                            user.email = reader.GetString(3);
                            
                        }
                    }
                }
                return user;
            }
            catch
            {
                throw new Exception("User not found");
            }
        }

        public static User GetUserByUsername(string username)
        {
            try
            {
                User user = new User();
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = @"SELECT * FROM RowanUsers WHERE username = @username";
                        command.Parameters.AddWithValue("@username", username);
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            user.id = reader.GetInt32(0);
                            user.username = reader.GetString(1);
                            user.password = reader.GetString(2);
                            user.email = reader.GetString(3);
                        }

                        if(user.username == null || user.username == "" || user.username != username)
                        {
                            throw new UserNotFoundException();
                        }
                        
                    }
                    connection.Close();
                    return user;
                }
            }
            catch(SqlException e)
            {
                throw new QueryErrorException(e.Message);
            }
            catch
            {
                throw new UserNotFoundException();
            }
        }

        //this will be use to reset the users password from the admin side and generate a temporary password for the user.
        public static void ResetPassword(int id, string newPassword)
        {
            try
            {
                User user = new User();
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = @"UPDATE RowanUsers SET password = @password WHERE id = @id";
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@password", newPassword);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                throw new Exception("User not found");
            }
        }

        //this will be used to get the user's password from the database after its being regenerated for the user upon reset password.
        public static string GetPassword(int id)
        {
            try
            {
                User user = new User();
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = @"SELECT password FROM RowanUsers WHERE id = @id";
                        command.Parameters.AddWithValue("@id", id);
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            user.password = reader.GetString(0);
                        }
                    }
                }
                return user.password;
            }
            catch
            {
                throw new Exception("User not found");
            }
        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base() { }
        public UserNotFoundException(string message) : base(message) { }
        public UserNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    }
}
