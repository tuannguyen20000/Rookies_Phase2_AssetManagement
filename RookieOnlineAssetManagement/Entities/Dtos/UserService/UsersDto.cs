using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities.Dtos.UserService
{
    public class UsersDto
    {
        public List<UserDetailsDto> Users { get; set; }
        public int TotalItem { get; set; }
        public int CurrentPage { get; set; }
        public double NumberPage { get; set; }
        public int? PageSize { get; set; }
        public UsersDto()
        {
            this.Users = new List<UserDetailsDto>();
        }
    }
}
