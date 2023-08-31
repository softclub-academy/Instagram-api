using System.Net;
using Domain.Dtos.UserDto;
using Domain.Filters.UserFilter;
using Domain.Responses;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
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
    public async Task<IActionResult> GetUserById(int id)
    {
        var result = await _service.GetUserById(id);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpPut("update-User")]
    public async Task<IActionResult> UpdateUser([FromQuery]AddUserDto user)
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
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _service.DeleteUser(id);
        return StatusCode(result.StatusCode, result);
    }
}