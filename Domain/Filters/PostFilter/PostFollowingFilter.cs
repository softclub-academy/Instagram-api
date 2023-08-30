namespace Domain.Filters.PostFilter;

public class PostFollowingFilter : PaginationFilter
{
    public int? UserId { get; set; }
}