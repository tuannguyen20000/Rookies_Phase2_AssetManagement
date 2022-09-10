using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.AssignmentService;
using RookieOnlineAssetManagement.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.UnitTests.FakeData
{
    public class AssignmentFakeData
    {
        public static List<Assignment> ListAssignments()
        {
            return new List<Assignment>() {
                new Assignment()
                {
                    Id = 1,
                    AdminId ="789",
                    AssetId = 1,
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.Accepted,
                    RequestState = RequestState.Completed,
                    ReturnedDate = DateTime.Now.Date,
                    Note = "abc",
                    UserId = "123",
                },
                new Assignment()
                {
                    Id = 2,
                    AdminId ="789",
                    AssetId = 2,
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.Accepted,
                    RequestState= RequestState.Completed,
                    ReturnedDate = DateTime.Now.Date,
                    Note = "bcd",
                    UserId = "456",

                },
                new Assignment()
                {
                    Id = 3,
                    AdminId ="789",
                    AssetId = 3,
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.WaitingForAcceptance,
                    Note = "xay",
                    UserId = "789",

                },

                new Assignment()
                {
                    Id = 4,
                    AdminId ="789",
                    AssetId = 4,
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.Accepted,
                    RequestState = RequestState.WaitingForReturning,
                    ReturnedDate = DateTime.Now.Date,
                    Note = "abc",
                    UserId = "789",

                },
                new Assignment()
                {
                    Id = 5,
                    AdminId ="123",
                    AssetId = 5,
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.WaitingForAcceptance,
                    Note = "abc",
                    UserId = "123",
                },
                new Assignment()
                {
                    Id = 6,
                    AdminId ="123",
                    AssetId = 6,
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.WaitingForAcceptance,
                    Note = "abc",
                    UserId = "123",
                },
                new Assignment()
                {
                    Id = 7,
                    AdminId ="123",
                    AssetId = 7,
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.WaitingForAcceptance,
                    Note = "abc",
                    UserId = "123",
                },
            };
        }

        public static CreateAssignmentDto CreateAssignment()
        {
            return new CreateAssignmentDto()
            {
                AssetId = 3,
                AssignedDate = DateTime.Now.Date,
                Note = "xay",
                UserId = "456"
            };
        }

        public static CreateAssignmentDto CreateAssignmentDateNotCurrentOrFuture()
        {
            return new CreateAssignmentDto()
            {
                AssetId = 3,
                AssignedDate = DateTime.Now.AddDays(-1).Date,
                Note = "xay",
                UserId = "456"
            };
        }

        public static UpdateAssignmentDto UpdateAssignment()
        {
            return new UpdateAssignmentDto()
            {
                AssignedDate = DateTime.Now.Date,
                Note = "update sth",
                AssetId = 1,
                UserId = "456",
                AdminId = ""
            };
        }
        public static DetailsAssignmentDto DetailsAssignment()
        {
            return new DetailsAssignmentDto()
            {
                AssignedDate = DateTime.Now.Date,
                Note = "xay",
                AssetCode = "123",
                AssetName = "AssetName1",
                AssignedBy = "789",
                AssignedTo = "456",
                Specification = "no",
                State = "2"
            };
        }
        public static List<DetailsAssignmentDto> ListDetailsAssignmentDtos()
        {
            return new List<DetailsAssignmentDto>() {
                new DetailsAssignmentDto()
                {
                    Id = 1,
                    UserId = "123",
                    AssetId = 1,
                    AssetCode= "CA000001" ,
                    AssetName ="Asset1",
                    Specification = "AssetSpe",
                    AssignedTo ="tuankiet2122",
                    AssignedBy="tuankiet2122",
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.WaitingForAcceptance.ToString(),
                    Note = "abc",
                },
                new DetailsAssignmentDto()
                {
                    Id = 5,
                    UserId = "123",
                    AssetId = 5,
                    AssetCode= "CD000005" ,
                    AssetName ="Asset5",
                    Specification = "AssetSpe",
                    AssignedTo ="tuankiet2122",
                    AssignedBy="tuankiet2122",
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.WaitingForAcceptance.ToString(),
                    Note = "abc",
                },
                new DetailsAssignmentDto()
                {
                    Id = 6,
                    UserId = "123",
                    AssetId = 6,
                    AssetCode= "CD000005" ,
                    AssetName ="Asset6",
                    Specification = "AssetSpe",
                    AssignedTo ="tuankiet2122",
                    AssignedBy="tuankiet2122",
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.WaitingForAcceptance.ToString(),
                    Note = "abc",
                },
                new DetailsAssignmentDto()
                {
                    Id = 7,
                    UserId = "123",
                    AssetId = 7,
                    AssetCode= "CD000007" ,
                    AssetName ="Asset7",
                    Specification = "AssetSpe",
                    AssignedTo ="tuankiet2122",
                    AssignedBy="tuankiet2122",
                    AssignedDate = DateTime.Now.Date,
                    State = AssignmentState.WaitingForAcceptance.ToString(),
                    Note = "abc",
                },
            };
        }
        public static List<DetailsAssignmentDto> ListAssignmentsSortOderByDescendSordFieldByAssetCode()
        {
            return ListDetailsAssignmentDtos().OrderByDescending(x => x.AssetCode).ToList();
        }
        public static List<DetailsAssignmentDto> ListAssignmentsSortOderByAscendSordFieldByAssetCode()
        {
            return ListDetailsAssignmentDtos().OrderBy(x => x.AssetCode).ToList();
        }
        public static List<DetailsAssignmentDto> ListAssignmentsSortOderByDescendSordFieldByAssetName()
        {
            return ListDetailsAssignmentDtos().OrderByDescending(x => x.AssetName).ToList();
        }
        public static List<DetailsAssignmentDto> ListAssignmentsSortOderByAscendSordFieldByAssetName()
        {
            return ListDetailsAssignmentDtos().OrderBy(x => x.AssetName).ToList();
        }
        public static List<DetailsAssignmentDto> ListAssignmentsSortOderByDescendSordFieldByState()
        {
            return ListDetailsAssignmentDtos().OrderByDescending(x => x.State).ToList();
        }
        public static List<DetailsAssignmentDto> ListAssignmentsSortOderByAscendSordFieldByState()
        {
            return ListDetailsAssignmentDtos().OrderBy(x => x.State).ToList();
        }
    }
}
