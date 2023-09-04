namespace Domain.Entities;

public class StoryStat
{
    public int Id { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public int? StoryId { get; set; }
    public Story Story { get; set; }
}