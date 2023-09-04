using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.StoryDtos;

public class AddStoryDto : StoryDto
{
    
    public List<IFormFile> Images { get; set; }
}