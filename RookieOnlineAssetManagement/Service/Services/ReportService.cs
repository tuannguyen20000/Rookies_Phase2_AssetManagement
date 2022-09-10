using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.ReportService;
using RookieOnlineAssetManagement.Entities.Enum;
using RookieOnlineAssetManagement.Service.IServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ReportService(ApplicationDbContext db, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _db = db;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public async Task<ReportDto> GetListReportAsync(int? page, int? pageSize, string sortOrder, string sortField)
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserLoggedIn = await _db.Users.Where(x => x.Id == accountId).FirstOrDefaultAsync();
            var categories = await _db.Categories.ToListAsync();
            var asset = _db.Assets
                .Include(x => x.Category)
                .Where(x => x.Location == currentUserLoggedIn.Location && x.Disabled == false);

            var queryReportDto = categories.GroupJoin(asset, c => c.Id, a => a.CategoryId, (c, a) =>
                {
                    return new DetailReportDto()
                    {
                        Id = c.Id,
                        Category = c.CategoryName,
                        Total = a.Count(),
                        Assigned = a.Where(x => x.State == AssetState.Assigned).Count(),
                        Available = a.Where(x => x.State == AssetState.Available).Count(),
                        NotAvailable = a.Where(x => x.State == AssetState.NotAvailable).Count(),
                        WaitingForRecycling = a.Where(x => x.State == AssetState.WaitingForRecycling).Count(),
                        Recycled = a.Where(x => x.State == AssetState.Recycled).Count()
                    };
                });
            if (queryReportDto != null)
            {
                // SORT CATEGORY
                if (sortOrder == "descend" && sortField == "category")
                {
                    queryReportDto = queryReportDto.OrderByDescending(x => x.Category);
                }
                else if (sortOrder == "ascend" && sortField == "category")
                {
                    queryReportDto = queryReportDto.OrderBy(x => x.Category);
                }
                // SORT TOTAL
                if (sortOrder == "descend" && sortField == "total")
                {
                    queryReportDto = queryReportDto.OrderByDescending(x => x.Total);
                }
                else if (sortOrder == "ascend" && sortField == "total")
                {
                    queryReportDto = queryReportDto.OrderBy(x => x.Total);
                }
                // SORT ASSIGNED
                if (sortOrder == "descend" && sortField == "assigned")
                {
                    queryReportDto = queryReportDto.OrderByDescending(x => x.Assigned);
                }
                else if (sortOrder == "ascend" && sortField == "assigned")
                {
                    queryReportDto = queryReportDto.OrderBy(x => x.Assigned);
                }
                // SORT AVAILABLE
                if (sortOrder == "descend" && sortField == "available")
                {
                    queryReportDto = queryReportDto.OrderByDescending(x => x.Available);
                }
                else if (sortOrder == "ascend" && sortField == "available")
                {
                    queryReportDto = queryReportDto.OrderBy(x => x.Available);
                }
                // SORT NOT AVAILABLE
                if (sortOrder == "descend" && sortField == "notAvailable")
                {
                    queryReportDto = queryReportDto.OrderByDescending(x => x.NotAvailable);
                }
                else if (sortOrder == "ascend" && sortField == "notAvailable")
                {
                    queryReportDto = queryReportDto.OrderBy(x => x.NotAvailable);
                }
                // SORT Waiting For Recycling
                if (sortOrder == "descend" && sortField == "waitingForRecycling")
                {
                    queryReportDto = queryReportDto.OrderByDescending(x => x.WaitingForRecycling);
                }
                else if (sortOrder == "ascend" && sortField == "waitingForRecycling")
                {
                    queryReportDto = queryReportDto.OrderBy(x => x.WaitingForRecycling);
                }
                // SORT RECYCLED
                if (sortOrder == "descend" && sortField == "recycled")
                {
                    queryReportDto = queryReportDto.OrderByDescending(x => x.Recycled);
                }
                else if (sortOrder == "ascend" && sortField == "recycled")
                {
                    queryReportDto = queryReportDto.OrderBy(x => x.Recycled);
                }
                var pageRecords = pageSize ?? 10;
                var pageIndex = page ?? 1;
                var totalPage = queryReportDto.Count();
                var numberPage = Math.Ceiling((float)totalPage / pageRecords);
                var startPage = (pageIndex - 1) * pageRecords;
                if (totalPage > pageRecords)
                    queryReportDto = queryReportDto.Skip(startPage).Take(pageRecords);
                if (pageIndex > numberPage) pageIndex = (int)numberPage;
                var listReportDto = queryReportDto.ToList();
                var reportDto = _mapper.Map<ReportDto>(listReportDto);
                reportDto.TotalItem = totalPage;
                reportDto.NumberPage = numberPage;
                reportDto.CurrentPage = pageIndex;
                reportDto.PageSize = pageRecords;
                return reportDto;
            }
            return null;
        }

        public async Task<List<DetailReportDto>> GetReportsAsync()
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserLoggedIn = await _db.Users.Where(x => x.Id == accountId).FirstOrDefaultAsync();
            var categories = await _db.Categories.ToListAsync();
            var asset = _db.Assets
                                .Include(x => x.Category)
                                .Where(x => x.Location == currentUserLoggedIn.Location && x.Disabled == false);
            var listReportDto = categories.GroupJoin(asset, c => c.Id, a => a.CategoryId, (c, a) =>
            {
                return new DetailReportDto()
                {
                    Category = c.CategoryName,
                    Total = a.Count(),
                    Assigned = a.Where(x => x.State == AssetState.Assigned).Count(),
                    Available = a.Where(x => x.State == AssetState.Available).Count(),
                    NotAvailable = a.Where(x => x.State == AssetState.NotAvailable).Count(),
                    WaitingForRecycling = a.Where(x => x.State == AssetState.WaitingForRecycling).Count(),
                    Recycled = a.Where(x => x.State == AssetState.Recycled).Count()
                };
            }).ToList();
            return listReportDto;
        }
    }
}
