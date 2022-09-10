using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities.Dtos.AssetService
{
    public class AssetsDto
    {
        public List<DetailsAssetDto> Assets { get; set; }
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
        public double NumberPage { get; set; }
        public int? PageSize { get; set; }

        // Khoi tao list de tranh null
        public AssetsDto()
        {
            this.Assets = new List<DetailsAssetDto>();
        }
    }
}
