using RookieOnlineAssetManagement.Entities.Dtos.AssetService;
using RookieOnlineAssetManagement.Entities.Enum;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Service.IServices;
using RookieOnlineAssetManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using System;
using AutoMapper;
using RookieOnlineAssetManagement.Entities.Dtos.AssignmentService;
using System.Collections;

namespace RookieOnlineAssetManagement.Service.Services
{
    public class AssetService : IAssetService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public AssetService(ApplicationDbContext db, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _db = db;
            _httpContext = httpContext;
            _mapper = mapper;
        }
        public async Task<string> GetStateAsync(int assetId)
        {
            var asset = await _db.Assets.FindAsync(assetId);
            return asset.State.ToString();
        }

        public async Task<bool> DeleteAsync(int assetId)
        {
            var asset = await _db.Assets.FindAsync(assetId);
            if (asset == null) throw new Exception($"Can not find asset with id: {assetId}");
            var checkHistory = await CheckHistoryAssetAsync(assetId);
            if (checkHistory) return false;
            asset.Disabled = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<DetailsUpdateAssetDto> GetUpdateAsync(int assetId)
        {
            var asset = await _db.Assets.FindAsync(assetId);
            if (asset == null) throw new Exception($"Can not find asset with id: {assetId}");
            var category = await GetCategoryAsync(asset.CategoryId);
            var assetDto = new DetailsUpdateAssetDto()
            {
                Id = asset.Id,
                AssetName = asset.AssetName,
                CategoryId = asset.CategoryId,
                CategoryName = category.CategoryName,
                InstalledDate = asset.InstalledDate,
                Specification = asset.Specification,
                State = asset.State
            };
            return assetDto;
        }

        public async Task<Asset> UpdateAsync(int assetId, UpdateAssetDto asset)
        {
            var assetDto = await _db.Assets.FindAsync(assetId);
            if (assetDto == null) throw new Exception($"Can not find asset with id: {assetId}");
            assetDto.AssetName = asset.AssetName;
            assetDto.Specification = asset.Specification;
            assetDto.InstalledDate = asset.InstalledDate;
            assetDto.State = asset.State;
            await _db.SaveChangesAsync();
            return assetDto;
        }

        public async Task<bool> CheckHistoryAssetAsync(int assetId)
        {
            var checkValidAsset = await _db.Assets.FindAsync(assetId);
            if (checkValidAsset == null) throw new Exception($"Can not find asset with id: {assetId}");
            var check = await _db.Assignments.Where(x => x.AssetId == assetId).FirstOrDefaultAsync();
            if (check == null) return false;
            return true;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _db.Categories.ToListAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
            return categoriesDto;
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            var category = await _db.Categories.Where(x => x.Id == categoryId).FirstOrDefaultAsync();
            if (category == null) throw new Exception($"Can not find category with id: {categoryId}");
            return category;
        }

        public async Task<Category> CreateCategoryAsync(CategoryDto model)
        {
            if (model.CategoryName == string.Empty || model.CategoryPrefix == string.Empty)
                throw new Exception("Input category is null");
            var category = _mapper.Map<Category>(model);
            category.CreatedDate = DateTime.Now;
            _db.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<Asset> CreateAssetAsync(CreateAssetDto model)
        {
            if (model.AssetName == string.Empty || model.Specification == string.Empty ||
                model.CategoryId == 0 || model.InstalledDate == new DateTime() || model.State == 0)
                throw new Exception("Input asset is null");
            var currentUser = await GetCurrentUserAsync();
            var assetCode = await GenerateAssetCodeAsync(model.CategoryId);
            var asset = _mapper.Map<Asset>(model);
            asset.Location = currentUser.Location;
            asset.AssetCode = assetCode;
            await _db.AddAsync(asset);
            await _db.SaveChangesAsync();
            return asset;
        }

        public async Task<string> GenerateAssetCodeAsync(int categoryId)
        {
            var category = await GetCategoryAsync(categoryId);
            string assetPrefix = category.CategoryPrefix;
            var maxAssetCode = _db.Assets.Where(a => a.AssetCode.StartsWith(assetPrefix)).OrderByDescending(a => a.AssetCode).FirstOrDefault();
            int number = maxAssetCode != null ? Convert.ToInt32(maxAssetCode.AssetCode.Replace(assetPrefix, "")) + 1 : 1;
            string newAssetCode = assetPrefix + number.ToString("D6");
            return newAssetCode;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _db.Users.Where(x => x.Id == accountId).FirstOrDefaultAsync();
        }


        public async Task<DetailsAssetDto> GetAssetDetailsAsync(int assetId)
        {
            var asset = await _db.Assets
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == assetId);
            if (asset == null)
            {
                return null;
            }
            var detailsAssetDto = _mapper.Map<DetailsAssetDto>(asset);
            return detailsAssetDto;
        }

        public async Task<AssetsDto> GetAllAssetsAsync(int? page, int? pageSize, string keyword, string[] states, string[] categories, string sortOrder, string sortField)
        {
            if (sortOrder == "undefined" && sortField == "undefined")
            {
                sortOrder = "ascend";
                sortField = "assetName";
            }
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserLoggedIn = await _db.Users.Where(x => x.Id == accountId).FirstOrDefaultAsync();
            if (states.Length == 0)
            {
                states = new string[] { AssetState.Available.ToString(), AssetState.NotAvailable.ToString(), AssetState.Assigned.ToString() };
            }
            var assetStates = states
                              .Select(s => (AssetState)Enum.Parse(typeof(AssetState), s))
                              .ToList();
            if (categories.Length == 0)
            {
                categories = new string[] { "All" };
            }
            //khoi tao
            var queryDetailsAssetDto = _db.Assets
                .Include(x => x.Category)
                .Where(x => x.Location == currentUserLoggedIn.Location && x.Disabled == false)
                .OrderBy(x => x.Id)
                .Select(x => new DetailsAssetDto
                {
                    Id = x.Id,
                    AssetCode = x.AssetCode,
                    AssetName = x.AssetName,
                    Category = x.Category.CategoryName,
                    InstalledDate = x.InstalledDate,
                    Location = x.Location,
                    Specification = x.Specification,
                    State = x.State
                });
            if (queryDetailsAssetDto != null)
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    var normalizeKeyword = keyword.ToLower();
                    queryDetailsAssetDto = queryDetailsAssetDto.Where(x =>
                        x.AssetCode.Contains(normalizeKeyword)
                        || x.AssetCode.Contains(keyword)
                        || x.AssetName.Contains(normalizeKeyword)
                        || x.AssetName.Contains(keyword)
                        );
                }
                if (states.Contains("All"))
                {
                    if (!categories.Contains("All"))
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.Where(x => categories.Contains(x.Category));
                    }
                }
                else
                {
                    if (categories.Contains("All"))
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.Where(x => assetStates.Contains(x.State));
                    }
                    else
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.Where(x => categories.Contains(x.Category) && assetStates.Contains(x.State));
                    }
                    // SORT ASSET ID
                    if (sortOrder == "descend" && sortField == "id")
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.OrderByDescending(x => x.Id);
                    }
                    // SORT ASSET CODE
                    if (sortOrder == "descend" && sortField == "assetCode")
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.OrderByDescending(x => x.AssetCode);
                    }
                    else if (sortOrder == "ascend" && sortField == "assetCode")
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.OrderBy(x => x.AssetCode);
                    }
                    // SORT ASSET NAME
                    if (sortOrder == "descend" && sortField == "assetName")
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.OrderByDescending(x => x.AssetName);
                    }
                    else if (sortOrder == "ascend" && sortField == "assetName")
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.OrderBy(x => x.AssetName);
                    }
                    // SORT CATEGORIES
                    if (sortOrder == "descend" && sortField == "category")
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.OrderByDescending(x => x.Category);
                    }
                    else if (sortOrder == "ascend" && sortField == "category")
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.OrderBy(x => x.Category);
                    }
                    // SORT STATE
                    if (sortOrder == "descend" && sortField == "state")
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.OrderByDescending(x => x.State);
                    }
                    else if (sortOrder == "ascend" && sortField == "state")
                    {
                        queryDetailsAssetDto = queryDetailsAssetDto.OrderBy(x => x.State);
                    }
                }
                var pageRecords = pageSize ?? 10;
                var pageIndex = page ?? 1;
                int totalPage = queryDetailsAssetDto.Count();
                var numberPage = Math.Ceiling((float)totalPage / pageRecords);
                var startPage = (pageIndex - 1) * pageRecords;
                if (totalPage > pageRecords)
                    queryDetailsAssetDto = queryDetailsAssetDto.Skip(startPage).Take(pageRecords);
                if (pageIndex > numberPage) pageIndex = (int)numberPage;
                var listDetailsAssetDto = queryDetailsAssetDto.ToList();
                var assetDto = _mapper.Map<AssetsDto>(listDetailsAssetDto);
                assetDto.TotalItem = totalPage;
                assetDto.NumberPage = numberPage;
                assetDto.CurrentPage = pageIndex;
                assetDto.PageSize = pageRecords;
                return assetDto;
            }
            return null;
        }


        public async Task<AssetsDto> GetAssetListAvailableAsync(int? page, int? pageSize, string keyword, string sortOrder, string sortField)
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserLoggedIn = await _db.Users.Where(x => x.Id == accountId).FirstOrDefaultAsync();
            var queryDetailsAssetDto = _db.Assets.Include(x => x.Category)
                .Where(x => x.State == AssetState.Available && x.Location == currentUserLoggedIn.Location && x.Disabled == false)
                .OrderBy(x => x.AssetName)
                .Select(x => new DetailsAssetDto
                {
                    Id = x.Id,
                    AssetCode = x.AssetCode,
                    AssetName = x.AssetName,
                    Category = x.Category.CategoryName,
                    InstalledDate = x.InstalledDate,
                    Location = x.Location,
                    Specification = x.Specification,
                    State = x.State
                });
            if (queryDetailsAssetDto != null)
            {
                // SORT ASSET CODE
                if (sortOrder == "descend" && sortField == "assetCode")
                {
                    queryDetailsAssetDto = queryDetailsAssetDto.OrderByDescending(x => x.AssetCode);
                }
                else if (sortOrder == "ascend" && sortField == "assetCode")
                {
                    queryDetailsAssetDto = queryDetailsAssetDto.OrderBy(x => x.AssetCode);
                }
                // SORT ASSET NAME
                if (sortOrder == "descend" && sortField == "assetName")
                {
                    queryDetailsAssetDto = queryDetailsAssetDto.OrderByDescending(x => x.AssetName);
                }
                else if (sortOrder == "ascend" && sortField == "assetName")
                {
                    queryDetailsAssetDto = queryDetailsAssetDto.OrderBy(x => x.AssetName);
                }
                // SORT CATEGORIES
                if (sortOrder == "descend" && sortField == "categoryName")
                {
                    queryDetailsAssetDto = queryDetailsAssetDto.OrderByDescending(x => x.Category);
                }
                else if (sortOrder == "ascend" && sortField == "categoryName")
                {
                    queryDetailsAssetDto = queryDetailsAssetDto.OrderBy(x => x.Category);
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    var normalizeKeyword = keyword.Trim().ToLower();
                    queryDetailsAssetDto = queryDetailsAssetDto.Where(x =>
                        x.AssetCode.Trim().ToLower().Contains(normalizeKeyword) ||
                        x.AssetCode.Contains(keyword) ||
                        x.AssetName.Trim().ToLower().Contains(normalizeKeyword)
                        || x.AssetName.Contains(keyword)
                        );
                }
                var pageRecords = pageSize ?? 10;
                var pageIndex = page ?? 1;
                var totalPage = queryDetailsAssetDto.Count();
                var numberPage = Math.Ceiling((float)totalPage / pageRecords);
                var startPage = (pageIndex - 1) * pageRecords;
                if (totalPage > pageRecords)
                    queryDetailsAssetDto = queryDetailsAssetDto.Skip(startPage).Take(pageRecords);
                if (pageIndex > numberPage) pageIndex = (int)numberPage;
                var listAssetDto = queryDetailsAssetDto.ToList();
                var assetDto = _mapper.Map<AssetsDto>(listAssetDto);
                assetDto.TotalItem = totalPage;
                assetDto.NumberPage = numberPage;
                assetDto.CurrentPage = pageIndex;
                assetDto.PageSize = pageRecords;
                return assetDto;
            }
            return null;
        }

        public async Task<AssignmentDto> GetHistoryAsync(int assetId)
        {
            var assignments = await _db.Assignments.Include(x => x.Asset)
          .Include(x => x.User)
          .Include(x => x.Admin)
          .Where(x => x.AssetId == assetId)
          .Select(x => new Assignment()
          {
              AdminId = x.AdminId,
              AssignedDate = x.AssignedDate,
              Id = x.Id,
              State = x.State,
              Admin = x.Admin,
              User = x.User,
              Asset = x.Asset,
              Note = x.Note,
              ReturnedDate = x.ReturnedDate,
              RequestState = x.RequestState,
          }).ToListAsync();
            if (assignments != null)
            {
                var listAssignmentsDetailsDto = _mapper.Map<List<DetailsAssignmentDto>>(assignments);
                var assignmentsDto = _mapper.Map<AssignmentDto>(listAssignmentsDetailsDto);
                return assignmentsDto;
            }
            return null;
        }
    }
}
