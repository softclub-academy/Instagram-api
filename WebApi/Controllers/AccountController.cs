using System.Net;
using Domain.Dtos.LoginDto;
using Domain.Dtos.RegisterDto;
using Domain.Responses;
using Infrastructure.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await _service.Register(model);
            return StatusCode(response.StatusCode, response);
        }
        else
        {
            var errorMessage = ModelState.SelectMany(op => op.Value.Errors.Select(er => er.ErrorMessage)).ToList();
            var response = new Response<string>(HttpStatusCode.BadRequest, errorMessage);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await _service.Login(model);
            return StatusCode(response.StatusCode, response);
        }
        else
        {
            var errorMessage = ModelState.SelectMany(op => op.Value.Errors.Select(er => er.ErrorMessage)).ToList();
            var response = new Response<string>(HttpStatusCode.BadRequest, errorMessage);
            return StatusCode(response.StatusCode, response);
        }
    }
}