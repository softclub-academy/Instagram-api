using System.Net;
using Domain.Dtos.ExternalAccountDto;
using Domain.Filters.ExternalAccountFilter;
using Domain.Responses;
using Infrastructure.Services.ExternalAccountService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ExternalAccountController : BaseController
{
    private readonly IExternalAccountService _service;

    public ExternalAccountController(IExternalAccountService service)
    {
        _service = service;
    }

    [HttpGet("get-ExternalAccounts")]
    public async Task<IActionResult> GetExternalAccounts([FromQuery]ExternalAccountFilter filter)
    {
        var result = await _service.GetExternalAccountsByName(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-ExternalAccount-by-id")]
    public async Task<IActionResult> GetExternalAccountById(int id)
    {
        var result = await _service.GetExternalAccountById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-ExternalAccount")]
    public async Task<IActionResult> AddExternalAccount([FromBody]ExternalAccountDto externalAccount)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.AddExternalAccount(externalAccount);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<ExternalAccountDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("update-ExternalAccount")]
    public async Task<IActionResult> UpdateExternalAccount([FromBody]ExternalAccountDto externalAccount)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.UpdateExternalAccount(externalAccount);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<ExternalAccountDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-ExternalAccount")]
    public async Task<IActionResult> DeleteExternalAccount(int id)
    {
        var result = await _service.DeleteExternalAccount(id);
        return StatusCode(result.StatusCode, result);
    }
}