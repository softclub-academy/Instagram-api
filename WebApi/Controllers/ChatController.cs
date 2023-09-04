using System.Net;
using System.Security.Claims;
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
    public async Task<IActionResult> GetChats()
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sid").Value;
            var result = await _service.GetChats(userId);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<List<MessageDto>>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-chat-by-id")]
    public async Task<IActionResult> GetChatById(int chatId)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.GetChatById(chatId);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<List<MessageDto>>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("create-chat")]
    public async Task<IActionResult> CreateChat(string resceiveUserId)
    {
        var sendUserId = User.Claims.FirstOrDefault(c => c.Type == "sid").Value;
        var response = await _service.CreateChat(sendUserId, resceiveUserId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody]MessageDto message)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sid").Value;
                var result = await _service.SendMessage(message, userId);
            return StatusCode(result.StatusCode, result);
        }
        
        var response = new Response<List<MessageDto>>(HttpStatusCode.BadRequest, ModelStateErrors());
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