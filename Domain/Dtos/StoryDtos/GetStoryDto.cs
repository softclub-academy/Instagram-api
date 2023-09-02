namespace Domain.Dtos.StoryDtos;

public class GetStoryDto : StoryDto
{
    public int Id { get; set; }
    public List<string> Images { get; set; }
}