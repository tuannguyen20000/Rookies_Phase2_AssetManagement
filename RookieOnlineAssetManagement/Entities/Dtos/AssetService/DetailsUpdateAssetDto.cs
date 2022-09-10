using RookieOnlineAssetManagement.Entities.Enum;
using System;

namespace RookieOnlineAssetManagement.Entities.Dtos.AssetService
{
    public class DetailsUpdateAssetDto
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public AssetState State { get; set; }
    }
}
