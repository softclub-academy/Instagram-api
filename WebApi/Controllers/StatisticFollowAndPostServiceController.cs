using System.Net;
using Domain.Dtos.LoginDto;
using Domain.Dtos.RegisterDto;
using Domain.Responses;
using Infrastructure.Services.AccountService;
using Infrastructure.Services.StatisticFollowAndPostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class StatisticFollowAndPostServiceController : ControllerBase
{
    private readonly IStatisticFollowAndPostService _service;

    public StatisticFollowAndPostServiceController(IStatisticFollowAndPostService service)
    {
        _service = service;
    }

    [HttpGet("CountPost")]
    public async Task<ObjectResult> GetCountPost()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
        var result = await _service.GetUserPost(userId);
        return StatusCode(result.StatusCode, result);
    }
    [HttpGet("CountFollowing")]
    public async Task<ObjectResult> CountFollowing()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
        var result = await _service.GetFollowing(userId);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpGet("CountFollowers")]
    public async Task<ObjectResult> CountFollowers()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
        var result = await _service.GetFollowers(userId);
        return StatusCode(result.StatusCode, result);
    }
}