using RookieOnlineAssetManagement.Areas.Identity.Pages.Account;
using RookieOnlineAssetManagement.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Moq;

namespace RookieOnlineAssetManagement.UnitTests.Identity
{
    public class LoginModelTest
    {
        private readonly ITestOutputHelper _output;
        private Mock<FakeSignInManager> _mockSignInManager;
        private Mock<ILogger<LoginModel>> _mockLogger;
        private LoginModel.InputModel _inputModel;
        private Mock<FakeUserManager> _mockUserManager;

        public LoginModelTest(ITestOutputHelper output)
        {
            _output = output;
            _mockSignInManager = new Mock<FakeSignInManager>();
            _mockLogger = new Mock<ILogger<LoginModel>>();
            _inputModel = new LoginModel.InputModel();
            _mockUserManager = new Mock<FakeUserManager>();
        }

        [Fact]
        public async Task OnPostAsync_UserNotDisabled_UsernameCorrect_PasswordCorrect_ShouldLocalRedirectToHome()
        {
            // Arrange
            var user = new User
            {
                UserName = "admin",
                Disabled = false,
            };

            _mockSignInManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>())).Returns(Task.FromResult(SignInResult.Success));
            _mockUserManager.Setup(
                x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            var unitUnderTest = new LoginModel(_mockSignInManager.Object, _mockLogger.Object, _mockUserManager.Object);
            unitUnderTest.Input = _inputModel;

            // Act
            var result = await unitUnderTest.OnPostAsync("~/");

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.LocalRedirectResult>(result);
            Assert.NotNull(okResult.Url);
            Assert.Equal("~/", okResult.Url);
        }

        [Fact]
        public async Task OnPostAsync_UserNotDisabled_UsernameCorrect_PasswordWrong_ShouldReRenderPage()
        {
            // Arrange
            var user = new User
            {
                UserName = "admin",
                Disabled = false,
            };

            _mockSignInManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>())).Returns(Task.FromResult(SignInResult.Failed));
            _mockUserManager.Setup(
                x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            var unitUnderTest = new LoginModel(_mockSignInManager.Object, _mockLogger.Object, _mockUserManager.Object);
            unitUnderTest.Input = _inputModel;

            // Act
            var result = await unitUnderTest.OnPostAsync("~/");

            // Assert
            var failedResult = Assert.IsType<Microsoft.AspNetCore.Mvc.RazorPages.PageResult>(result);
            Assert.NotNull(failedResult);
        }


        [Fact]
        public async Task OnPostAsync_UserDisabled_UsernameCorrect_PasswordCorrect_ShouldReRenderPage()
        {
            // Arrange
            var user = new User
            {
                UserName = "admin",
                Disabled = true,
            };

            _mockSignInManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>())).Returns(Task.FromResult(SignInResult.Success));
            _mockUserManager.Setup(
                x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            var unitUnderTest = new LoginModel(_mockSignInManager.Object, _mockLogger.Object, _mockUserManager.Object);
            unitUnderTest.Input = _inputModel;

            // Act
            var result = await unitUnderTest.OnPostAsync("~/");

            // Assert
            var failedResult = Assert.IsType<Microsoft.AspNetCore.Mvc.RazorPages.PageResult>(result);
            Assert.NotNull(failedResult);
        }

        [Fact]
        public async Task OnPostAsync_UserDisabled_UsernameCorrect_PasswordWrong_ShouldReRenderPage()
        {
            // Arrange
            var user = new User
            {
                UserName = "admin",
                Disabled = true,
            };

            _mockSignInManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>())).Returns(Task.FromResult(SignInResult.Failed));
            _mockUserManager.Setup(
                x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            var unitUnderTest = new LoginModel(_mockSignInManager.Object, _mockLogger.Object, _mockUserManager.Object);
            unitUnderTest.Input = _inputModel;

            // Act
            var result = await unitUnderTest.OnPostAsync("~/");

            // Assert
            var failedResult = Assert.IsType<Microsoft.AspNetCore.Mvc.RazorPages.PageResult>(result);
            Assert.NotNull(failedResult);
        }

        [Fact]
        public async Task OnPostAsync_UserNull_UsernameWrong_PasswordAny_ShouldReRenderPage()
        {
            // Arrange
            _mockSignInManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>())).Returns(Task.FromResult(SignInResult.Failed));
            _mockUserManager.Setup(
                x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(new User()));

            var unitUnderTest = new LoginModel(_mockSignInManager.Object, _mockLogger.Object, _mockUserManager.Object);
            unitUnderTest.Input = _inputModel;

            // Act
            var result = await unitUnderTest.OnPostAsync("~/");

            // Assert
            var failedResult = Assert.IsType<Microsoft.AspNetCore.Mvc.RazorPages.PageResult>(result);
            Assert.NotNull(failedResult);
        }
    }
}
