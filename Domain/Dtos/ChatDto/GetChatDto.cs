using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.ChatDto;

public class GetChatDto
{
    public string SendUserId { get; set; } = null!;
    public string? SendUserName { get; set; }
    public string? SendUserImage { get; set; }

    public int ChatId { get; set; }

    public string ReceiveUserId { get; set; } = null!;
    public string? ReceiveUserName { get; set; }
    public string? ReceiveUserImage { get; set; }
}