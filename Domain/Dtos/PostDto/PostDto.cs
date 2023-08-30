using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.PostDto;

public class PostDto
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<IFormFile> Images { get; set; }
    public string Status { get; set; }
}