namespace Domain.Filters.FollowingRelationShipFilter;

public class FollowingRelationShipFilter : PaginationFilter
{
    public int? UserId { get; set; }
    public int? FollowingId { get; set; }
}