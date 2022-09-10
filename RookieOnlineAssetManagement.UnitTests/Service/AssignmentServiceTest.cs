using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities.Enum;
using RookieOnlineAssetManagement.Service.IServices;
using RookieOnlineAssetManagement.Service.Services;
using RookieOnlineAssetManagement.UnitTests.Data;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.UnitTests.Service
{
    public class AssignmentServiceTest : SQLiteContext
    {
        private static IMapper _mapper;
        private readonly ApplicationDbContext dbContext;
        public AssignmentServiceTest()
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

        private IAssignmentService GetSqlLiteAssignmentService()
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
            return new AssignmentService(dbContext, _mapper, mockHttpContextAccessor.Object);
        }

        [Fact]
        public async void CreateAssignment_WhenCall_ReturnAssignmentCreated()
        {
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.CreateAssignmentAsync(FakeData.AssignmentFakeData.CreateAssignment());
            Assert.NotNull(result);
        }

        [Fact]
        public async void CreateAssignment_WhenCall_ReturnExceptionAssignedDateIsNotCurrentOrFuture()
        {
            IAssignmentService service = GetSqlLiteAssignmentService();
            Func<Task> act = async () => await service.CreateAssignmentAsync(FakeData.AssignmentFakeData.CreateAssignmentDateNotCurrentOrFuture());
            Exception exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Assigned Date is only current or future date", exception.Message);
        }

        [Fact]
        public async void CreateAssignment_WhenCall_ReturnStateIsWaitingForAcceptance()
        {
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.CreateAssignmentAsync(FakeData.AssignmentFakeData.CreateAssignment());
            Assert.Equal("WaitingForAcceptance", result.State);
        }

        [Fact]
        public async void CreateAssignment_WhenCall_ReturnStateAssetIsAssigned()
        {
            IAssignmentService service = GetSqlLiteAssignmentService();
            await service.CreateAssignmentAsync(FakeData.AssignmentFakeData.CreateAssignment());
            var dbContext = CreateContext();
            var asset = await dbContext.Assets.FindAsync(3);
            Assert.Equal(AssetState.Assigned, asset.State);
        }

        [Fact]
        public async void UpdateAssignment_WhenCall_ReturnAssignmentUpdated()
        {
            IAssignmentService service = GetSqlLiteAssignmentService();
            int assignmentId = 2;
            var result = await service.UpdateAssignmentAsync(assignmentId, FakeData.AssignmentFakeData.UpdateAssignment());
            Assert.NotNull(result);
        }

        [Fact]
        public async void UpdateAssignment_WhenCall_UpdateAssetAndReturnAssetUpdated()
        {
            IAssignmentService service = GetSqlLiteAssignmentService();
            int assignmentId = 2;
            var assignmentInDb = await dbContext.Assignments.FirstOrDefaultAsync(x => x.Id == assignmentId);
            var assetInDb = assignmentInDb.AssetId; // 2
            var result = await service.UpdateAssignmentAsync(assignmentId, FakeData.AssignmentFakeData.UpdateAssignment());
            Assert.NotEqual(assetInDb, result.AssetId);
            Assert.Equal(FakeData.AssignmentFakeData.UpdateAssignment().AssetId, result.AssetId);
        }

        [Fact]
        public async void UpdateAssignment_WhenCall_UpdateUserAndReturnUserUpdated()
        {
            IAssignmentService service = GetSqlLiteAssignmentService();
            int assignmentId = 2;
            var assignmentInDb = await dbContext.Assignments.FirstOrDefaultAsync(x => x.Id == assignmentId);
            var userInDb = assignmentInDb.UserId;
            var updateAssignment = FakeData.AssignmentFakeData.UpdateAssignment();
            updateAssignment.UserId = "101112";
            var result = await service.UpdateAssignmentAsync(assignmentId, updateAssignment);
            Assert.NotEqual(userInDb, result.UserId);
            Assert.Equal(updateAssignment.UserId, result.UserId);
        }

        [Fact]
        public async void DeleteAssignment_WhenCall_AssignmentDeleted()
        {
            IAssignmentService service = GetSqlLiteAssignmentService();
            int assignmentId = 2;
            var result = await service.DisableAssignmentAsync(assignmentId);
            var assignmentInDb = await dbContext.Assignments.FirstOrDefaultAsync(x => x.Id == assignmentId);
            Assert.Null(assignmentInDb);
        }
        //Assignment List Test
        [Fact]
        public async Task GetListAssignment_WhenCalled_ReturnsListAssignmentAll()
        {
            var page = 1;
            var pageSize = 8;
            var keyword = "";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignment_WhenCalled_ReturnsListAssignmentAccepted()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignment_WhenCalled_ReturnsListAssignmentWaitingForAcceptance()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignment_WhenCalled_ReturnCountListAssignmentAllNotEqual2()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(2, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignment_WhenCalled_ReturnCountListAssignmentAcceptedNotEqual2()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(2, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignment_WhenCalled_ReturnCountListAssignmentWaitingForAcceptanceNotEqual2()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(2, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByStatesAccepted_WhenCalled_ReturnTwoAssignmentStateAccepted()
        {
            var page = 1;
            var pageSize = 2;
            var keyword = "";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.Single(result.Assignments);
            Assert.Contains(result.Assignments, item => item.State == "Accepted");
        }

        [Fact]
        public async Task GetListAssignmentByStateAccepted_WhenCalled_ReturnTwoAssignmentStateWaitingForAcceptance()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.Single(result.Assignments);
            Assert.DoesNotContain(result.Assignments, item => item.State == "WaitingForAcceptance");
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetCode_WhenCalled_ReturnAssignmentByAssetCode()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "123";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetCode_WhenCalled_ReturnAssignmentByAssetCodeNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "456";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetCodeAndStateAccepted_WhenCalled_ReturnAssignmentByAssetCode()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "123";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetCodeAndStateAccepted_WhenCalled_ReturnAssignmentByAssetCodeNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "456";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetCodeAndStateWaitingForAcceptance_WhenCalled_ReturnAssignmentByAssetCode()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "123";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetCodeAndStateWaitingForAcceptance_WhenCalled_ReturnAssignmentByAssetCodeNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "456";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetName_WhenCalled_ReturnAssignmentByAssetName()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "AssetName1";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetName_WhenCalled_ReturnAssignmentByAssetNameNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "AssetName2";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetNameAndStateAccepted_WhenCalled_ReturnAssignmentByAssetName()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "AssetName1";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetNameAndStateAccepted_WhenCalled_ReturnAssignmentByAssetNameNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "AssetName2";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetNameAndStateWaitingForAcceptance_WhenCalled_ReturnAssignmentByAssetName()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "AssetName1";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssetNameAndStateWaitingForAcceptance_WhenCalled_ReturnAssignmentByAssetNameNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "AssetName2";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedTo_WhenCalled_ReturnAssignmentByAssignedTo()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "456";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedTo_WhenCalled_ReturnAssignmentByAssignedToNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "132";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedToAndStateAccepted_WhenCalled_ReturnAssignmentByAssignedTo()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "456";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedToAndStateAccepted_WhenCalled_ReturnAssignmentByAssignedToNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "132";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedToAndStateWaitingForAcceptance_WhenCalled_ReturnAssignmentByAssignedTo()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "456";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedToAndStateWaitingForAcceptance_WhenCalled_ReturnAssignmentByAssignedToNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "132";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedBy_WhenCalled_ReturnAssignmentByAssignedBy()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "789";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedBy_WhenCalled_ReturnAssignmentByAssignedByNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "132";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedByAndStateAccepted_WhenCalled_ReturnAssignmentByAssignedBy()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "789";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedByAndStateAccepted_WhenCalled_ReturnAssignmentByAssignedByNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "132";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedByAndStateWaitingForAcceptance_WhenCalled_ReturnAssignmentByAssignedBy()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "789";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedByAndStateWaitingForAcceptance_WhenCalled_ReturnAssignmentByAssignedByNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "132";
            DateTime assignedDate = DateTime.MinValue;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(1, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedDate_WhenCalled_ReturnAssignmentByAssignedDate()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.Now;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedDate_WhenCalled_ReturnAssignmentByAssignedDateNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.Now;
            string[] states = new string[] { "All" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(2, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedDateAndStateAccepted_WhenCalled_ReturnAssignmentByAssignedDate()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.Now;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedDateAndStateAccepted_WhenCalled_ReturnAssignmentByAssignedDateNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.Now;
            string[] states = new string[] { "Accepted" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(2, result.Assignments.Count);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedDateAndStateWaitingForAcceptance_WhenCalled_ReturnAssignmentByAssignedDate()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.Now;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotNull(result.Assignments);
        }

        [Fact]
        public async Task GetListAssignmentByKeyWordAssignedDateAndStateWaitingForAcceptance_WhenCalled_ReturnAssignmentByAssignedDateNull()
        {
            var page = 1;
            var pageSize = 5;
            var keyword = "";
            DateTime assignedDate = DateTime.Now;
            string[] states = new string[] { "WaitingForAcceptance" };
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            var result = await service.GetAssignmentListAsync(page, pageSize, keyword, assignedDate, states, sortOrder, sortField);
            Assert.NotEqual(2, result.Assignments.Count);
        }
        [Fact]
        public async Task GetListAssignmentByUserId_WhenCalled_ReturnAssignmetnByUser()
        {
            //Arrange
            var userId = "123";
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            //Act
            var result = await service.GetAssignmentByUserIdAsync(userId, sortOrder, sortField);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetListAssignmentByUserId_WhenCalled_ReturnAssignmetnByUser_SortOderByDescendSordFieldByAssetCode()
        {
            //Arrange
            var userId = "123";
            var sortOrder = "descend";
            var sortField = "assetCode";
            IAssignmentService service = GetSqlLiteAssignmentService();
            //Act
            var result = await service.GetAssignmentByUserIdAsync(userId, sortOrder, sortField);
            var item = result[0].AssetCode;
            //Assert
            Assert.Equal(item, FakeData.AssignmentFakeData.ListAssignmentsSortOderByDescendSordFieldByAssetCode()[0].AssetCode);
        }
        [Fact]
        public async Task GetListAssignmentByUserId_WhenCalled_ReturnAssignmetnByUser_SortOderByAscendSordFieldByAssetCode()
        {
            //Arrange
            var userId = "123";
            var sortOrder = "ascend";
            var sortField = "assetCode";
            IAssignmentService service = GetSqlLiteAssignmentService();
            //Act
            var result = await service.GetAssignmentByUserIdAsync(userId, sortOrder, sortField);
            var item = result[0].Id;
            //Assert
            Assert.Equal(item, 5);
        }
        [Fact]
        public async Task GetListAssignmentByUserId_WhenCalled_ReturnAssignmetnByUser_SortOderByDescendSordFieldByAssetName()
        {
            //Arrange
            var userId = "123";
            var sortOrder = "descend";
            var sortField = "assetName";
            IAssignmentService service = GetSqlLiteAssignmentService();
            //Act
            var result = await service.GetAssignmentByUserIdAsync(userId, sortOrder, sortField);
            //Assert
            Assert.Equal(result[0].Id, FakeData.AssignmentFakeData.ListAssignmentsSortOderByDescendSordFieldByAssetName()[0].Id);
        }
        [Fact]
        public async Task GetListAssignmentByUserId_WhenCalled_ReturnAssignmetnByUser_SortOderByAscendSordFieldByAssetName()
        {
            //Arrange
            var userId = "123";
            var sortOrder = "ascend";
            var sortField = "assetName";
            IAssignmentService service = GetSqlLiteAssignmentService();
            //Act
            var result = await service.GetAssignmentByUserIdAsync(userId, sortOrder, sortField);
            var item = result[0].Id;
            //Assert
            Assert.Equal(5, item);
        }
        [Fact]
        public async Task GetListAssignmentByUserId_WhenCalled_ReturnAssignmetnByUser_SortOderByDescendSordFieldByState()
        {
            //Arrange
            var userId = "123";
            var sortOrder = "descend";
            var sortField = "state";
            IAssignmentService service = GetSqlLiteAssignmentService();
            //Act
            var result = await service.GetAssignmentByUserIdAsync(userId, sortOrder, sortField);
            var item = result[0].State;
            //Assert
            Assert.Equal(item, FakeData.AssignmentFakeData.ListAssignmentsSortOderByDescendSordFieldByState()[0].State);
        }
        [Fact]
        public async Task GetListAssignmentByUserId_WhenCalled_ReturnAssignmetnByUser_SortOderByAscendSordFieldByState()
        {
            //Arrange
            var userId = "123";
            var sortOrder = "ascend";
            var sortField = "state";
            IAssignmentService service = GetSqlLiteAssignmentService();
            //Act
            var result = await service.GetAssignmentByUserIdAsync(userId, sortOrder, sortField);
            var item = result[0].State;
            //Assert
            Assert.Equal(item, FakeData.AssignmentFakeData.ListAssignmentsSortOderByAscendSordFieldByState()[0].State);
        }
        [Fact]
        public async Task GetListAssignmentByUserId_WhenCalled_ReturnAssignmetnByUser_CannotFindUser()
        {
            //Arrange
            var userId = "5551234";
            var sortOrder = "";
            var sortField = "";
            IAssignmentService service = GetSqlLiteAssignmentService();
            //Act
            Func<Task> result = async () => await service.GetAssignmentByUserIdAsync(userId, sortOrder, sortField);
            //Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(result);
            Assert.Equal($"Can not find user with id: {userId}", exception.Message);
        }
        [Fact]
        public async Task Accepted_WhenCalled_AssignmentIsAccepted_WhenAssignmentIsNotFound()
        {
            //Arrange
            var assignmentId = 1221;
            IAssignmentService service = GetSqlLiteAssignmentService();
            //Act
            Func<Task> result = async () => await service.Accepted(assignmentId);
            //Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(result);
            Assert.Equal($"Can not find assignment with id: {assignmentId}", exception.Message);
        }
        [Fact]
        public async Task Accepted_WhenCalled_AssignmentIsAccepted()
        {
            //Arrange
            var assignmentId = 3;
            IAssignmentService service = GetSqlLiteAssignmentService();
            //Act
            var result = await service.Accepted(assignmentId);
            //Assert
            Assert.True(result);
        }

    }
}
