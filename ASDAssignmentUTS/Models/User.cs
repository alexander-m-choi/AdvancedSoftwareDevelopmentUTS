﻿using ASDAssignmentUTS.Services;
using System.ComponentModel.DataAnnotations;

namespace ASDAssignmentUTS.Models
{

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? confirmPassword { get; set; }

        [EmailAddress]
        public string email { get; set; }


        //this is used to add to the database.
        public User(string username, string password, string email)
        {
            this.username = username;
            this.password = password;
            this.email = email;
        }

        public User()
        {

        }

        public User(int id, string username, string password, string email)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.email = email;
        }

        public List<User> GetUsers()
        {
            return UserDBManager.GetUsers();
        }
    }

   
}
