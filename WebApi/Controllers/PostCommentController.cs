using System.Net;
using Domain.Dtos.PostCommentDto;
using Domain.Filters.PostCommentFilter;
using Domain.Responses;
using Infrastructure.Services.PostCommentService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("post-comment")]
public class PostCommentController : ControllerBase
{
    private readonly IPostCommentService _service;

    public PostCommentController(IPostCommentService service)
    {
        _service = service;
    }

    [HttpGet("get-PostComments")]
    public async Task<IActionResult> GetPostComments([FromQuery]PostCommentFilter filter)
    {
        var result = await _service.GetPostComments(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-PostComment-by-id")]
    public async Task<IActionResult> GetPostCommentById(int id)
    {
        var result = await _service.GetPostCommentById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-PostComment")]
    public async Task<IActionResult> AddPostComment([FromQuery]AddPostCommentDto postComment)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.AddPostComment(postComment);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<PostCommentDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-PostComment")]
    public async Task<IActionResult> DeletePostComment(int id)
    {
        var result = await _service.DeletePostComment(id);
        return StatusCode(result.StatusCode, result);
    }
}