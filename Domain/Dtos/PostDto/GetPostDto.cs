using Domain.Dtos.PostCommentDto;

namespace Domain.Dtos.PostDto;

public class GetPostDto : PostDto
{
    public int PostId { get; set; }
    public string UserId { get; set; }
    public string DatePublished { get; set; }
    public List<string> Images { get; set; }
    public bool PostLike { get; set; }
    public int PostLikeCount { get; set; }
    public List<string>? UserLikes { get; set; }
    public int CommentCount { get; set; }
    public List<GetPostCommentDto>? Comments { get; set; }
    public int PostView { get; set; }
    public List<string>? UserViews { get; set; }
    public bool PostFavorite { get; set; }
}