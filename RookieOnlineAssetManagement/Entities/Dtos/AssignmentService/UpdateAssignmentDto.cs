using RookieOnlineAssetManagement.Entities.Enum;
using System;

namespace RookieOnlineAssetManagement.Entities.Dtos.AssignmentService
{
    public class UpdateAssignmentDto
    {
        public string UserId { get; set; }
        public string AdminId { get; set; }
        public int AssetId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string Note { get; set; }
    }
}
