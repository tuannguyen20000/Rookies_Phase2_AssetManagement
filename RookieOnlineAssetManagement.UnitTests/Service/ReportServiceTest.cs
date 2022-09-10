using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Service.IServices;
using RookieOnlineAssetManagement.Service.Services;
using RookieOnlineAssetManagement.UnitTests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.UnitTests.Service
{
    public class ReportServiceTest : SQLiteContext
    {
        private static IMapper _mapper;
        private readonly ApplicationDbContext dbContext;
        public ReportServiceTest()
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

        private IReportService GetSqlLiteService()
        {
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
            return new ReportService(dbContext, mockHttpContextAccessor.Object,_mapper);
        }

        [Fact]
        public async Task GetListReport_WhenCalled_Return2Report()
        {
            var page = 1;
            var pageSize = 10;
            var sortOrder = "";
            var sortField = "";
            IReportService service = GetSqlLiteService();
            var result = await service.GetListReportAsync(page, pageSize, sortOrder, sortField);
            Assert.Equal(4, result.Reports.Count);
        }

        [Fact]
        public async Task GetListReport_WhenCalled_ReturnSortDescendingCategory()
        {
            var page = 1;
            var pageSize = 10;
            var sortOrder = "descend";
            var sortField = "category";
            IReportService service = GetSqlLiteService();
            var result = await service.GetListReportAsync(page, pageSize, sortOrder, sortField);
            var expectedResult = result.Reports.OrderByDescending(x => x.Category);
            Assert.Equal(expectedResult, result.Reports);
            Assert.Equal(expectedResult.LastOrDefault().Category, result.Reports.LastOrDefault().Category);
        }

        [Fact]
        public async Task GetListReport_WhenCalled_ReturnSortAscendingCategory()
        {
            var page = 1;
            var pageSize = 10;
            var sortOrder = "ascend";
            var sortField = "category";
            IReportService service = GetSqlLiteService();
            var result = await service.GetListReportAsync(page, pageSize, sortOrder, sortField);
            var expectedResult = result.Reports.OrderBy(x => x.Category);
            Assert.Equal(expectedResult, result.Reports);
            Assert.Equal(expectedResult.LastOrDefault().Category, result.Reports.LastOrDefault().Category);
        }

        [Fact]
        public async Task GetListReport_WhenCalled_ReturnNotSortDescendingCategory()
        {
            var page = 1;
            var pageSize = 10;
            var sortOrder = "ascend";
            var sortField = "category";
            IReportService service = GetSqlLiteService();
            var result = await service.GetListReportAsync(page, pageSize, sortOrder, sortField);
            var expectedResult = result.Reports.OrderByDescending(x => x.Category);
            Assert.NotEqual(expectedResult, result.Reports);
            Assert.NotEqual(expectedResult.LastOrDefault().Category, result.Reports.LastOrDefault().Category);
        }

        [Fact]
        public async Task GetListReport_WhenCalled_ReturnNotSortAscendingCategory()
        {
            var page = 1;
            var pageSize = 10;
            var sortOrder = "descend";
            var sortField = "category";
            IReportService service = GetSqlLiteService();
            var result = await service.GetListReportAsync(page, pageSize, sortOrder, sortField);
            var expectedResult = result.Reports.OrderBy(x => x.Category);
            Assert.NotEqual(expectedResult, result.Reports);
            Assert.NotEqual(expectedResult.LastOrDefault().Category, result.Reports.LastOrDefault().Category);
        }

        [Fact]
        public async Task GetListReport_WhenCalled_ReturnCountTotalFirstItemEqual1()
        {
            var page = 1;
            var pageSize = 10;
            var sortOrder = "";
            var sortField = "";
            IReportService service = GetSqlLiteService();
            var result = await service.GetListReportAsync(page, pageSize, sortOrder, sortField);
            Assert.Equal(1, result.Reports.FirstOrDefault().Total);
        }
    }
}
