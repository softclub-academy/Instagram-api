using Domain.Dtos.ChatDto;
using Domain.Dtos.MessageDto;
using Domain.Responses;

namespace Infrastructure.Services.ChatService;

public interface IChatService
{
    Task<Response<List<ChatDto>>> GetChats(string? userId);
    Task<Response<List<GetMessageDto>>> GetChatById(ChatDto chat);
    Task<Response<int>> SendMessage(MessageDto message);
    Task<Response<bool>> DeleteMessage(int massageId);
}