using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.AssetService;
using System.Collections.Generic;
using RookieOnlineAssetManagement.Entities.Dtos.UserService;
using System.Threading.Tasks;
using RookieOnlineAssetManagement.Entities.Dtos.AssignmentService;

namespace RookieOnlineAssetManagement.Service.IServices
{
    public interface IAssetService
    {
        Task<AssetsDto> GetAssetListAvailableAsync(int? page, int? pageSize, string keyword, string sortOrder, string sortField);
        Task<AssetsDto> GetAllAssetsAsync(int? page, int? pageSize, string keyword, string[] states, string[] categories, string sortOrder, string sortField);
        Task<DetailsAssetDto> GetAssetDetailsAsync(int assetId);
        Task<Asset> UpdateAsync(int assetId, UpdateAssetDto asset);
        Task<DetailsUpdateAssetDto> GetUpdateAsync(int assetId);
        Task<bool> DeleteAsync(int assetId);
        Task<string> GetStateAsync(int assetId);
        Task<bool> CheckHistoryAssetAsync(int assetId);
        Task<List<CategoryDto>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(int categoryId);
        Task<string> GenerateAssetCodeAsync(int categoryId);
        Task<Category> CreateCategoryAsync(CategoryDto model);
        Task<Asset> CreateAssetAsync(CreateAssetDto model);
        Task<AssignmentDto> GetHistoryAsync(int assetId);
    }
}
