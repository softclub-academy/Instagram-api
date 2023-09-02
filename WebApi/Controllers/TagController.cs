using System.Net;
using Domain.Dtos.TagDto;
using Domain.Filters.TagFilter;
using Domain.Responses;
using Infrastructure.Services.TagService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class TagController : ControllerBase
{
    private readonly ITagService _service;

    public TagController(ITagService service)
    {
        _service = service;
    }

    [HttpGet("get-Tags")]
    public async Task<IActionResult> GetTags([FromQuery]TagFilter filter)
    {
        var result = await _service.GetTags(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-Tag-by-id")]
    public async Task<IActionResult> GetTagById(int id)
    {
        var result = await _service.GetTagById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-Tag")]
    public async Task<IActionResult> AddTag([FromBody]TagDto tag)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.AddTag(tag);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<TagDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("update-Tag")]
    public async Task<IActionResult> UpdateTag([FromBody]UpdateTagDto tag)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.UpdateTag(tag);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<TagDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-Tag")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        var result = await _service.DeleteTag(id);
        return StatusCode(result.StatusCode, result);
    }
}