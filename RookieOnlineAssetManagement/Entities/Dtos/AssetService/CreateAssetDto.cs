using RookieOnlineAssetManagement.Entities.Enum;
using System;

namespace RookieOnlineAssetManagement.Entities.Dtos.AssetService
{
    public class CreateAssetDto
    {
        public string AssetCode { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public int CategoryId { get; set; } = 0;
        public string Specification { get; set; } = string.Empty;
        public DateTime InstalledDate { get; set; } = new DateTime();
        public AssetState State { get; set; } = 0;
    }
}
