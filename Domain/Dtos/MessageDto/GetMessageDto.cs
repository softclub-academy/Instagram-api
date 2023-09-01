namespace Domain.Dtos.MessageDto;

public class GetMessageDto : MessageDto
{
    public int MessageId { get; set; }
    public DateTime SendMassageDate { get; set; }
}