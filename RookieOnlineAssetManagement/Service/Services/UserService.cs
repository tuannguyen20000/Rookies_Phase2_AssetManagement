using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.UserService;
using RookieOnlineAssetManagement.Entities.Enum;
using RookieOnlineAssetManagement.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Service.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<User> _userManager;

        public UserService(ApplicationDbContext db, IMapper mapper, IHttpContextAccessor httpContext,
            UserManager<User> userManager)
        {
            _db = db;
            _mapper = mapper;
            _httpContext = httpContext;
            _userManager = userManager;
        }

        public async Task<UserDetailsDto> GetUserDetailsAsync(string userId)
        {
            // Get user 
            var user = await _db.Users.Where(x => x.Id == userId)
            .Select(x => new User
            {
                Id = x.Id,
                StaffCode = x.StaffCode,
                UserName = x.UserName,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                Gender = x.Gender,
                Location = x.Location,
                JoinedDate = x.JoinedDate,
                Disabled = x.Disabled
            }).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception($"Cannot find user with id: {userId}");
            }
            // Get Role
            var roleOfUser = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (roleOfUser == null)
            {
                throw new Exception($"Cannot find role with userid: {userId}");
            }
            var roleName = await _db.Roles.FirstOrDefaultAsync(x => x.Id == roleOfUser.RoleId);

            var userDetailDto = _mapper.Map<UserDetailsDto>(user);
            userDetailDto.Type = roleName.Name;
            return userDetailDto;
        }

        public async Task<UsersDto> GetUsersListAsync(int? page, int? pageSize, string keyword, string[] types, string sortOrder, string sortField)
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserLoggedIn = await _db.Users.Where(x => x.Id == accountId).FirstOrDefaultAsync();
            if (types.Length == 0)
            {
                types = new string[] { "All" };
            }
            var queryUsersDetailsDto = _db.Users
                .Where(
                    x => x.Location == currentUserLoggedIn.Location && x.Disabled == false
                ).OrderBy(x => x.FirstName + " " + x.LastName)
                .Select(x => new UserDetailsDto
                {
                    Id = x.Id,
                    StaffCode = x.StaffCode,
                    UserName = x.UserName,
                    FullName = x.FirstName + " " + x.LastName,
                    Gender = ((Gender)x.Gender).ToString(),
                    Location = ((Location)x.Location).ToString(),
                    DateOfBirth = x.DateOfBirth,
                    JoinedDate = x.JoinedDate,
                    Disabled = x.Disabled,
                    Type = _db.Roles.FirstOrDefault(r => r.Id == _db.UserRoles.FirstOrDefault(u => u.UserId == x.Id).RoleId).Name
                });
            if (queryUsersDetailsDto != null)
            {
                // SORT STAFF CODE
                if (sortOrder == "descend" && sortField == "staffCode")
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.OrderByDescending(x => x.StaffCode);
                }
                else if (sortOrder == "ascend" && sortField == "staffCode")
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.OrderBy(x => x.StaffCode);
                }

                // SORT FULL NAME
                if (sortOrder == "descend" && sortField == "fullName")
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.OrderByDescending(x => x.FullName);
                }
                else if (sortOrder == "ascend" && sortField == "fullName")
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.OrderBy(x => x.FullName);
                }

                // SORT USER NAME
                if (sortOrder == "descend" && sortField == "userName")
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.OrderByDescending(x => x.UserName);
                }
                else if (sortOrder == "ascend" && sortField == "userName")
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.OrderBy(x => x.UserName);
                }

                // SORT JOINED DATE
                if (sortOrder == "descend" && sortField == "joinedDate")
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.OrderByDescending(x => x.JoinedDate);
                }
                else if (sortOrder == "ascend" && sortField == "joinedDate")
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.OrderBy(x => x.JoinedDate);
                }

                // SORT TYPE
                if (sortOrder == "descend" && sortField == "type")
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.OrderByDescending(x => x.Type);
                }
                else if (sortOrder == "ascend" && sortField == "type")
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.OrderBy(x => x.Type);
                }

                // FILTERS
                if ((types.Length > 0 && !types.Contains("All")))
                {
                    queryUsersDetailsDto = queryUsersDetailsDto.Where(x => types.Contains(x.Type));
                }
                // SEARCH
                if (!string.IsNullOrEmpty(keyword))
                {
                    var normalizeKeyword = keyword.Trim().ToLower();
                    queryUsersDetailsDto = queryUsersDetailsDto.Where(
                        x => x.UserName.Contains(keyword) ||
                        x.UserName.Trim().ToLower().Contains(normalizeKeyword) ||
                        x.StaffCode.Contains(keyword) ||
                        x.StaffCode.Trim().ToLower().Contains(normalizeKeyword) ||
                        x.FullName.Trim().ToLower().Contains(normalizeKeyword) ||
                        x.FullName.Contains(keyword)
                        );
                }
                var pageRecords = pageSize ?? 10;
                var pageIndex = page ?? 1;
                var totalPage = queryUsersDetailsDto.Count();
                var numberPage = Math.Ceiling((float)totalPage / pageRecords);
                var startPage = (pageIndex - 1) * pageRecords;
                if (totalPage > pageRecords)
                    queryUsersDetailsDto = queryUsersDetailsDto.Skip(startPage).Take(pageRecords);
                if (pageIndex > numberPage) pageIndex = (int)numberPage;
                var listUsersDetailsDto = queryUsersDetailsDto.ToList();
                var usersDto = _mapper.Map<UsersDto>(listUsersDetailsDto);
                usersDto.TotalItem = totalPage;
                usersDto.NumberPage = numberPage;
                usersDto.CurrentPage = pageIndex;
                usersDto.PageSize = pageRecords;
                return usersDto;
            }
            return null;
        }

        public async Task<User> CreateUserAsync(UserCreateDto model)
        {
            var accountId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserLoggedIn = await _db.Users.Where(x => x.Id == accountId).FirstOrDefaultAsync();
            string staffCode = GenerateUserCode();
            string username = GenerateUserName(model.FirstName, model.LastName);
            string password = GeneratePassword(username, model.DateOfBirth);

            var user = new User
            {
                UserName = username,
                StaffCode = staffCode,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                JoinedDate = model.JoinedDate,
                Gender = model.Gender,
                Location = currentUserLoggedIn.Location,
            };
            var result = await _userManager.CreateAsync(user, password);
            User users = await _userManager.FindByIdAsync(user.Id);
            var resultRole = await _userManager.AddToRoleAsync(users, model.Type);
            return user;
        }
        public string GenerateUserCode()
        {
            string staffPrefix = "SD";
            var maxUserCode = _db.Users.OrderByDescending(a => a.StaffCode).FirstOrDefault();
            int number = maxUserCode != null ? Convert.ToInt32(maxUserCode.StaffCode.Replace(staffPrefix, "")) + 1 : 1;
            string newUserCode = staffPrefix + number.ToString("D4");
            return newUserCode;
        }
        public string GenerateUserName(string firstName, string lastName)
        {
            StringBuilder username = new StringBuilder();
            username.Append(firstName.Trim().ToLower());
            List<string> words = lastName.Split(' ').ToList();

            foreach (var word in words)
            {
                username.Append(Char.ToLower(word[0]));
            };
            var userCount = _db.Users.Where(s => s.UserName.Contains(username.ToString())).ToList().Count();
            if (userCount > 0)
            {
                username.Append(userCount);
            }
            return username.ToString();
        }
        public string GeneratePassword(string username, DateTime dob)
        {
            StringBuilder password = new StringBuilder();
            password.Append(username);
            password.Append("@");
            string day = dob.ToString("dd");
            string month = dob.Month.ToString("D2");
            string year = dob.ToString("yyyy");
            password.Append(day);
            password.Append(month);
            password.Append(year);

            return password.ToString();
        }
        public async Task<bool> UpdateAsync(UserUpdateDto request)
        {
            var user = await _db.Users.FindAsync(request.id.ToString());
            if (user == null) throw new Exception($"Cannot find user with id: {request.id}");

            var userRole = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var roleOfUser = await _db.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);

            user.DateOfBirth = request.DateOfBirth;
            user.JoinedDate = request.JoinedDate;
            user.Gender = request.Gender;
            var updateRole = await _userManager.RemoveFromRoleAsync(user, roleOfUser.Name);
            var resultRole = await _userManager.AddToRoleAsync(user, request.Type);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DisableUserAsync(string userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception($"Cannot find user with id: {userId}");
            }
            user.Disabled = true;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return true;
        }

        //Check valid assignments.
        public async Task<bool> CheckValidAssignmentsAsync(string userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception($"Cannot find user with id: {userId}");
            }
            var query = await _db.Assignments.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (query == null) return false;
            var check = await _db.Assignments
                .Where(x => (x.UserId == userId) && (x.RequestState != Entities.Enum.RequestState.Completed))
                .FirstOrDefaultAsync();
            if (check == null) return false;
            return true;
        }
        public async Task<UserUpdateDetail> GetUserUpdateDetailsAsync(string userId)
        {
            // Get user 
            var user = await _db.Users.FindAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception($"Cannot find user with id: {userId}");
            }
            //Get role
            var userRole = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userRole == null)
            {
                throw new Exception($"Cannot find role with userid: {userId}");
            }
            var roleOfUser = await _db.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);
            var userVM = new UserUpdateDetail()
            {
                Id = user.Id,
                Type = roleOfUser.Name,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                JoinedDate = user.JoinedDate,
            };
            return userVM;
        }
    }
}
