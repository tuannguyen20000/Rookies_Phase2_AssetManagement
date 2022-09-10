using RookieOnlineAssetManagement.Entities.Enum;
using System;

namespace RookieOnlineAssetManagement.Entities.Dtos.UserService
{
    public class UserCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Location Location { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Type { get; set; }
    }
}
