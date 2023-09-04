using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Post;

public class PostUserLike
{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public User.User User { get; set; }
    public int PostLikeId { get; set; }
    public PostLike PostLike { get; set; }
}