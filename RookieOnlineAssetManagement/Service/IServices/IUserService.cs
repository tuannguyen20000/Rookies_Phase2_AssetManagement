using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.UserService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Service.IServices
{
    public interface IUserService
    {
        Task<UsersDto> GetUsersListAsync(int? page, int? pageSize, string keyword, string[] types, string sortOrder, string sortField);
        Task<UserDetailsDto> GetUserDetailsAsync(string userId);
        Task<User> CreateUserAsync(UserCreateDto user);
        Task<bool> UpdateAsync(UserUpdateDto request);
        Task<bool> DisableUserAsync(string userId);
        Task<bool> CheckValidAssignmentsAsync(string userId);
        Task<UserUpdateDetail> GetUserUpdateDetailsAsync(string userId);


    }
}
