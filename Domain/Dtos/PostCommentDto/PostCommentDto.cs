namespace Domain.Dtos.PostCommentDto;

public class PostCommentDto
{
    public int PostCommentId { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Comment { get; set; }
}