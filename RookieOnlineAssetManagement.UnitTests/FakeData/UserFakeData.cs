using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Entities.Dtos.UserService;
using RookieOnlineAssetManagement.Entities.Enum;

namespace RookieOnlineAssetManagement.UnitTests.FakeData
{
    public class UserFakeData
    {

        public static List<User> ListUser()
        {
            return new List<User>() {
                new User()
                {
                    Id = "123",
                    DateOfBirth = DateTime.Now,
                    FirstName = "Tuan",
                    LastName = "Nguyen",
                    UserName = "tuankiet2122",
                    Gender = Gender.Male,
                    JoinedDate = DateTime.Now,
                    Location = Location.HoChiMinh,
                    StaffCode = "187pm1231",
                },
                new User()
                {
                    Id = "456",
                    DateOfBirth = DateTime.Now,
                    FirstName = "Tuan2",
                    LastName = "Nguyen",
                    UserName = "tuankiet2322",
                    Gender = Gender.Male,
                    JoinedDate = DateTime.Now,
                    Location = Location.HoChiMinh,
                    StaffCode = "187pm1232",
                },
                 new User()
                {
                    Id = "789",
                    DateOfBirth = DateTime.Now,
                    FirstName = "Van A",
                    LastName = "Nguyen",
                    UserName = "tuankiet2106",
                    Gender = Gender.Male,
                    JoinedDate = DateTime.Now,
                    Location = Location.HoChiMinh,
                    StaffCode = "187pm1233",
                },
                  new User()
                {
                    Id = "101112",
                    DateOfBirth = DateTime.Now,
                    FirstName = "Van T",
                    LastName = "Nguyen",
                    UserName = "tuankiet2126",
                    Gender = Gender.Male,
                    JoinedDate = DateTime.Now,
                    Location = Location.HaNoi,
                    StaffCode = "187pm1234",
                },
                   new User()
                {
                    Id = "131415",
                    DateOfBirth = DateTime.Now,
                    FirstName = "Van C",
                    LastName = "Nguyen",
                    UserName = "tuankiet2136",
                    Gender = Gender.Male,
                    JoinedDate = DateTime.Now,
                    Location = Location.HoChiMinh,
                    StaffCode = "187pm12365",
                }
            };
        }
        public static UserCreateDto CreateUser()
        {
            return new UserCreateDto()
            {
                FirstName = "thien",
                LastName = "ho duc",
                DateOfBirth = new DateTime(2000, 11, 11),
                Gender = Gender.Female,
                Location = Location.HoChiMinh,
                JoinedDate = new DateTime(2010, 11, 11),
                Type = "Admin",
            };

        }
        public static User ResUser()
        {
            return new User()
            {
                Id = "161718",
                FirstName = "thien",
                LastName = "ho duc",
                UserName = "thienhd1",
                DateOfBirth = new DateTime(2000, 11, 11),
                Gender = Gender.Male,
                Location = Location.HoChiMinh,
                JoinedDate = new DateTime(2010, 11, 11),
                Disabled = false,
                StaffCode = "SD0001"
            };
        }

        public static List<Role> ListRoles()
        {
            return new List<Role>()
            {
                new Role
                    {
                        Id = "idroleadmin",
                        Name = "Admin",
                        NormalizedName = "admin",
                        ConcurrencyStamp = "idroleadmin1"
                    },
                    new Role
                    {
                        Id = "idrolestaff",
                        Name = "Staff",
                        NormalizedName = "staff",
                        ConcurrencyStamp = "idrolestaff1"
                    }
            };
        }

        public static List<IdentityUserRole<string>> ListUserInRoles()
        {
            return new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                    {
                        RoleId = "idroleadmin",
                        UserId = "123"
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = "idrolestaff",
                        UserId = "456"
                    },
                     new IdentityUserRole<string>
                    {
                        RoleId = "idroleadmin",
                        UserId = "789"
                    },
                      new IdentityUserRole<string>
                    {
                        RoleId = "idrolestaff",
                        UserId = "101112"
                    },
                       new IdentityUserRole<string>
                    {
                        RoleId = "idrolestaff",
                        UserId = "131415"
                    },
                       new IdentityUserRole<string>
                    {
                        RoleId = "idroleadmin",
                        UserId = "161718"
                    }
            };
        }
        public static UserUpdateDto UpdateUserDetail()
        {
            return new UserUpdateDto()
            {
                id = "9999999999999999999",
                DateOfBirth = DateTime.Now,
                Gender = 0,
                JoinedDate = DateTime.Now,
                Type = "Admin",
            };
        }
        public static UserDetailsDto GetUserDetail()
        {
            return new UserDetailsDto()
            {
                Id = "789",
                DateOfBirth = DateTime.Now,
                FullName = "Nguyen Van A",
                UserName = "tuankiet2106",
                Gender = "Male",
                JoinedDate = DateTime.Now,
                Location = nameof(Location.HoChiMinh),
                StaffCode = "187pm1233",
                Type = "Admin",
            };
        }

        public static UserUpdateDto UpdateResUser()
        {
            return new UserUpdateDto()
            {
                id = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                Type = "187pm1231",
                DateOfBirth = DateTime.Now,
                Gender = Gender.Male,
                JoinedDate = DateTime.Now,
            };
        }

        public static UserUpdateDetail UpdateDetail()
        {
            return new UserUpdateDetail()
            {
                Id = "596b46c3-2e5a-4811-8e18-7f35205780bb",
                Type = "Staff",
                FirstName = "thien",
                LastName = "ho duc",
                DateOfBirth = new DateTime(2020, 07, 26, 18, 44, 7),
                Gender = Gender.Male,
                JoinedDate = new DateTime(2022, 07, 26, 18, 44, 7),
            };
        }
    }
}