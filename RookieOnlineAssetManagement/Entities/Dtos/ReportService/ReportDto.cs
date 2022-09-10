using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities.Dtos.ReportService
{
    public class ReportDto
    {
        public List<DetailReportDto> Reports { get; set; }
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
        public double NumberPage { get; set; }
        public int? PageSize { get; set; }

        public ReportDto()
        {
            this.Reports = new List<DetailReportDto>();
        }
    }
}
