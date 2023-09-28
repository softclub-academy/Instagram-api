using System.Net;
using Domain.Dtos.PostFavoriteDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.PostFavoriteService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class PostFavoriteController : BaseController
{
    private readonly IPostFavoriteService _service;

    public PostFavoriteController(IPostFavoriteService service)
    {
        _service = service;
    }

    [HttpGet("get-PostFavorites")]
    public async Task<IActionResult> GetPostFavorites([FromQuery]PaginationFilter filter)
    {
        var result = await _service.GetPostFavorites(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-PostFavorite-by-id")]
    public async Task<IActionResult> GetPostFavoriteById(int id)
    {
        var result = await _service.GetPostFavoriteById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-PostFavorite")]
    public async Task<IActionResult> AddPostFavorite([FromBody]AddPostFavoriteDto postFavorite)
    {
        if (ModelState.IsValid)
        {
             var userTokenId=User.Claims.FirstOrDefault(e=>e.Type=="sid").Value;
            var result = await _service.AddPostFavorite(postFavorite);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<PostFavoriteDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-PostFavorite")]
    public async Task<IActionResult> DeletePostFavorite(int id)
    {
        var result = await _service.DeletePostFavorite(id);
        return StatusCode(result.StatusCode, result);
    }
}