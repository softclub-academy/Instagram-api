namespace Domain.Dtos.PostCommentDto;

public class GetPostCommentDto : PostCommentDto
{
    public int CommentId { get; set; }
    public string UserId { get; set; }
    public DateTime DateCommented { get; set; }
}