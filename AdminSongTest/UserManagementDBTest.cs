using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Services;

namespace AdminSongTest
{
    [TestClass]
    public class UserManagementDBTest
    {
        [TestMethod]
        public void TestAddUser()
        {
            var user = new User();
            user.username = "erinTest";
            user.password = "test";
            user.email = "test@testable.com";
            //adds the user to the database
            UserDBManager.AddUser(user);
            //performs a check to see if the user is in the database
            Assert.AreEqual("erinTest", UserDBManager.GetUserByUsername("erinTest").username);
        }

        [TestMethod]
        public void TestDeleteUser() 
        {
            User user = UserDBManager.GetUserByUsername("erinTest");
            UserDBManager.DeleteUser(user.id);
            Assert.ThrowsException<UserNotFoundException>(() => UserDBManager.GetUserByUsername("erinTest"));
        }
    }
}
