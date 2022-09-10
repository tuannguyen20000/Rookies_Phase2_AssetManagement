using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.AssignmentService;
using RookieOnlineAssetManagement.Entities.Enum;
using RookieOnlineAssetManagement.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Service.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        public AssignmentService(ApplicationDbContext db, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _db = db;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public async Task<DetailsAssignmentDto> CreateAssignmentAsync(CreateAssignmentDto request)
        {
            var currentUser = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUser == null)
            {
                throw new Exception("Admin is not loggin or not available now");
            }
            if (request.AssignedDate.Date < DateTime.Now.Date)
            {
                throw new Exception("Assigned Date is only current or future date");
            }
            var assignment = _mapper.Map<Assignment>(request);
            assignment.State = AssignmentState.WaitingForAcceptance;
            assignment.AdminId = currentUser;
            var asset = await _db.Assets.FindAsync(request.AssetId);
            asset.State = AssetState.Assigned;
            _db.Assignments.Add(assignment);
            _db.Assets.Update(asset);
            await _db.SaveChangesAsync();

            var detailAssignmentDto = _mapper.Map<DetailsAssignmentDto>(assignment);
            return detailAssignmentDto;

        }

        public async Task<DetailsAssignmentDto> DetailsAssignmentAsync(int Id)
        {
            var assignment = await _db.Assignments
                                    .Include(a => a.Asset)
                                    .Include(u => u.User)
                                    .Include(a => a.Admin)
                                    .FirstOrDefaultAsync(x => x.Id == Id);
            var assignmentDetailsDto = _mapper.Map<DetailsAssignmentDto>(assignment);
            return assignmentDetailsDto;
        }

        public async Task<DetailsAssignmentDto> DisableAssignmentAsync(int Id)
        {
            var assignmentInDb = await _db.Assignments.FirstOrDefaultAsync(x => x.Id == Id);
            if (assignmentInDb != null)
            {
                var asset = await _db.Assets.FindAsync(assignmentInDb.AssetId);
                asset.State = AssetState.Available;

                _db.Assignments.Remove(assignmentInDb);
                _db.Assets.Update(asset);
                await _db.SaveChangesAsync();

                var assignmentDetailsDto = _mapper.Map<DetailsAssignmentDto>(assignmentInDb);
                return assignmentDetailsDto;
            }
            return null;
        }

        public async Task<DetailsAssignmentDto> UpdateAssignmentAsync(int Id, UpdateAssignmentDto request)
        {
            var adminId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            request.AdminId = adminId;
            var assignmentInDb = await _db.Assignments.FirstOrDefaultAsync(x => x.Id == Id);
            if (assignmentInDb != null)
            {
                var oldAsset = await _db.Assets.FindAsync(assignmentInDb.AssetId);
                oldAsset.State = AssetState.Available;
                _db.Assets.Update(oldAsset);
                await _db.SaveChangesAsync();
                assignmentInDb = _mapper.Map<UpdateAssignmentDto, Assignment>(request, assignmentInDb);
                var newAsset = await _db.Assets.FindAsync(assignmentInDb.AssetId);
                newAsset.State = AssetState.Assigned;
                _db.Assignments.Update(assignmentInDb);

                _db.Assets.Update(newAsset);
                await _db.SaveChangesAsync();
                var assignmentDetailsDto = _mapper.Map<DetailsAssignmentDto>(assignmentInDb);
                return assignmentDetailsDto;
            }
            return null;
        }

        public async Task<DetailsAssignmentDto> GetDetailsAssignmentByIdAsync(int Id)
        {
            var assignment = await _db.Assignments.FirstOrDefaultAsync(x => x.Id == Id);
            var asset = await _db.Assets.FirstOrDefaultAsync(x => x.Id == assignment.AssetId);
            if (asset == null)
            {
                throw new Exception("Asset is not exist!");
            }
            var admin = await _db.Users.FindAsync(assignment.AdminId);
            if (admin == null)
            {
                throw new Exception("Admin is not loggin or not available now");
            }
            var user = await _db.Users.FindAsync(assignment.UserId);
            if (user == null)
            {
                throw new Exception("User is not exits!");
            }
            var assignmentDetailsDto = _mapper.Map<DetailsAssignmentDto>(assignment);
            assignmentDetailsDto.AssetCode = asset.AssetCode;
            assignmentDetailsDto.AssetName = asset.AssetName;
            assignmentDetailsDto.Specification = asset.Specification;
            assignmentDetailsDto.AssignedTo = admin.UserName;
            assignmentDetailsDto.AssignedBy = user.UserName;
            return assignmentDetailsDto;
        }

        public async Task<AssignmentDto> GetAssignmentListAsync(int? page, int? pageSize, string keyword, DateTime assignedDate, string[] states, string sortOrder, string sortField)
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserLoggedIn = await _db.Users.Where(x => x.Id == accountId).FirstOrDefaultAsync();
            if (states.Length == 0)
            {
                states = new string[] { "All" };
            }
            IQueryable<Assignment> assignments = _db.Assignments.Where(x => x.RequestState != RequestState.Completed).Include(x => x.Asset)
                .Include(x => x.User)
                .Include(x => x.Admin)
                .OrderBy(x => x.Id);
            if (assignments != null)
            {
                // SORT ID
                if (sortOrder == "descend" && sortField == "id")
                {
                    assignments = assignments.OrderByDescending(x => x.Id);
                }
                // SORT ASSET CODE
                if (sortOrder == "descend" && sortField == "assetCode")
                {
                    assignments = assignments.OrderByDescending(x => x.Asset.AssetCode);
                }
                else if (sortOrder == "ascend" && sortField == "assetCode")
                {
                    assignments = assignments.OrderBy(x => x.Asset.AssetCode);
                }
                // SORT ASSET NAME
                if (sortOrder == "descend" && sortField == "assetName")
                {
                    assignments = assignments.OrderByDescending(x => x.Asset.AssetName);
                }
                else if (sortOrder == "ascend" && sortField == "assetName")
                {
                    assignments = assignments.OrderBy(x => x.Asset.AssetName);
                }
                // SORT ASSIGNED BY
                if (sortOrder == "descend" && sortField == "assignedBy")
                {
                    assignments = assignments.OrderByDescending(x => x.Admin.UserName);
                }
                else if (sortOrder == "ascend" && sortField == "assignedBy")
                {
                    assignments = assignments.OrderBy(x => x.Admin.UserName);
                }

                // SORT ASSIGNED TO
                if (sortOrder == "descend" && sortField == "assignedTo")
                {
                    assignments = assignments.OrderByDescending(x => x.User.UserName);
                }
                else if (sortOrder == "ascend" && sortField == "assignedTo")
                {
                    assignments = assignments.OrderBy(x => x.User.UserName);
                }

                // SORT ASSIGNED DATE
                if (sortOrder == "descend" && sortField == "assignedDate")
                {
                    assignments = assignments.OrderByDescending(x => x.AssignedDate);
                }
                else if (sortOrder == "ascend" && sortField == "assignedDate")
                {
                    assignments = assignments.OrderBy(x => x.AssignedDate);
                }

                // SORT STATE
                if (sortOrder == "descend" && sortField == "state")
                {
                    assignments = assignments.OrderByDescending(x => x.State);
                }
                else if (sortOrder == "ascend" && sortField == "state")
                {
                    assignments = assignments.OrderBy(x => x.State);
                }
                if ((states.Length > 0 && !states.Contains("All")))
                {
                    foreach (var state in states)
                    {
                        AssignmentState assignmentState;
                        assignments = assignments.Where(x => Enum.TryParse(state, out assignmentState) && assignmentState == x.State);
                    }
                }
                if (assignedDate != DateTime.MinValue)
                {
                    assignments = assignments.Where(x => x.AssignedDate == assignedDate);
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    var normalizeKeyword = keyword.Trim().ToLower();
                    assignments = assignments.Where(x =>
                        x.Asset.AssetCode.Contains(normalizeKeyword) ||
                        x.Asset.AssetCode.Contains(keyword) ||
                        x.Asset.AssetName.Contains(keyword) ||
                        x.Asset.AssetName.Trim().ToLower().Contains(normalizeKeyword) ||
                        x.Admin.UserName.Trim().ToLower().Contains(normalizeKeyword) ||
                        x.Admin.UserName.Contains(keyword) ||
                        x.User.UserName.Contains(keyword) ||
                        x.User.UserName.Trim().ToLower().Contains(normalizeKeyword)
                        );
                }

                var pageRecords = pageSize ?? 10;
                var pageIndex = page ?? 1;
                var totalPage = assignments.Count();
                var startPage = (pageIndex - 1) * pageRecords;
                if (totalPage > pageRecords)
                {
                    assignments = assignments.Skip(startPage).Take(pageRecords);
                }
                var listAssignmentsDetailsDto = _mapper.Map<List<DetailsAssignmentDto>>(assignments);
                var numberPage = Math.Ceiling((float)totalPage / pageRecords);
                if (pageIndex > numberPage) pageIndex = (int)numberPage;
                var assignmentsDto = _mapper.Map<AssignmentDto>(listAssignmentsDetailsDto);
                assignmentsDto.TotalItem = totalPage;
                assignmentsDto.NumberPage = numberPage;
                assignmentsDto.CurrentPage = pageIndex;
                assignmentsDto.PageSize = pageRecords;
                return assignmentsDto;
            }
            return null;
        }

        public async Task<List<DetailsAssignmentDto>> GetAssignmentByUserIdAsync(string id, string sortOrder, string sortField)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) throw new Exception($"Can not find user with id: {id}");
            var query = from a in _db.Assignments
                        join u in _db.Users on a.UserId equals u.Id
                        join asset in _db.Assets on a.AssetId equals asset.Id
                        where u.Id == id && a.AssignedDate <= DateTime.Now.Date && a.RequestState != RequestState.Completed
                        select new { a, u, asset };

            var assignments = await query.Select(x => new DetailsAssignmentDto()
            {
                Id = x.a.Id,
                UserId = x.a.UserId,
                AssetId = x.a.AssetId,
                AssetCode = x.asset.AssetCode,
                AssetName = x.asset.AssetName,
                Specification = x.asset.Specification,
                AssignedTo = x.u.UserName,
                AssignedBy = x.a.Admin.UserName,
                AssignedDate = x.a.AssignedDate,
                State = x.a.State.ToString(),
                Note = x.a.Note,
                RequestState = x.a.RequestState.ToString(),
                RequestedById = x.a.RequestedById,
            }).ToListAsync();
            // SORT ASSET CODE
            if (assignments != null)
            {
                if (sortOrder == "descend" && sortField == "assetCode")
                {
                    assignments = assignments.OrderByDescending(x => x.AssetCode).ToList();
                }
                else if (sortOrder == "ascend" && sortField == "assetCode")
                {
                    assignments = assignments.OrderBy(x => x.AssetCode).ToList();
                }
                // SORT ASSET NAME
                if (sortOrder == "descend" && sortField == "assetName")
                {
                    assignments = assignments.OrderByDescending(x => x.AssetName).ToList();
                }
                else if (sortOrder == "ascend" && sortField == "assetName")
                {
                    assignments = assignments.OrderBy(x => x.AssetName).ToList();
                }
                // SORT STATE
                if (sortOrder == "descend" && sortField == "state")
                {
                    assignments = assignments.OrderByDescending(x => x.State).ToList();
                }
                else if (sortOrder == "ascend" && sortField == "state")
                {
                    assignments = assignments.OrderBy(x => x.State).ToList();
                }
                // SORT ASSIGNED BY
                if (sortOrder == "descend" && sortField == "assignedBy")
                {
                    assignments = assignments.OrderByDescending(x => x.AssignedBy).ToList();
                }
                else if (sortOrder == "ascend" && sortField == "assignedBy")
                {
                    assignments = assignments.OrderBy(x => x.AssignedBy).ToList();
                }
                // SORT ASSIGNED TO
                if (sortOrder == "descend" && sortField == "assignedTo")
                {
                    assignments = assignments.OrderByDescending(x => x.AssignedTo).ToList();
                }
                else if (sortOrder == "ascend" && sortField == "assignedTo")
                {
                    assignments = assignments.OrderBy(x => x.AssignedTo).ToList();
                }

                // SORT ASSIGNED DATE
                if (sortOrder == "descend" && sortField == "assignedDate")
                {
                    assignments = assignments.OrderByDescending(x => x.AssignedDate).ToList();
                }
                else if (sortOrder == "ascend" && sortField == "assignedDate")
                {
                    assignments = assignments.OrderBy(x => x.AssignedDate).ToList();
                }
                return assignments;
            }
            return null;
        }

        public async Task<bool> Accepted(int id)
        {
            var assignment = await _db.Assignments.FindAsync(id);
            if (assignment == null) throw new Exception($"Can not find assignment with id: {id}");
            assignment.State = AssignmentState.Accepted;
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
