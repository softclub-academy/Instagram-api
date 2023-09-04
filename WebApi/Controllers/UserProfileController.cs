using System.Net;
using Domain.Dtos.UserProfileDto;
using Domain.Filters.UserProfileFilter;
using Domain.Responses;
using Infrastructure.Services.UserProfileService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UserProfileController : BaseController
{
    private readonly IUserProfileService _service;

    public UserProfileController(IUserProfileService service)
    {
        _service = service;
    }

   

    [HttpGet("get-UserProfile-by-id")]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var result = await _service.GetUserProfileById(id);
        return StatusCode(result.StatusCode, result);
    }

   
    
    [HttpPut("update-UserProfile")]
    public async Task<IActionResult> UpdateUserProfile([FromForm]UpdateUserProfileDto userProfile)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
            var result = await _service.UpdateUserProfile(userProfile,userId);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<UserProfileDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

  
}