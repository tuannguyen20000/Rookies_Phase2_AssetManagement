using RookieOnlineAssetManagement.Entities.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RookieOnlineAssetManagement.Entities
{
    public class Asset
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public int CategoryId { get; set; }
        public string Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public AssetState State { get; set; }
        public Location Location { get; set; }
        public bool Disabled { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
