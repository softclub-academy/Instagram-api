namespace Domain.Dtos.UserDto;

public class GetUserShortInfoDto
{
    public string UserId { get; set; } = null!;
    public string? UserName { get; set; }
    public string? UserPhoto { get; set; }
    public string? Fullname { get; set; }
}