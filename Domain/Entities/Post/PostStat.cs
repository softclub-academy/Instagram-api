using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Post;

public class PostStat
{
    [Key]




    
    public int PostId { get; set; }
    public int LikeCount { get; set; }
    public Post Post { get; set; }
    public List<StatUserId> StatUserIds { get; set; }
}