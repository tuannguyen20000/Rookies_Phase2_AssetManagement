using System;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IList<string> Role { get; set; }
        public bool FirstLogin { get; set; } = true;
    }
}
