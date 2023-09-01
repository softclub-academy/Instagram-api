using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Post;

public class Post
{
    public int PostId { get; set; }
    public string UserId { get; set; }
    public User.User User { get; set; }
    [MaxLength(45)]
    public string Title { get; set; }
    public string Content { get; set; }
    [MaxLength(45)]
    public string Status { get; set; }
    public DateTime DatePublished { get; set; }
    public List<PostComment> PostComments { get; set; }
    public List<PostFavorite> PostFavorites { get; set; }
    public PostView PostView { get; set; }
    public PostStat PostStat { get; set; }
    public List<PostCategory> PostCategories { get; set; }
    public List<PostTag> PostTags { get; set; }
    public PostView PostView { get; set; }
    public List<Image> Images { get; set; }
}