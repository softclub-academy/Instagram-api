namespace Domain.Dtos.UserDto;

public class GetUserDto : UserDto
{
    public int UserId { get; set; }
    public DateTime DateRegistred { get; set; }
}