using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit.Abstractions;
using RookieOnlineAssetManagement.Controllers;
using RookieOnlineAssetManagement.Service.IServices;

namespace RookieOnlineAssetManagement.UnitTests.Identity
{
    public class LogoutModelTest
    {
        private readonly ITestOutputHelper _output;
        private Mock<FakeSignInManager> _mockSignInManager;
        private Mock<FakeUserManager> _mockUserManager;
        private Mock<IUserService> _userService;

        public LogoutModelTest(ITestOutputHelper output)
        {
            _output = output;
            _mockSignInManager = new Mock<FakeSignInManager>();
            _mockUserManager = new Mock<FakeUserManager>();
            _userService = new Mock<IUserService>();
        }

        [Fact]
        public async Task ExecuteAsync_InvokesSignOutAsyncOnAllConfiguredSchemes()
        {
            // Arrange
            _mockSignInManager.Setup(
                x => x.SignOutAsync()).Returns(Task.FromResult(SignInResult.Success));

            var unitUnderTest = new UsersController(_userService.Object,_mockUserManager.Object, _mockSignInManager.Object);

            // Act
           var result = await unitUnderTest.Logout();

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
        }

    }
}
