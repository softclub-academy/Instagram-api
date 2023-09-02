using System.Net;
using Domain.Dtos.PostStatDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.PostStatService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class PostStatController : ControllerBase
{
    private readonly IPostStatService _service;

    public PostStatController(IPostStatService service)
    {
        _service = service;
    }

    [HttpGet("get-PostStats")]
    public async Task<IActionResult> GetPostStats([FromQuery]PaginationFilter filter)
    {
        
        var result = await _service.GetPostStats(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-PostStat")]
    public async Task<IActionResult> AddPostStat([FromBody]string userId, int postId)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.AddPostStat(userId, postId);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<PostStatDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-poststat")]
    public async Task<IActionResult> DeletePostStat(string userId, int postId)
    {
        var result = await _service.DeletePostStat(userId, postId);
        return StatusCode(result.StatusCode, result);
    }
}