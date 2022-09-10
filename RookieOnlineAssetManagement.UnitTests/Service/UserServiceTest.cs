using Xunit;
using Moq;
using RookieOnlineAssetManagement.Service.IServices;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Enum;
using RookieOnlineAssetManagement.Service.Services;
using AutoMapper;
using RookieOnlineAssetManagement.Entities.Dtos.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using System.Security.Claims;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;
using System.Data.Entity.Infrastructure;
using Xunit.Abstractions;
using RookieOnlineAssetManagement.UnitTests.Data;

namespace RookieOnlineAssetManagement.UnitTests.Service
{
    public class UserServiceTest : SQLiteContext
    {
        private static IMapper _mapper;

        public UserServiceTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        private IUserService GetSqlLiteUserService()
        {
            var dbContext = CreateContext();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var userInfor = FakeData.UserFakeData.GetUserDetail();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, userInfor.Id),
                                        new Claim(ClaimTypes.Name, userInfor.UserName)
                                   }, "TestAuthentication"));
            var context = new DefaultHttpContext()
            {
                User = user
            };
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            return new UserService(dbContext, _mapper, mockHttpContextAccessor.Object, mockUserManager.Object);
        }

        [Fact]
        public async void GetIdUser_WhenCalled_ReturnsFullNameAsync()
        {
            IUserService service = GetSqlLiteUserService();
            var userId = "123";
            var result = await service.GetUserDetailsAsync(userId);
            Assert.Equal("Tuan Nguyen", result.FullName);
        }



        [Fact]
        public async void GetIdUser_WhenCalled_ReturnsFullNameNotCorrect()
        {
            var userId = "123";
            IUserService service = GetSqlLiteUserService();
            var result = await service.GetUserDetailsAsync(userId);
            Assert.NotEqual("Nguyen Van B", result.FullName);
        }


        [Fact]
        public async void GetIdUser_WhenCalled_ReturnsNotFoundUser()
        {
            var userId = "abc";
            IUserService service = GetSqlLiteUserService();

            Func<Task> act = async () => await service.GetUserDetailsAsync(userId);
            Exception exception = await Assert.ThrowsAsync<Exception>(act);

            Assert.Equal($"Cannot find user with id: {userId}", exception.Message);
        }


        [Fact]
        public async Task GetListUser_WhenCalled_ReturnsListUser()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "";
            string[] types = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IUserService service = GetSqlLiteUserService();
            var result = await service.GetUsersListAsync(page, pageSize, keyword, types, sortOrder, sortField);
            Assert.NotNull(result.Users);
        }

        [Fact]
        public async Task GetListUser_WhenCalled_ReturnCountListUserNotEqual2()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "";
            string[] types = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IUserService service = GetSqlLiteUserService();
            var result = await service.GetUsersListAsync(page, pageSize, keyword, types, sortOrder, sortField);
            Assert.NotEqual(2, result.Users.Count);
        }

        [Fact]
        public async Task GetListUserByTypeStaff_WhenCalled_ReturnTwoUserTypeStaff()
        {
            var page = 1;
            var pageSize = 2;
            var keyword = "";
            string[] types = new string[] { "Staff" };
            var sortOrder = "";
            var sortField = "";
            IUserService service = GetSqlLiteUserService();
            var result = await service.GetUsersListAsync(page, pageSize, keyword, types, sortOrder, sortField);
            Assert.Equal(2, result.Users.Count);
            Assert.Contains(result.Users, item => item.Type == "Staff");
        }

        [Fact]
        public async Task GetListUserByTypeStaff_WhenCalled_ReturnTwoUserTypeAdmin()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "";
            string[] types = new string[] { "Staff" };
            var sortOrder = "";
            var sortField = "";
            IUserService service = GetSqlLiteUserService();
            var result = await service.GetUsersListAsync(page, pageSize, keyword, types, sortOrder, sortField);
            Assert.Equal(2, result.Users.Count);
            Assert.DoesNotContain(result.Users, item => item.Type == "Admin");
        }

        [Fact]
        public async Task GetListUserByKeyWordUsername_WhenCalled_ReturnFourUserWithKeyWordOfUsername()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "tuankiet";
            string[] types = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IUserService service = GetSqlLiteUserService();
            var result = await service.GetUsersListAsync(page, pageSize, keyword, types, sortOrder, sortField);
            Assert.Equal(4, result.Users.Count);
            Assert.Contains(result.Users, item => item.UserName.Contains(keyword));
        }

        [Fact]
        public async Task GetListUserByKeyWordUsername_WhenCalled_ReturnFourUserNotContainKeyWordOfUsername()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "tuankiet";
            string[] types = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IUserService service = GetSqlLiteUserService();
            var result = await service.GetUsersListAsync(page, pageSize, keyword, types, sortOrder, sortField);
            Assert.Equal(4, result.Users.Count);
            Assert.DoesNotContain(result.Users, item => item.UserName.Contains("abc"));
        }

        [Fact]
        public async Task GetListUserByKeyWordStaffCode_WhenCalled_ReturnFourUserWithKeyWordOfStaffCode()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "187pm123";
            string[] types = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IUserService service = GetSqlLiteUserService();
            var result = await service.GetUsersListAsync(page, pageSize, keyword, types, sortOrder, sortField);
            Assert.Equal(4, result.Users.Count);
            Assert.Contains(result.Users, item => item.StaffCode.Contains(keyword));
        }

        [Fact]
        public async Task GetListUserByKeyWordStaffCode_WhenCalled_ReturnFourUserNotContainKeyWordOfStaffCode()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "187pm123";
            string[] types = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IUserService service = GetSqlLiteUserService();
            var result = await service.GetUsersListAsync(page, pageSize, keyword, types, sortOrder, sortField);
            Assert.Equal(4, result.Users.Count);
            Assert.DoesNotContain(result.Users, item => item.UserName.Contains("abc"));
        }

        [Fact]
        public async Task GetListUserByKeyWordStaffCodeAndTypeAdmin_WhenCalled_ReturnTwoUserContainKeyWordOfStaffCodeWithTypeAdmin()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "187pm123";
            string[] types = new string[] { "Admin" };
            var sortOrder = "";
            var sortField = "";
            IUserService service = GetSqlLiteUserService();
            var result = await service.GetUsersListAsync(page, pageSize, keyword, types, sortOrder, sortField);
            Assert.Equal(2, result.Users.Count);
            Assert.Contains(result.Users, item => item.StaffCode.Contains(keyword));
            Assert.Contains(result.Users, item => item.Type == "Admin");
        }

        [Fact]
        public async Task GenerateCode()
        {
            IUserService service = GetSqlLiteUserService();
            // Act
            var result = await service.CreateUserAsync(FakeData.UserFakeData.CreateUser());

            // Assert
            Assert.Equal(FakeData.UserFakeData.ResUser().UserName, result.UserName);
        }

        [Fact]
        public async Task Update_WhenSuccess_ShouldReturnTrue()
        {
            // Arrange
            var newUser = new User()
            {
                Id = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                DateOfBirth = DateTime.Now,
                UserName = "tuankiet2106",
                Gender = Gender.Male,
                JoinedDate = DateTime.Now,
                Location = Location.HoChiMinh,
                StaffCode = "187pm1233",
            };
            var role = new Role()
            {
                Id = "cd353897-57cb-48a1-bb9c-20780c891962",
                Name = "187pm1231"
            };
            var userRole = new IdentityUserRole<string>()
            {
                UserId = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                RoleId = "cd353897-57cb-48a1-bb9c-20780c891962"
            };

            var dbContext = CreateContext();
            dbContext.Add(newUser);
            dbContext.Add(role);
            dbContext.Add(userRole);
            dbContext.SaveChanges();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var _mockUserManager = new Mock<FakeUserManager>();

            var _userService = new UserService(dbContext, _mapper, mockHttpContextAccessor.Object, _mockUserManager.Object);

            // Act
            var result = await _userService.UpdateAsync(FakeData.UserFakeData.UpdateResUser());

            // Assert
            Assert.True(result.Equals(true));
        }

        [Fact]
        public async Task Update_WhenNoUser_ShouldThrowException()
        {
            // Arrange
            var newUser = new User()
            {
                Id = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                DateOfBirth = DateTime.Now,
                UserName = "tuankiet2106",
                Gender = Gender.Male,
                JoinedDate = DateTime.Now,
                Location = Location.HoChiMinh,
                StaffCode = "187pm1233",
            };

            var dbContext = CreateContext();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var _mockUserManager = new Mock<FakeUserManager>();

            var _userService = new UserService(dbContext, _mapper, mockHttpContextAccessor.Object, _mockUserManager.Object);

            // Act
            Func<Task> result = async () => await _userService.UpdateAsync(FakeData.UserFakeData.UpdateResUser());

            // Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(result);
            Assert.Equal($"Cannot find user with id: {FakeData.UserFakeData.UpdateResUser().id}", exception.Message);
        }

        [Fact]
        public async Task DisableUser_WhenSuccess_ShouldReturnTrue()
        {
            // Arrange
            var newUser = new User()
            {
                Id = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                DateOfBirth = DateTime.Now,
                UserName = "tuankiet2106",
                Gender = Gender.Male,
                JoinedDate = DateTime.Now,
                Location = Location.HoChiMinh,
                StaffCode = "187pm1233",
            };

            var dbContext = CreateContext();
            dbContext.Add(newUser);
            dbContext.SaveChanges();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var _mockUserManager = new Mock<FakeUserManager>();

            var _userService = new UserService(dbContext, _mapper, mockHttpContextAccessor.Object, _mockUserManager.Object);

            // Act
            var result = await _userService.DisableUserAsync(FakeData.UserFakeData.UpdateResUser().id);

            // Assert
            Assert.True(result.Equals(true));
        }

        [Fact]
        public async Task DisableUser_WhenNoUser_ShouldThrowException()
        {
            // Arrange
            var newUser = new User()
            {
                Id = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                DateOfBirth = DateTime.Now,
                UserName = "tuankiet2106",
                Gender = Gender.Male,
                JoinedDate = DateTime.Now,
                Location = Location.HoChiMinh,
                StaffCode = "187pm1233",
            };

            var dbContext = CreateContext();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var _mockUserManager = new Mock<FakeUserManager>();

            var _userService = new UserService(dbContext, _mapper, mockHttpContextAccessor.Object, _mockUserManager.Object);

            // Act
            Func<Task> result = async () => await _userService.DisableUserAsync(FakeData.UserFakeData.UpdateResUser().id);

            // Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(result);
            Assert.Equal($"Cannot find user with id: {FakeData.UserFakeData.UpdateResUser().id}", exception.Message);
        }

        [Fact]
        public async Task GetUserUpdateDetailsAsync_WhenUser_ShouldReturnUserUpdateDetail()
        {
            // Arrange
            var newUser = new User()
            {
                Id = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                StaffCode = "187pm1233",
                FirstName = "thien",
                LastName = "ho duc",
                DateOfBirth = new DateTime(2020, 07, 26, 18, 44, 7),
                Gender = Gender.Male,
                JoinedDate = new DateTime(2022, 07, 26, 18, 44, 7),
            };
            var role = new Role()
            {
                Id = "cd353897-57cb-48a1-bb9c-20780c891962",
                Name = "187pm1231"
            };
            var userRole = new IdentityUserRole<string>()
            {
                UserId = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                RoleId = "cd353897-57cb-48a1-bb9c-20780c891962"
            };

            var dbContext = CreateContext();
            dbContext.Add(newUser);
            dbContext.Add(role);
            dbContext.Add(userRole);
            dbContext.SaveChanges();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var _mockUserManager = new Mock<FakeUserManager>();

            var _userService = new UserService(dbContext, _mapper, mockHttpContextAccessor.Object, _mockUserManager.Object);

            // Act
            var result = await _userService.GetUserUpdateDetailsAsync(FakeData.UserFakeData.UpdateDetail().Id);

            // Assert
            Assert.Equal(FakeData.UserFakeData.UpdateDetail().Id, result.Id);
        }

        [Fact]
        public async Task GetUserUpdateDetailsAsync_WhenNoUser_ShouldThrowException()
        {
            // Arrange
            var newUser = new User()
            {
                Id = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                StaffCode = "187pm1233",
                FirstName = "thien",
                LastName = "ho duc",
                DateOfBirth = DateTime.Now,
                Gender = Gender.Male,
                JoinedDate = DateTime.Now,
            };

            var dbContext = CreateContext();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var _mockUserManager = new Mock<FakeUserManager>();

            var _userService = new UserService(dbContext, _mapper, mockHttpContextAccessor.Object, _mockUserManager.Object);

            // Act
            Func<Task> result = async () => await _userService.GetUserUpdateDetailsAsync(FakeData.UserFakeData.UpdateDetail().Id);

            // Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(result);
            Assert.Equal($"Cannot find user with id: {FakeData.UserFakeData.UpdateDetail().Id}", exception.Message);
        }

        [Fact]
        public async Task GetUserUpdateDetailsAsync_WhenNoUserRole_ShouldThrowException()
        {
            // Arrange
            var newUser = new User()
            {
                Id = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                StaffCode = "187pm1233",
                FirstName = "thien",
                LastName = "ho duc",
                DateOfBirth = DateTime.Now,
                Gender = Gender.Male,
                JoinedDate = DateTime.Now,
            };
            var role = new Role()
            {
                Id = "cd353897-57cb-48a1-bb9c-20780c891962",
                Name = "187pm1231"
            };

            var dbContext = CreateContext();
            dbContext.Add(newUser);
            dbContext.Add(role);
            dbContext.SaveChanges();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var _mockUserManager = new Mock<FakeUserManager>();

            var _userService = new UserService(dbContext, _mapper, mockHttpContextAccessor.Object, _mockUserManager.Object);

            // Act
            Func<Task> result = async () => await _userService.GetUserUpdateDetailsAsync(FakeData.UserFakeData.UpdateDetail().Id);

            // Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(result);
            Assert.Equal($"Cannot find role with userid: {FakeData.UserFakeData.UpdateDetail().Id}", exception.Message);
        }
    }
}
