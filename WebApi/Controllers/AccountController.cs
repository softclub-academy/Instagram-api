using System.Net;
using Domain.Dtos.LoginDto;
using Domain.Dtos.RegisterDto;
using Domain.Responses;
using Infrastructure.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AccountController : BaseController
{
    private readonly IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody]RegisterDto model)
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
    public async Task<IActionResult> Login([FromBody]LoginDto model)
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