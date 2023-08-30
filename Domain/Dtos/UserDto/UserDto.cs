namespace Domain.Dtos.UserDto;

public class UserDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public string UserType { get; set; }
    public string AccountStatus { get; set; }
}