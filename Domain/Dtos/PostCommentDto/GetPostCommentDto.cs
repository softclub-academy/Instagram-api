namespace Domain.Dtos.PostCommentDto;

public class GetPostCommentDto : PostCommentDto
{
    public int PostCommentId { get; set; }
    public DateTime DateCommented { get; set; }
}