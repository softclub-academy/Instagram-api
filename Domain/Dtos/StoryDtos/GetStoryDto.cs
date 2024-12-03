using Domain.Dtos.UserDto;
using Domain.Dtos.ViewerDtos;

namespace Domain.Dtos.StoryDtos;

public class GetStoryDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = null!;
    public int? PostId { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = null!;
    public string? UserAvatar { get; set; }
    public ViewerDto? ViewerDto { get; set; }
}

public class GetMyStoryDto
{
    public string UserId { get; set; } = null!;
    public string? UserName { get; set; }
    public string? UserImage { get; set; }
    public List<UserStoryDto> Stories { get; set; } = [];
}
public class UserStoryDto
{
    public int Id { get; set; }
    public string? FileName { get; set; }
    public int? PostId { get; set; }
    public DateTime CreateAt { get; set; }
    public bool Liked { get; set; }
    public int LikedCount { get; set; }
}
