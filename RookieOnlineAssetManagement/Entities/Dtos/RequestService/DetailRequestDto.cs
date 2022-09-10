using System;

namespace RookieOnlineAssetManagement.Entities.Dtos.RequestService
{
    public class DetailRequestDto
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string RequestedBy { get; set; }
        public string AcceptedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        public string RequestState { get; set; }
    }
}
