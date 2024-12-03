namespace Domain.Dtos.UserProfileDto;

public class GetUserProfileDto : UserProfileDto
{
    public string UserName { get; set; } = null!;
    public new string? Image { get; set; }
    public DateTime DateUpdated { get; set; }
    public string? Gender { get; set; }
    public int PostCount { get; set; }
    public int SubscribersCount { get; set; }
    public int SubscriptionsCount { get; set; }
}

public class GetIsFollowUserProfileDto : UserProfileDto
{
    public string UserName { get; set; } = null!;
    public new string? Image { get; set; }
    public DateTime DateUpdated { get; set; }
    public string? Gender { get; set; }
    public int PostCount { get; set; }
    public int SubscribersCount { get; set; }
    public int SubscriptionsCount { get; set; }
    public bool IsSubscriber { get; set; }
}