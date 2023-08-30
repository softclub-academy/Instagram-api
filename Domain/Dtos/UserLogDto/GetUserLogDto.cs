namespace Domain.Dtos.UserLogDto;

public class GetUserLogDto : UserLogDto
{
    public int LogId { get; set; }
    public DateTime? LogoutDate { get; set; }
}