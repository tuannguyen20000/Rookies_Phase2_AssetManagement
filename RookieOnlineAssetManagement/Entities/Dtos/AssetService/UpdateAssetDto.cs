using RookieOnlineAssetManagement.Entities.Enum;
using System;

namespace RookieOnlineAssetManagement.Entities.Dtos.AssetService
{
    public class UpdateAssetDto
    {
        public string AssetName { get; set; }
        public string Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public AssetState State { get; set; }
    }
}
