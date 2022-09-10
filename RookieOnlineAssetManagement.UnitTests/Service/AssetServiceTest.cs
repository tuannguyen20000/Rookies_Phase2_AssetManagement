using RookieOnlineAssetManagement.Service.IServices;
using RookieOnlineAssetManagement.Service.Services;
using RookieOnlineAssetManagement.UnitTests.Data;
using RookieOnlineAssetManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Common;
using AutoMapper;
using Moq;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using RookieOnlineAssetManagement.Entities.Dtos.AssetService;
using RookieOnlineAssetManagement.Entities.Enum;

namespace RookieOnlineAssetManagement.UnitTests.Service
{
    public class AssetServiceTest : SQLiteContext
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
        private static IMapper _mapper;
        private readonly ApplicationDbContext dbContext;
        private readonly ITestOutputHelper _output;

        public AssetServiceTest(ITestOutputHelper output)
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
            _output = output;
            dbContext = CreateContext();
        }

        private IAssetService GetSqlLiteAssetService()
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

            return new AssetService(dbContext, mockHttpContextAccessor.Object, _mapper);
        }

        [Fact]
        public async void GetIdAsset_WhenCalled_ReturnsAssetCodeAsync()
        {
            IAssetService service = GetSqlLiteAssetService();
            var assetId = 1;
            var result = await service.GetAssetDetailsAsync(assetId);
            Assert.Equal("CA000001", result.AssetCode);
        }

        [Fact]
        public async Task GetAllAsset_WhenCalled_ReturnsListAsset()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "";
            var sortOrder = "assend";
            var sortField = "assetName";
            string[] state = new string[] {};
            string[] category = new string[] {};
            IAssetService service = GetSqlLiteAssetService();
            var result = await service.GetAllAssetsAsync(page, pageSize, keyword, state, category, sortOrder, sortField);
            Assert.Equal(2, result.TotalItem);
        }

        [Fact]
        public async Task GetCategoriesAsset_WhenCalled_ReturnsListAsset()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "";
            var sortOrder = "assend";
            var sortField = "assetName";
            string[] state = new string[] {};
            string[] category = new string[] { "Cate1" };
            IAssetService service = GetSqlLiteAssetService();
            var result = await service.GetAllAssetsAsync(page, pageSize, keyword, state, category, sortOrder, sortField);
            Assert.Equal(1, result.TotalItem);
        }

        [Fact]
        public async Task GetStateAsset_WhenCalled_ReturnsListAsset()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "";
            var sortOrder = "assend";
            var sortField = "assetName";
            string[] state = new string[] { "Available" };
            string[] category = new string[] { "All" };
            IAssetService service = GetSqlLiteAssetService();
            var result = await service.GetAllAssetsAsync(page, pageSize, keyword, state, category, sortOrder, sortField);
            Assert.Equal(1, result.TotalItem);
        }

        [Fact]
        public async Task GetFilterAsset_WhenCalled_ReturnsListAsset()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "";
            var sortOrder = "assend";
            var sortField = "assetName";
            string[] state = new string[] { AssetState.Available.ToString() };
            string[] category = new string[] { "Cate1" };
            IAssetService service = GetSqlLiteAssetService();
            var result = await service.GetAllAssetsAsync(page, pageSize, keyword, state, category, sortOrder, sortField);
            Assert.Equal(1, result.TotalItem);
        }

        [Fact]
        public async Task GetKeywordAsset_WhenCalled_ReturnsListAsset()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "CA000001";
            var sortOrder = "assend";
            var sortField = "assetName";
            string[] state = new string[] {};
            string[] category = new string[] {};
            IAssetService service = GetSqlLiteAssetService();
            var result = await service.GetAllAssetsAsync(page, pageSize, keyword, state, category, sortOrder, sortField);
            Assert.Equal(1, result.TotalItem);
        }

        [Fact]
        public async void CreateAsset_WhenSuccess_ReturnAssetCreated()
        {
            // Arrange
            IAssetService service = GetSqlLiteAssetService();

            // Act
            var result = await service.CreateAssetAsync(FakeData.AssetFakeData.CreateAsset());

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsset_WhenInputNull_ThrowNewException()
        {
            // Arrange
            IAssetService service = GetSqlLiteAssetService();

            // Act
            Func<Task> act = async () => await service.CreateAssetAsync(new CreateAssetDto());

            // Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Input asset is null", exception.Message);
        }

        [Fact]
        public async Task GetCategories_WhenSuccess_ReturnCategories()
        {
            // Arrange
            IAssetService service = GetSqlLiteAssetService();

            // Act
            var result = await service.GetCategoriesAsync();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCategory_WhenSuccess_ReturnCategory()
        {
            // Arrange
            IAssetService service = GetSqlLiteAssetService();
            int categoryId = 1;

            // Act
            var result = await service.GetCategoryAsync(categoryId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCategory_WhenNoCategory_ThrowNewException()
        {
            // Arrange
            IAssetService service = GetSqlLiteAssetService();
            int categoryId = 20;

            // Act
            Func<Task> act = async () => await service.GetCategoryAsync(categoryId);

            // Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal($"Can not find category with id: {categoryId}", exception.Message);
        }

        [Fact]
        public async Task CreateCategory_WhenSuccess_ReturnCategory()
        {
            // Arrange
            IAssetService service = GetSqlLiteAssetService();

            // Act
            var result = await service.CreateCategoryAsync(FakeData.AssetFakeData.CreateCategory());

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateCategory_WhenNoInput_ThrowNewException()
        {
            // Arrange
            IAssetService service = GetSqlLiteAssetService();

            // Act
            Func<Task> act = async () => await service.CreateCategoryAsync(new CategoryDto());


            // Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Input category is null", exception.Message);
        }

        [Fact]
        public async Task GenerateAssetCode_WhenSuccess_ReturnAssetCode()
        {
            // Arrange
            IAssetService service = GetSqlLiteAssetService();
            int categoryId = 1;

            // Act
            var result = await service.GenerateAssetCodeAsync(categoryId);

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async void UpdateAsset_WhenCall_ReturnUpdated_Success()
        {
            //Arrange
            IAssetService service = GetSqlLiteAssetService();
            int assetId = 2;
            //Act
            var result = await service.UpdateAsync(assetId, FakeData.AssetFakeData.UpdateAsset());
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async void GetUpdateDetail_WhenCall_GetUpdate_Return_Success()
        {
            //Arrange
            IAssetService service = GetSqlLiteAssetService();
            int assetId = 1;
            //Act
            var result = await service.GetUpdateAsync(assetId);
            //Assert
            Assert.Equal(result.Id, FakeData.AssetFakeData.GetAssetUpdateDetail().Id);
            Assert.Equal(result.AssetName, FakeData.AssetFakeData.GetAssetUpdateDetail().AssetName);
            Assert.Equal(result.CategoryId, FakeData.AssetFakeData.GetAssetUpdateDetail().CategoryId);
            Assert.Equal(result.CategoryName, FakeData.AssetFakeData.GetAssetUpdateDetail().CategoryName);
            Assert.Equal(result.Specification, FakeData.AssetFakeData.GetAssetUpdateDetail().Specification);
            Assert.Equal(result.State, FakeData.AssetFakeData.GetAssetUpdateDetail().State);
            Assert.Equal(result.InstalledDate, FakeData.AssetFakeData.GetAssetUpdateDetail().InstalledDate);

        }
        [Fact]
        public async void DeleteAsset_WhenCall_AssetDeleted()
        {
            //Arrange
            IAssetService service = GetSqlLiteAssetService();
            int assetId = 8;
            //Act
            var result = await service.DeleteAsync(assetId);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public async void DeleteAsset_WhenCall_Delete_WhenAssetHasHistory()
        {
            //Arrange
            IAssetService service = GetSqlLiteAssetService();
            int assetId = 1;
            //Act
            var result = await service.DeleteAsync(assetId);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public async void DeleteAsset_WhenCall_ReturnNotFoundAsset()
        {
            //Arrange
            IAssetService service = GetSqlLiteAssetService();
            int assetId = 10;
            //Act
            Func<Task> result = async () => await service.DeleteAsync(assetId);
            //Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(result);
            Assert.Equal($"Can not find asset with id: {assetId}", exception.Message);
        }

        [Fact]
        public async void GetUpdateDetail_WhenCall_GetUpdate_ReturnNotFoundAsset()
        {
            //Arrange
            IAssetService service = GetSqlLiteAssetService();
            int assetId = 10;
            //Act
            Func<Task> result = async () => await service.GetUpdateAsync(assetId);
            //Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(result);
            Assert.Equal($"Can not find asset with id: {assetId}", exception.Message);
        }

        [Fact]
        public async void CheckHistory_WhenCall_AssetHasHistory()
        {
            //Arrange
            IAssetService service = GetSqlLiteAssetService();
            int assetId = 1;
            //Act
            var result = await service.CheckHistoryAssetAsync(assetId);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public async void CheckHistory_WhenCall_AssetNotHaveHistory()
        {
            //Arrange
            IAssetService service = GetSqlLiteAssetService();
            int assetId = 8;
            //Act
            var result = await service.CheckHistoryAssetAsync(assetId);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public async void CheckHistory_WhenCall_ReturnNotFoundAsset()
        {
            //Arrange
            IAssetService service = GetSqlLiteAssetService();
            int assetId = 10;
            //Act
            Func<Task> result = async () => await service.CheckHistoryAssetAsync(assetId);
            //Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(result);
            Assert.Equal($"Can not find asset with id: {assetId}", exception.Message);
        }
        [Fact]
        public async void GetState()
        {
            //Arrange
            IAssetService service = GetSqlLiteAssetService();
            int assetId = 1;
            //Act
            var result = await service.GetStateAsync(assetId);
            //Assert
            Assert.Equal(result, FakeData.AssetFakeData.GetAssetUpdateDetail().State.ToString());
        }
    }
}
