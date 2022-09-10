using RookieOnlineAssetManagement.Entities.Dtos.RequestService;
using System;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Service.IServices
{
    public interface IRequestService
    {
        Task<DetailRequestDto> CompleteRequestAsync(int id);
        Task<DetailRequestDto> CancelRequestAsync(int id);
        Task<DetailRequestDto> CreateRequestAsync(int id);
        Task<RequestsDto> GetRequestListAsync(int? page, int? pageSize, string keyword, DateTime returnedDate, string[] states, string sortOrder, string sortField);
    }
}
