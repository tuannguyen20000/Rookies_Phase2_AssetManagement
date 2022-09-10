using System.Collections.Generic;
using System.Threading.Tasks;
using RookieOnlineAssetManagement.Entities.Dtos.ReportService;

namespace RookieOnlineAssetManagement.Service.IServices
{
    public interface IReportService
    {
        Task<ReportDto> GetListReportAsync(int? page, int? pageSize, string sortOrder, string sortField);

        Task<List<DetailReportDto>> GetReportsAsync();
    }
}
