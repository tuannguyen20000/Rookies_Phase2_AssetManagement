using Microsoft.AspNetCore.Identity;
using RookieOnlineAssetManagement.Data;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Enum;
using System;
using System.Linq;

namespace RookieOnlineAssetManagement.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            var userHCM = new User()
            {
                UserName = "adminhcm",
                NormalizedUserName = "adminhcm",
                Email = "adminhcm@gmail.com",
                NormalizedEmail = "adminhcm@gmail.com",
                FirstName = "Admin",
                LastName = "Ho Chi Minh",
                DateOfBirth = new DateTime(1999, 09, 13),
                StaffCode = "SD0002",
                Disabled = false,
                Location = Location.HoChiMinh,
                Gender = Gender.Male,
                JoinedDate = new DateTime(2022, 07, 28)
            };

            var userHN = new User()
            {
                UserName = "adminhn",
                NormalizedUserName = "adminhn",
                Email = "adminhn@gmail.com",
                NormalizedEmail = "adminhn@gmail.com",
                FirstName = "Admin",
                LastName = "Ha Noi",
                DateOfBirth = new DateTime(1999, 09, 13),
                StaffCode = "SD0001",
                Disabled = false,
                Location = Location.HaNoi,
                Gender = Gender.Male,
                JoinedDate = new DateTime(2022, 07, 28)
            };
            _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Staff")).GetAwaiter().GetResult();

            _userManager.CreateAsync(userHCM, "Admin@hcm123").GetAwaiter().GetResult();
            _userManager.CreateAsync(userHN, "Admin@hn123").GetAwaiter().GetResult();
            var roleAdmin = _roleManager.FindByNameAsync("Admin").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(userHCM, roleAdmin.Name).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(userHN, roleAdmin.Name).GetAwaiter().GetResult();
        }
    }
}
