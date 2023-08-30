using System.Net;
using Domain.Dtos.PostCategoryDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.PostCategoryService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("post-category")]
public class PostCategoryController : ControllerBase
{
    private readonly IPostCategoryService _service;

    public PostCategoryController(IPostCategoryService service)
    {
        _service = service;
    }

    [HttpGet("get-PostCategories")]
    public async Task<IActionResult> GetPostCategories([FromQuery]PaginationFilter filter)
    {
        var result = await _service.GetPostCategories(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-PostCategory-by-id")]
    public async Task<IActionResult> GetPostCategoryById(int id)
    {
        var result = await _service.GetPostCategoryById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-PostCategory")]
    public async Task<IActionResult> AddPostCategory([FromQuery]PostCategoryDto postCategory)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.AddPostCategory(postCategory);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<PostCategoryDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("update-PostCategory")]
    public async Task<IActionResult> UpdatePostCategory([FromQuery]PostCategoryDto postCategory)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.UpdatePostCategory(postCategory);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<PostCategoryDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-PostCategory")]
    public async Task<IActionResult> DeletePostCategory(int id)
    {
        var result = await _service.DeletePostCategory(id);
        return StatusCode(result.StatusCode, result);
    }
}