using System.ComponentModel.DataAnnotations;

namespace ASDAssignmentUTS.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public String Role { get; set; }

    }
}
