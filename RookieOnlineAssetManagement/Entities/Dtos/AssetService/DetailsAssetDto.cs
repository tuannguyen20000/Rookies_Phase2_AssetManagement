using RookieOnlineAssetManagement.Entities.Enum;
using System;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities.Dtos.AssetService
{
    public class DetailsAssetDto
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string Category { get; set; }
        public DateTime InstalledDate { get; set; }
        public AssetState State { get; set; }
        public RequestState RequestState { get; set; }
        public Location Location { get; set; }
        public string Specification { get; set; }

        //public List<HistoryAssetDto> History { get; set; }
    }
}
