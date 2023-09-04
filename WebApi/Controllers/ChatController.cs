using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Domain.Dtos.ChatDto;
using Domain.Dtos.MessageDto;
using Domain.Responses;
using Infrastructure.Services.ChatService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ChatController : BaseController
{
    private readonly IChatService _service;

    public ChatController(IChatService service)
    {
        _service = service;
    }

    [HttpGet("get-chats")]
    public async Task<IActionResult> GetChats(string? userId)
    {
        var result = await _service.GetChats(userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-chat-by-id")]
    public async Task<IActionResult> GetChatById(ChatDto chat)
    {
        var result = await _service.GetChatById(chat);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody]MessageDto message)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.SendMessage(message);
            return StatusCode(result.StatusCode, result);
        }
        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<List<MessageDto>>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-message")]
    public async Task<IActionResult> DeleteMessage(int massageId)
    {
        var result = await _service.DeleteMessage(massageId);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpDelete("delete-chat")]
    public async Task<IActionResult> DeleteChat(int chatId)
    {
        var result = await _service.DeleteChat(chatId);
        return StatusCode(result.StatusCode, result);
    }
}