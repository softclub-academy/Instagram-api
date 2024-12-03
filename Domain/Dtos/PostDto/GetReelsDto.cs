namespace Domain.Dtos.PostDto;

public class GetReelsDto : GetPostDto
{
    public new string? UserName { get; set; }
    public bool IsSubscriber { get; set; }
    public new string? UserImage { get; set; }
    public new string? Images { get; set; }
}