using System;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Repositories;

namespace ASDAssignmentUTS.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        // Constructor that receives the UserRepository as a dependency
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void AddUser(RegisterModel model)
        {
            // Using the UserRepository to add a user to the database
            _userRepository.AddUser(model);
        }

    public void DeleteUser(int userId)
    {
        try
        {
            // Call the DeleteUser method in the UserRepository to delete the user 
            _userRepository.DeleteUser(userId);
        }
        catch (Exception ex)
        {
            // Handle any exceptions that might occur during deletion
            throw new ApplicationException($"Exception occurred while trying to delete user with ID {userId}.", ex);
        }
    }


    }
}
