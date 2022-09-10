using RookieOnlineAssetManagement.Entities.Dtos.AssignmentService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Service.IServices
{
    public interface IAssignmentService
    {
        Task<DetailsAssignmentDto> CreateAssignmentAsync(CreateAssignmentDto request);
        Task<DetailsAssignmentDto> DetailsAssignmentAsync(int Id);
        Task<DetailsAssignmentDto> UpdateAssignmentAsync(int Id, UpdateAssignmentDto request);
        Task<DetailsAssignmentDto> DisableAssignmentAsync(int Id);
        Task<DetailsAssignmentDto> GetDetailsAssignmentByIdAsync(int Id);
        Task<AssignmentDto> GetAssignmentListAsync(int? page, int? pageSize, string keyword,
                                                DateTime assignedDate, string[] states, string sortOrder, string sortField);
        Task<List<DetailsAssignmentDto>> GetAssignmentByUserIdAsync(string id, string sortOrder, string sortField);
        Task<bool> Accepted(int id);
    }
}
