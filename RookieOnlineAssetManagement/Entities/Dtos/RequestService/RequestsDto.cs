using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities.Dtos.RequestService
{
    public class RequestsDto
    {
        public List<DetailRequestDto> Requests { get; set; }
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
        public double NumberPage { get; set; }
        public int? PageSize { get; set; }
        public RequestsDto()
        {
            this.Requests = new List<DetailRequestDto>();
        }
    }
}
