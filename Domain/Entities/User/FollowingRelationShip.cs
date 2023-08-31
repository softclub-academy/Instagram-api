using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.User;

public class FollowingRelationShip
{
    [Key]
    public int FollowingRelationShipId { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string FollowingId { get; set; }
    public User Following { get; set; }
    public DateTime DateFollowed { get; set; }
}