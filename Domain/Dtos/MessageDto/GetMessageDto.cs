namespace Domain.Dtos.MessageDto;

public class GetMessageDto
{
    public string UserId { get; set; } = null!;
    public string? UserName { get; set; }
    public string? UserImage { get; set; }
    public int MessageId { get; set; }
    public int ChatId { get; set; }
    public string? MessageText { get; set; }
    public DateTime SendMassageDate { get; init; }
    public string? File { get; set; }
}