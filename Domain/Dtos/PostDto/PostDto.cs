using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.PostDto;

public class PostDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public List<IFormFile> Images { get; set; }
}