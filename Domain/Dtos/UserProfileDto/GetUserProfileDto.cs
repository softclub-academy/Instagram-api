namespace Domain.Dtos.UserProfileDto;

public class GetUserProfileDto : UserProfileDto
{
    public string Image { get; set; }
    public DateTime DateUpdated { get; set; }
    public string? Gender { get; set; }
    public int SubscribersCount { get; set; }
    public int SubscriptionsCount { get; set; }
}