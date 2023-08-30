using System.Net;
using Domain.Dtos.UserDto;
using Domain.Filters.UserFilter;
using Domain.Responses;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("user")]
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

    [HttpPost("register-User")]
    public async Task<IActionResult> UserRegister([FromQuery]AddUserDto user)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.UserRegister(user);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<UserDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost("login-User")]
    public async Task<IActionResult> UserLogin([FromQuery]UserLoginDto user)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.UserLogin(user);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<UserDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("user-logout")]
    public async Task<IActionResult> UserLogout(int id)
    {
        var result = await _service.UserLogout(id);
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

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<UserDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-User")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _service.DeleteUser(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-user-logs-by-user-id")]
    public async Task<IActionResult> GetUserLogsByUserId([FromQuery]UserLogFilter filter)
    {
        var result = await _service.GetUserLogsByUserId(filter);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpGet("get-user-log-by-id")]
    public async Task<IActionResult> GetUserLogById(int id)
    {
        var result = await _service.GetUserLogById(id);
        return StatusCode(result.StatusCode, result);
    }
}