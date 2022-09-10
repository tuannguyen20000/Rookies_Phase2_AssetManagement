using System;

namespace RookieOnlineAssetManagement.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryPrefix { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
