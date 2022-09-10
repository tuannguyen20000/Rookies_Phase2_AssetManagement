using RookieOnlineAssetManagement.Entities.Enum;
using System;

namespace RookieOnlineAssetManagement.Entities.Dtos.UserService
{
    public class UserUpdateDto
    {
        public string id { get; set; }
        public string StaffCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Type { get; set; }
    }
}
