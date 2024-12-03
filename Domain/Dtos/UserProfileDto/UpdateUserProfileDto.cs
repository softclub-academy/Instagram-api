using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.UserProfileDto;

public class UpdateUserProfileDto
{
    public string? About { get; set; }
    public Gender? Gender { get; set; }
}