using System.Net;
using Domain.Dtos.PostDto;
using Domain.Filters.PostFilter;
using Domain.Responses;
using Infrastructure.Services.PostService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class PostController : BaseController
{
    private readonly IPostService _service;

    public PostController(IPostService service)
    {
        _service = service;
    }

    [HttpGet("get-posts")]
    public async Task<IActionResult> GetPosts([FromQuery]PostFilter filter)
    {
        var result = await _service.GetPosts(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-post-by-id")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var result = await _service.GetPostById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-following-post")]
    public async Task<IActionResult> GetFollowingPost([FromQuery]PostFollowingFilter filter)
    {
        var result = await _service.GetPostByFollowing(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-post")]
    public async Task<IActionResult> AddPost([FromForm]AddPostDto post)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.AddPost(post);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<PostDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-post")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var result = await _service.DeletePost(id);
        return StatusCode(result.StatusCode, result);
    }
}