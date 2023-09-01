using System.Net;
using Domain.Dtos.PostTagDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.PostTagService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class PostTagController : ControllerBase
{
    private readonly IPostTagService _service;

    public PostTagController(IPostTagService service)
    {
        _service = service;
    }

    [HttpGet("get-posttags")]
    public async Task<IActionResult> GetPostTags([FromQuery]PaginationFilter filter)
    {
        var result = await _service.GetPostTags(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-PostTag-by-id")]
    public async Task<IActionResult> GetPostTagById(int id)
    {
        var result = await _service.GetPostTagById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-PostTag")]
    public async Task<IActionResult> AddPostTag([FromBody]PostTagDto postTag)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.AddPostTag(postTag);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<PostTagDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("update-PostTag")]
    public async Task<IActionResult> UpdatePostTag([FromBody]UpdatePostTagDto postTag)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.UpdatePostTag(postTag);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<PostTagDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-PostTag")]
    public async Task<IActionResult> DeletePostTag(int id)
    {
        var result = await _service.DeletePostTag(id);
        return StatusCode(result.StatusCode, result);
    }
}