using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Service.IServices;
using RookieOnlineAssetManagement.Service.Services;
using RookieOnlineAssetManagement.UnitTests.Data;
using System;
using System.Data.Common;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.UnitTests.Service
{
    public class RequestServiceTest : SQLiteContext
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;
        private static IMapper _mapper;
        private readonly ApplicationDbContext dbContext;

        public RequestServiceTest()
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
            dbContext = CreateContext();
        }

        private IRequestService GetSqlLiteRequestService()
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

            return new RequestService(dbContext, _mapper, mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task CompleteRequest_WhenSuccess_ReturnDetailRequest()
        {
            // Arrange
            IRequestService service = GetSqlLiteRequestService();
            var requestId = 1;

            // Act
            var result = await service.CompleteRequestAsync(requestId);

            // Assert
            Assert.Equal(FakeData.RequestFakeData.CompleteRequest().RequestState, result.RequestState);
        }

        [Fact]
        public async Task CompleteRequest_WhenFailed_ReturnNull()
        {
            // Arrange
            IRequestService service = GetSqlLiteRequestService();
            var requestId = 10;

            // Act
            var result = await service.CompleteRequestAsync(requestId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CancelRequest_WhenSuccess_ReturnDetailRequest()
        {
            // Arrange
            IRequestService service = GetSqlLiteRequestService();
            var requestId = 1;

            // Act
            var result = await service.CancelRequestAsync(requestId);

            // Assert
            Assert.Equal(FakeData.RequestFakeData.CancelRequest().RequestState, result.RequestState);
        }

        [Fact]
        public async Task CancelRequest_WhenFailed_ReturnNull()
        {
            // Arrange
            IRequestService service = GetSqlLiteRequestService();
            var requestId = 10;

            // Act
            var result = await service.CancelRequestAsync(requestId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateRequest_WhenSuccess_ReturnDetailRequest()
        {
            IRequestService service = GetSqlLiteRequestService();
            var requestId = 1;

            // Act
            var result = await service.CreateRequestAsync(requestId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateRequest_WhenFailed_ReturnDetailRequest()
        {
            IRequestService service = GetSqlLiteRequestService();
            var requestId = 2;

            // Act
            var result = await service.CreateRequestAsync(requestId);

            // Assert
            Assert.Null(result.AssetCode);
        }

        [Fact]
        public async Task GetListRequest_States_All()
        {
            IRequestService service = GetSqlLiteRequestService();

            // Act
            int page = 1;
            int pageSize = 10;
            string keyword = "";
            DateTime returnedDate = new DateTime();
            string[] states = new string[] { "All" };
            string sortOrder = "ascend";
            string sortField = "assetCode";
            var result = await service.GetRequestListAsync(page, pageSize, keyword, returnedDate, states, sortOrder, sortField);

            // Assert
            Assert.Equal(2, result.TotalItem);
        }

        [Fact]
        public async Task GetListRequest_States_Completed()
        {
            IRequestService service = GetSqlLiteRequestService();

            // Act
            int page = 1;
            int pageSize = 10;
            string keyword = "";
            DateTime returnedDate = new DateTime();
            string[] states = new string[] { "Completed" };
            string sortOrder = "ascend";
            string sortField = "assetCode";
            var result = await service.GetRequestListAsync(page, pageSize, keyword, returnedDate, states, sortOrder, sortField);

            // Assert
            Assert.Equal(2, result.TotalItem);
        }

        [Fact]
        public async Task GetListRequest_ReturnedDate()
        {
            IRequestService service = GetSqlLiteRequestService();

            // Act
            int page = 1;
            int pageSize = 10;
            string keyword = "";
            DateTime returnedDate = DateTime.Now.Date;
            string[] states = new string[] { "All" };
            string sortOrder = "ascend";
            string sortField = "assetCode";
            var result = await service.GetRequestListAsync(page, pageSize, keyword, returnedDate, states, sortOrder, sortField);

            // Assert
            Assert.Equal(2, result.TotalItem);
        }

    }
}
