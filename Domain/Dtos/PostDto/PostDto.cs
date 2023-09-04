using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.PostDto;

public class PostDto
{
    public string? UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<IFormFile> Images { get; set; }
    public string Status { get; set; }
}