using RookieOnlineAssetManagement.Entities.Enum;
using System;

namespace RookieOnlineAssetManagement.Entities.Dtos.UserService
{
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string StaffCode { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Location { get; set; }
        public bool Disabled { get; set; } = false;
    }
}
