using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.User;

[Index("UserId", "FollowingId", IsUnique = true)]
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