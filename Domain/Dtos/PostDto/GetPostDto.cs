namespace Domain.Dtos.PostDto;

public class GetPostDto : PostDto
{
    public int PostId { get; set; }
    public string DatePublished { get; set; }
    public List<string> Images { get; set; }
    public int PostLikeCount { get; set; }
    public int CommentCount { get; set; }
    public int PostView { get; set; }
    public bool PostFavorite { get; set; }

}