namespace Domain.Dtos.UserProfileDto;

public class GetUserProfileDto : UserProfileDto
{
    public int UserProfileId { get; set; }
    public string Image { get; set; }
    public DateTime DateUpdated { get; set; }
}