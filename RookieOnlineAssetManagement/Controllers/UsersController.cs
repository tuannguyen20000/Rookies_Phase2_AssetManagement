using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.UserService;
using RookieOnlineAssetManagement.Models;
using RookieOnlineAssetManagement.Service.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Controllers
{
    [Authorize(Roles = "Admin, Staff")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UsersController(IUserService userService
            , UserManager<User> userManager
            , SignInManager<User> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("getlist")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetList(int? page, int? pageSize, string keyword, [FromQuery] string[] types, string sortOrder, string sortField)
        {
            var users = await _userService.GetUsersListAsync(page, pageSize, keyword, types, sortOrder, sortField);
            if (users == null) return BadRequest(users);
            return Ok(users);
        }

        [HttpGet]
        [Route("detail/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetDetail(string userId)
        {
            var data = await _userService.GetUserDetailsAsync(userId);
            if (data == null) return BadRequest(data);
            return Ok(data);
        }

        [HttpPost]
        [Route("create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateUser([FromBody] UserCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = await _userService.CreateUserAsync(model);

            return Ok(userId);
        }

        [HttpPut]
        [Route("update/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromForm] UserUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.UpdateAsync(request);

            if (result == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("disable-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Disable(string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.DisableUserAsync(userId);
            if (result == false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("checkValidAssignments/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckValidAssignments(string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.CheckValidAssignmentsAsync(userId);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-user")]
        public async Task<IActionResult> Get()
        {
            var user = await GetCurrentUserAsync();
            var role = await GetCurrentUserRoleAsync(user);
            return Ok(new UserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.PasswordHash,
                Role = role,
                FirstLogin = user.FirstLogin,
            });
        }

        [HttpPost]
        [Route("ChangeFirstPassword")]
        public async Task<IActionResult> ChangeFirstPassword(ChangePasswordRequest changePasswordRequest)
        {
            var user = await _userManager.FindByIdAsync(changePasswordRequest.Id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, changePasswordRequest.NewPassword);

            // Update last login to not trigger change password on first login
            user.FirstLogin = false;
            await _userManager.UpdateAsync(user);
            return Ok();
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [Route("ChangeNewPassword")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByIdAsync(request.Id);
                var checkPass = _userManager.CheckPasswordAsync(user, request.OldPassword);
                if (checkPass.Result == true)
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {
                        return BadRequest();
                    }
                    await _signInManager.RefreshSignInAsync(user);
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Password is incorrect");
                    return BadRequest();
                }
            }

            return BadRequest();
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpGet]
        [Route("update-detail/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetUpdateDetail(string userId)
        {
            var data = await _userService.GetUserUpdateDetailsAsync(userId);
            if (data == null) return BadRequest(data);
            return Ok(data);

        }

        private async Task<IList<string>> GetCurrentUserRoleAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

    }
}
