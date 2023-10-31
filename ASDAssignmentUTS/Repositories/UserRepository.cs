    using System;
    using ASDAssignmentUTS.Models;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Configuration;



    namespace ASDAssignmentUTS.Repositories
    {
        public class UserRepository
        {
            private readonly string _connectionString;

            public UserRepository(string connectionString)
            {
                _connectionString = connectionString;
            }

    public void AddUser(RegisterModel model)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO RowanUsers (id, username, password, email, role) VALUES ((SELECT COALESCE(MAX(id), 0) + 1 FROM RowanUsers), @username, @password, @email, @role)";
                command.Parameters.AddWithValue("@username", model.Username);
                command.Parameters.AddWithValue("@password", model.Password); // Ensure password is securely hashed
                command.Parameters.AddWithValue("@email", model.Email);
                command.Parameters.AddWithValue("@role", model.Role);
                command.ExecuteNonQuery();
            }
        }
            }

    public bool UsernameExists(string username)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(*) FROM RowanUsers WHERE username = @username";
                command.Parameters.AddWithValue("@username", username);
                
                int count = (int)command.ExecuteScalar();
                
                return count > 0; // Return true if username exists, otherwise false
            }
        }
    }

    public bool EmailExists(string email)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(*) FROM RowanUsers WHERE email = @email";
                command.Parameters.AddWithValue("@email", email);
                
                int count = (int)command.ExecuteScalar();
                
                return count > 0; // Return true if email exists, otherwise false
            }
        }
    }



            public bool ValidateUser(LoginModel model)
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                // Using parameterized query to prevent SQL injection
                string sql = "SELECT COUNT(*) FROM DBO.RowanUsers WHERE Username=@Username AND Password=@Password";
                using SqlCommand command = new SqlCommand(sql, connection);
        
                // Adding parameters with their values
                command.Parameters.AddWithValue("@Username", model.Username);
                command.Parameters.AddWithValue("@Password", model.Password);
        
                int count = (int)command.ExecuteScalar();
                return count > 0; // If count is more than zero, it means a user with given credentials exists.
            }

            public void DeleteUser(int userId)
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                // Using parameterized query to prevent SQL injection
                string sql = "DELETE FROM DBO.RowanUsers WHERE id=@UserId";
                using SqlCommand command = new SqlCommand(sql, connection);
        
                // Adding parameter with its value
                command.Parameters.AddWithValue("@UserId", userId);
        
                command.ExecuteNonQuery();
            }   

            public int? GetUserIdByUsername(string username)
            {
            using (var connection = new SqlConnection(_connectionString))
            {
            connection.Open();
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id FROM RowanUsers WHERE username = @username";
                command.Parameters.AddWithValue("@username", username);
                var result = command.ExecuteScalar();
                return result != null ? (int?)result : null;
            }
            }
            }
            
            // Method to get the user role by username
            public string GetUserRoleByUsername(string username)
            {
            using (var connection = new SqlConnection(_connectionString))
            {
            connection.Open();
            using (var command = new SqlCommand())
            {
            command.Connection = connection;
            command.CommandText = "SELECT role FROM RowanUsers WHERE username = @username";
            command.Parameters.AddWithValue("@username", username);

            var result = command.ExecuteScalar();
            return result != DBNull.Value ? ((string)result).Trim() : null;
            }
        }
    }

public string GetCurrentPassword(string username)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        connection.Open();
        using (var command = new SqlCommand("SELECT Password FROM RowanUsers WHERE Username = @Username", connection))
        {
            command.Parameters.AddWithValue("@Username", username);
            return (string)command.ExecuteScalar();
        }
    }
}


public void UpdatePassword(string username, string newPassword)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        connection.Open();
        using (var command = new SqlCommand("UPDATE RowanUsers SET Password = @NewPassword WHERE Username = @Username", connection))
        {
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@NewPassword", newPassword);
            command.ExecuteNonQuery();
        }
    }
}


            // Implement other methods to interact with the User table as needed
        }
    }
