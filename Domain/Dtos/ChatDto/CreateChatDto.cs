using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.Dtos.ChatDto;

public class CreateChatDto
{
    public string SendUserId { get; set; }
    public string ReceiveUserId { get; set; }
}