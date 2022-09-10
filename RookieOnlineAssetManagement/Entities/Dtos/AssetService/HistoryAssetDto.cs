using System;

namespace RookieOnlineAssetManagement.Entities.Dtos.AssetService
{
    public class HistoryAssetDto
    {
        public DateTime AssignedDate { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public DateTime? ReturnedDate { get; set; }
    }
}
