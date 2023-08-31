namespace Domain.Dtos.UserDto;

public class GetUserDto : UserDto
{
    public string Id { get; set; }
    public DateTime DateRegistred { get; set; }
}