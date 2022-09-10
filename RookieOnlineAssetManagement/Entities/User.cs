using Microsoft.AspNetCore.Identity;
using RookieOnlineAssetManagement.Entities.Enum;
using System;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities
{
    public class User : IdentityUser
    {
        public string StaffCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public DateTime JoinedDate { get; set; }
        public Location Location { get; set; }
        public bool Disabled { get; set; } = false;
        public bool FirstLogin { get; set; } = true;
    }
}
