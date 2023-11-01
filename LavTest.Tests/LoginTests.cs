using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using ASDAssignmentUTS.Controllers;
using ASDAssignmentUTS.Models;
using ASDAssignmentUTS.Repositories;

[TestClass]
public class LoginTests
{
    [TestMethod]
    public void Login_ValidUser_ReturnsRedirectToHomeIndex()
    {
        // Arrange
        var mockRepo = new Mock<UserRepository>();
        mockRepo.Setup(repo => repo.ValidateUser(It.IsAny<LoginModel>())).Returns(true);
        var controller = new AccountController(mockRepo.Object);

        var loginModel = new LoginModel 
        {
            Username = "User123",
            Password = "User123"
        };

        // Act
        var result = controller.Login(loginModel) as RedirectToActionResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Index", result.ActionName);
        Assert.AreEqual("Home", result.ControllerName);
    }
}
