namespace Domain.Dtos.PostDto;

public class GetPostDto : PostDto
{
    public int PostId { get; set; }
    public string DatePublished { get; set; }
    public List<string> Images { get; set; }
}