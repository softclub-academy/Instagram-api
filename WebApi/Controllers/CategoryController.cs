using System.Net;
using Domain.Dtos.CategoryDto;
using Domain.Filters.CategoryFilter;
using Domain.Responses;
using Infrastructure.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet("get-categories")]
    public async Task<IActionResult> GetCategories([FromQuery]CategoryFilter filter)
    {
        var result = await _service.GetCategoriesByName(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-category-by-id")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var result = await _service.GetCategoryById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-category")]
    public async Task<IActionResult> AddCategory([FromBody]CategoryDto category)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.AddCategory(category);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<CategoryDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("update-category")]
    public async Task<IActionResult> UpdateCategory([FromBody]UpdateCategoryDto category)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.UpdateCategory(category);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<CategoryDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-category")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _service.DeleteCategory(id);
        return StatusCode(result.StatusCode, result);
    }
}