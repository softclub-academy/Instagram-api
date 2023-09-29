using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.User;

// [Index("FollowingRelationShipId", IsUnique = true)]
[PrimaryKey("FollowingRelationShipId")]
public class FollowingRelationShip
{
    public int FollowingRelationShipId { get; set; }
    [Key, Column(Order = 1)]
    public string UserId { get; set; }
    public User User { get; set; }
    [Key, Column(Order = 2)]
    public string FollowingId { get; set; }
    public User Following { get; set; }
    public DateTime DateFollowed { get; set; }
}