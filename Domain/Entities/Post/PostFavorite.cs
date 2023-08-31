namespace Domain.Entities.Post;

public class PostFavorite
{
    public int PostFavoriteId { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
    public string UserId { get; set; }
    public User.User User { get; set; }
    public DateTime DateFavorited { get; set; }
}