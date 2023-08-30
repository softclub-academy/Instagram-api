using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Dtos.RegisterDto;

public class RegisterDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    public UserType UserType { get; set; }
    // public string AccountStatus { get; set; }
}