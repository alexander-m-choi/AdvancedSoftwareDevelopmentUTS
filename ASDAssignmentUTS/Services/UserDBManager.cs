﻿using ASDAssignmentUTS.Models;
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
                    command.CommandText = @"UPDATE RowanUsers SET username = @username, password = @password, email = @email";
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
    }
}