using RookieOnlineAssetManagement.Entities.Enum;
using System;

namespace RookieOnlineAssetManagement.Entities.Dtos.AssignmentService
{
    public class DetailsAssignmentDto
    {
        public int Id {get; set;}
        public string UserId { get; set; }
        public int AssetId { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string Specification { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        public string State { get; set; }
        public string Note { get; set; }
        public string RequestState {get; set;}
        public string RequestedById {get; set;}
    }
}
