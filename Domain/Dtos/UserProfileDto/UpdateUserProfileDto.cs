using Domain.Enums;

namespace Domain.Dtos.UserProfileDto;

public class UpdateUserProfileDto : UserProfileDto
{
    public DateTime DateUpdated { get; set; }
    public Gender? Gender { get; set; }

}