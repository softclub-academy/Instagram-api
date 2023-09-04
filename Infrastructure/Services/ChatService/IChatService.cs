using Domain.Dtos.ChatDto;
using Domain.Dtos.MessageDto;
using Domain.Responses;

namespace Infrastructure.Services.ChatService;

public interface IChatService
{
    Task<Response<List<GetChatDto>>> GetChats(string? userId);
    Task<Response<List<GetMessageDto>>> GetChatById(ChatDto chat, string userId);
    Task<Response<int>> SendMessage(MessageDto message, string userId);
    Task<Response<bool>> DeleteMessage(int massageId);
    Task<Response<bool>> DeleteChat(int chatId);
}