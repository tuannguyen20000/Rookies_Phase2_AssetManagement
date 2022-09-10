using Moq;
using RookieOnlineAssetManagement.Controllers;
using RookieOnlineAssetManagement.Entities.Dtos.AssignmentService;
using RookieOnlineAssetManagement.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace RookieOnlineAssetManagement.UnitTests.Controllers
{
    public class AssignmentControllerTest
    {
        private readonly ITestOutputHelper _output;
        private Mock<IAssignmentService> _mockService;

        public AssignmentControllerTest(ITestOutputHelper output)
        {
            _output = output;
            _mockService = new Mock<IAssignmentService>();
        }

        [Fact]
        public async void CreateAssignment_WhenCallService_ReturnOkResult()
        {
            _mockService.Setup(x => x.CreateAssignmentAsync(It.IsAny<CreateAssignmentDto>())).ReturnsAsync(FakeData.AssignmentFakeData.DetailsAssignment());
            var controller = new AssignmentController(_mockService.Object);
            var result = await controller.CreateAssignment(FakeData.AssignmentFakeData.CreateAssignment());
            Assert.NotNull(result);
        }
    }
}
