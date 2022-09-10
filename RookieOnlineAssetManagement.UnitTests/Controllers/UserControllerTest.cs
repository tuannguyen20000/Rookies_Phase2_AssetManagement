using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RookieOnlineAssetManagement.Controllers;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models;
using RookieOnlineAssetManagement.Service.IServices;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace RookieOnlineAssetManagement.UnitTests.Controllers
{
    public class UserControllerTest
    {
        private readonly ITestOutputHelper _output;
        private Mock<FakeUserManager> _mockUserManager;
        private Mock<FakeSignInManager> _mockSignInManager;
        private Mock<IUserService> _mockUserService;

        public UserControllerTest(ITestOutputHelper output)
        {
            _mockUserManager = new Mock<FakeUserManager>();
            _mockSignInManager = new Mock<FakeSignInManager>();
            _mockUserService = new Mock<IUserService>();
            _output = output;
        }

        [Fact]
        public async Task Get_WhenSuccess_ReturnUserModel()
        {
            // Arrange
            IList<string> role = new List<string>() { "Administrator" };
            var user = new User()
            {
                Id = "6d38f2e2-da5a-49ab-a6e2-bbfde1c68582",
                UserName = "admin",
                Email = "abc@abc.com",
                FirstLogin = true,
            };
            var returnedUser = new UserModel()
            {
                Id = "6d38f2e2-da5a-49ab-a6e2-bbfde1c68582",
                UserName = "admin",
                Email = "abc@abc.com",
                Role = new List<string>() { "Administrator" },
                FirstLogin = true,
            };
            var tempUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "abc@abc.com"),
            }));

            _mockUserManager.Setup(userManager => userManager.GetUserAsync(tempUser)).Returns(Task.FromResult(user));
            _mockUserManager.Setup(userManager => userManager.GetRolesAsync(user)).Returns(Task.FromResult(role));

            var controller = new UsersController(_mockUserService.Object, _mockUserManager.Object, _mockSignInManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = tempUser
                }
            };

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = okResult.Value as UserModel;
            Assert.Equal(data.Id, returnedUser.Id);
            Assert.Equal(data.UserName, returnedUser.UserName);
            Assert.Equal(data.Email, returnedUser.Email);
            Assert.Equal(data.Role, returnedUser.Role);
            Assert.Equal(data.FirstLogin, returnedUser.FirstLogin);
        }

        [Fact]
        public async Task ChangePassword_OnSuccess_ReturnOk()
        {
            // Arrange
            var changePasswordRequest = new ChangePasswordRequest()
            {
                Id = "6d38f2e2-da5a-49ab-a6e2-bbfde1c68582",
                NewPassword = "@Bc123",
            };
            var user = new User()
            {
                Id = "6d38f2e2-da5a-49ab-a6e2-bbfde1c68582",
                UserName = "admin",
                Email = "abc@abc.com",
                FirstLogin = true,
            };
            var token = "UZSt6O9KjPt95TrwpRyQuRLki5JTtU0B8p7BxFLc3NJe2a0xI3mONmnJ7A5Vi6Axorgmq5xEOf4tyNz7pZQeBuq4i1C9Dfur2MM5y4AAI4ccWwsxdAtW420SS1aniRsiIb5s0oqCsc6Mcl3HBXa3CHAPAR1T7ICkYpTawGC72gqq3BkPrEWt18Dp2czQI2Iom6bRw0s02z2f9vd7Tl8Hr0Il1Omr5iox94NAEgG3ZoBwzqLy8wkwhtoD47v";

            // Act
            _mockUserManager.Setup(userManager => userManager.FindByIdAsync(changePasswordRequest.Id)).Returns(Task.FromResult(user));
            _mockUserManager.Setup(userManager => userManager.GeneratePasswordResetTokenAsync(user)).Returns(Task.FromResult(token));
            _mockUserManager.Setup(userManager => userManager.ResetPasswordAsync(user, token, changePasswordRequest.NewPassword)).Returns(Task.FromResult(IdentityResult.Success));

            var controller = new UsersController(_mockUserService.Object, _mockUserManager.Object, _mockSignInManager.Object);

            var result = await controller.ChangeFirstPassword(changePasswordRequest);
            
            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
