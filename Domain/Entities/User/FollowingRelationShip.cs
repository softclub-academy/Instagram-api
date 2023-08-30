        namespace Domain.Entities.User;

public class FollowingRelationShip
{
    public int FollowingRelationShipId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int FollowingId { get; set; }
    public User Following { get; set; }
    public DateTime DateFollowed { get; set; }
}