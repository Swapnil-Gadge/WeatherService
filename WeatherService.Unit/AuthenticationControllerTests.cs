using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using WeatherService.Controllers;
using WeatherService.Data.Model;
using WeatherService.Data.Repository;
using WeatherService.Domain.Service;

namespace WeatherService.Unit
{
    public class AuthenticationControllerTests
    {
        public AuthenticationController authenticationController;

        [Test]
        public async Task GivenValidUserCredentials_WhenAttemptLogin_ThenShouldGetBackToken()
        {
            // Arrange
            SetupContext();

            // Act
            var actionResult = await this.authenticationController.Login(new LoginModel() { UserName = "user", Password = "userPassword"});

            // Assert
            Assert.IsTrue(actionResult is OkObjectResult);
            var result = ((OkObjectResult)actionResult).Value;
            var tokenPropInfo = result.GetType().GetProperty("token");
            Assert.IsTrue(tokenPropInfo.GetValue(result) != null);
        }

        [Test]
        public async Task GivenInValidUserCredentials_WhenAttemptLogin_ThenShouldGetBackUnauthorizedResult()
        {
            // Arrange
            SetupContext(true);

            // Act
            var actionResult = await this.authenticationController.Login(new LoginModel() { UserName = "admin", Password = "userPassword" });

            // Assert
            Assert.IsTrue(actionResult is UnauthorizedResult);
        }

        private void SetupContext(bool negative = false)
        {
            var repo = new Mock<IAuthenticationRepository>();
            repo.Setup(c => c.ValidateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(negative ? (string.Empty, false) : ("User", true));
            repo.Setup(c => c.GetTokenForValidUser(It.IsAny<List<Claim>>())).Returns(new JwtSecurityToken());
            var authentiationService = new AuthentiationService(repo.Object);
            this.authenticationController = new AuthenticationController(authentiationService);
        }
    }
}
