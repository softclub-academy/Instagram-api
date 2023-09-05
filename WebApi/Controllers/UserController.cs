using System.Net;
using Domain.Dtos.UserDto;
using Domain.Filters.UserFilter;
using Domain.Responses;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("get-users")]
    public async Task<IActionResult> GetUsers([FromQuery]UserFilter filter)
    {
        var result = await _service.GetUsers(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-User-by-id")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var result = await _service.GetUserById(userId);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpPut("update-User")]
    public async Task<IActionResult> UpdateUser([FromBody]AddUserDto user)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.UpdateUser(user);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value?
            .Errors.Select(er => er.ErrorMessage) ?? Array.Empty<string>()).ToList();
        var response = new Response<UserDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-User")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _service.DeleteUser(userId);
        return StatusCode(result.StatusCode, result);
    }
}