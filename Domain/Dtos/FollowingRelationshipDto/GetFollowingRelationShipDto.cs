namespace Domain.Dtos.FollowingRelationshipDto;

public class GetFollowingRelationShipDto : FollowingRelationShipDto
{
    public int FollowingRelationShipId { get; set; }
    public string UserId { get; set; }
    public DateTime DateFollowed { get; set; }
}