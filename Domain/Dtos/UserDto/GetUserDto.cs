namespace Domain.Dtos.UserDto;

public class GetUserDto : UserDto
{
    public string Id { get; set; }
    public DateTime DateRegistered { get; set; }
    public string Avatar { get; set; }
}