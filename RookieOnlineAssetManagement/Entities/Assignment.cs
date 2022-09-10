using RookieOnlineAssetManagement.Entities.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RookieOnlineAssetManagement.Entities
{
        public class Assignment
        {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string AdminId { get; set; }
        [ForeignKey("AdminId")]
        public User Admin { get; set; }
        public int AssetId { get; set; }
        [ForeignKey("AssetId")]
        public Asset Asset { get; set; }
        public DateTime AssignedDate { get; set; }
        public AssignmentState State { get; set; }
        public string Note { get; set; }
        public RequestState RequestState { get; set; }
        public DateTime ReturnedDate { get; set; }
        public string RequestedById { get; set; }
        [ForeignKey("RequestedById")]
        public User RequestedByUser { get; set; }
        public string AcceptedById { get; set; }
        [ForeignKey("AcceptedById")]
        public User AcceptedByAdmin { get; set; }
        }
}
