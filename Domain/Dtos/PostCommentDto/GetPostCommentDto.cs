namespace Domain.Dtos.PostCommentDto;

public class GetPostCommentDto : PostCommentDto
{
    public int PostCommentId { get; set; }
    public string UserId { get; set; } = null!;
    public string? UserName { get; set; }
    public string? UserImage { get; set; }
    public DateTime DateCommented { get; set; }
}