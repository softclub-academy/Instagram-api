using Domain.Enums;

namespace Domain.Dtos.UserDto;

public class UserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public UserType UserType { get; set; }
}