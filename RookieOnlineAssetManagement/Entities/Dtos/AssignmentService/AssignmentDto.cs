using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities.Dtos.AssignmentService
{
    public class AssignmentDto
    {
        public List<DetailsAssignmentDto> Assignments { get; set; }
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
        public double NumberPage { get; set; }
        public int? PageSize { get; set; }
        public AssignmentDto()
        {
            this.Assignments = new List<DetailsAssignmentDto>();
        }
    }
}
