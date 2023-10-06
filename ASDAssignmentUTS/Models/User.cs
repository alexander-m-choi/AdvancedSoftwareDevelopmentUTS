using ASDAssignmentUTS.Services;

namespace ASDAssignmentUTS.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        //this is used to add to the database.
        public User(string username, string password, string email)
        {
            this.username = username;
            this.password = password;
            this.email = email;
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
