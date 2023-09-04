namespace Domain.Dtos.MessageDto;

public class GetMessageDto : MessageDto
{
    public string UserId { get; set; }
    public int MessageId { get; set; }
    public DateTime SendMassageDate { get; set; }
}