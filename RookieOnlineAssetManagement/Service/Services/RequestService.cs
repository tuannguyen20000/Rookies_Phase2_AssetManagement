using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.RequestService;
using RookieOnlineAssetManagement.Entities.Enum;
using RookieOnlineAssetManagement.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Service.Services
{
    public class RequestService : IRequestService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public RequestService(ApplicationDbContext db, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _db = db;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<DetailRequestDto> CompleteRequestAsync(int id)
        {
            var request = await GetSingleRequest(id);
            if (request != null)
            {
                var currentUser = await GetCurrentUserAsync();
                var asset = await GetSingleAsset(request.AssetId);
                request.RequestState = RequestState.Completed;
                request.ReturnedDate = DateTime.Now;
                request.AcceptedById = currentUser.Id;
                asset.State = AssetState.Available;
                _db.Update(request);
                _db.Update(asset);
                await _db.SaveChangesAsync();
                return _mapper.Map<DetailRequestDto>(request);
            }
            else return null;
        }

        public async Task<DetailRequestDto> CancelRequestAsync(int id)
        {
            var request = await GetSingleRequest(id);
            if (request != null)
            {
                request.RequestState = 0;
                _db.Update(request);
                await _db.SaveChangesAsync();
                return _mapper.Map<DetailRequestDto>(request);
            }
            else return null;
        }

        private async Task<Asset> GetSingleAsset(int id)
        {
            return await _db.Assets.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        private async Task<Assignment> GetSingleRequest(int id)
        {
            return await _db.Assignments.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _db.Users.Where(x => x.Id == accountId).FirstOrDefaultAsync();
        }

        public async Task<DetailRequestDto> CreateRequestAsync(int id)
        {
            var user = await GetCurrentUserAsync();
            var assignmentInDb = await _db.Assignments.FirstOrDefaultAsync(x => x.Id == id);
            if (assignmentInDb != null)
            {
                assignmentInDb.RequestedById = user.Id;
                assignmentInDb.ReturnedDate = DateTime.Now;
                assignmentInDb.RequestState = RequestState.WaitingForReturning;
                _db.Update(assignmentInDb);
                await _db.SaveChangesAsync();
                return _mapper.Map<DetailRequestDto>(assignmentInDb);
            }
            return null;
        }

        public async Task<RequestsDto> GetRequestListAsync(int? page, int? pageSize, string keyword, DateTime returnedDate, string[] states, string sortOrder, string sortField)
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserLoggedIn = await _db.Users.Where(x => x.Id == accountId).FirstOrDefaultAsync();
            if (states.Length == 0 || states.Contains("undefined"))
            {
                states = new string[] { "All" };
            }
            var requests = _db.Assignments
                .Include(x => x.Asset)
                .Include(x => x.User)
                .Include(x => x.Admin)
                .OrderBy(x => x.Id)
                .Where(x => x.RequestState != 0 && x.Asset.Location == currentUserLoggedIn.Location);
            if (requests != null)
            {
                if (sortOrder == "descend" && sortField == "id")
                {
                    requests = requests.OrderByDescending(x => x.Id);
                }
                if (sortOrder == "descend" && sortField == "assetCode")
                {
                    requests = requests.OrderByDescending(x => x.Asset.AssetCode);
                }
                else if (sortOrder == "ascend" && sortField == "assetCode")
                {
                    requests = requests.OrderBy(x => x.Asset.AssetCode);
                }
                if (sortOrder == "descend" && sortField == "assetName")
                {
                    requests = requests.OrderByDescending(x => x.Asset.AssetName);
                }
                else if (sortOrder == "ascend" && sortField == "assetName")
                {
                    requests = requests.OrderBy(x => x.Asset.AssetName);
                }
                if (sortOrder == "descend" && sortField == "requestedBy")
                {
                    requests = requests.OrderByDescending(x => x.Admin.UserName);
                }
                else if (sortOrder == "ascend" && sortField == "requestedBy")
                {
                    requests = requests.OrderBy(x => x.Admin.UserName);
                }
                if (sortOrder == "descend" && sortField == "assignedDate")
                {
                    requests = requests.OrderByDescending(x => x.AssignedDate);
                }
                else if (sortOrder == "ascend" && sortField == "assignedDate")
                {
                    requests = requests.OrderBy(x => x.AssignedDate);
                }
                if (sortOrder == "descend" && sortField == "returnedDate")
                {
                    requests = requests.OrderByDescending(x => x.ReturnedDate);
                }
                else if (sortOrder == "ascend" && sortField == "returnedDate")
                {
                    requests = requests.OrderBy(x => x.ReturnedDate);
                }
                if (sortOrder == "descend" && sortField == "acceptedBy")
                {
                    requests = requests.OrderByDescending(x => x.Admin.UserName);
                }
                else if (sortOrder == "ascend" && sortField == "acceptedBy")
                {
                    requests = requests.OrderBy(x => x.Admin.UserName);
                }
                if (sortOrder == "descend" && sortField == "requestState")
                {
                    requests = requests.OrderByDescending(x => x.RequestState);
                }
                else if (sortOrder == "ascend" && sortField == "requestState")
                {
                    requests = requests.OrderBy(x => x.RequestState);
                }

                if ((states.Length > 0 && !states.Contains("All")))
                {
                    foreach (var state in states)
                    {
                        RequestState requestState;
                        requests = requests.Where(x => Enum.TryParse(state, out requestState) && requestState == x.RequestState);
                    }
                }
                if (returnedDate != DateTime.MinValue)
                {
                    requests = requests.Where(x => x.ReturnedDate.Date == returnedDate.Date);
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    var normalizeKeyword = keyword.Trim().ToLower();
                    requests = requests.Where(x =>
                        x.Asset.AssetCode.Contains(keyword) ||
                        x.Asset.AssetCode.Trim().ToLower().Contains(normalizeKeyword) ||
                        x.Asset.AssetName.Contains(keyword) ||
                        x.Asset.AssetName.Trim().ToLower().Contains(normalizeKeyword) ||
                        x.Admin.UserName.Contains(keyword) ||
                        x.Admin.UserName.Trim().ToLower().Contains(normalizeKeyword)
                        );
                }
                var _pageSize = pageSize ?? 10;
                var pageIndex = page ?? 1;
                var totalPage = requests.Count();
                var numberPage = Math.Ceiling((float)totalPage / _pageSize);
                var startPage = (pageIndex - 1) * _pageSize;
                if (totalPage > _pageSize)
                    requests = requests.Skip(startPage).Take(_pageSize);
                if (pageIndex > numberPage) pageIndex = (int)numberPage;
                var queryRequestsDetailsDto = _mapper.Map<List<DetailRequestDto>>(requests);
                var requestsDto = _mapper.Map<RequestsDto>(queryRequestsDetailsDto);
                requestsDto.TotalItem = totalPage;
                requestsDto.NumberPage = numberPage;
                requestsDto.CurrentPage = pageIndex;
                requestsDto.PageSize = _pageSize;
                return requestsDto;
            }
            return null;
        }
    }
}
