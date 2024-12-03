using System.Net;
using Domain.Dtos.UserProfileDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.PostService;
using Infrastructure.Services.UserProfileService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UserProfileController(IUserProfileService userProfileService,
        IPostService postService)
    : BaseController
{
    [HttpGet("get-user-profile-by-id")]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var result = await userProfileService.GetUserProfileById(id);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-is-follow-user-profile-by-id")]
    public async Task<IActionResult> GetIsFollowUserProfileById(string followingUserId)
    {
        var userId = User.Claims.FirstOrDefault(u => u.Type == "sid")!.Value;

        var result = await userProfileService.GetIsFollowUserProfileById(userId, followingUserId);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-my-profile")]
    public async Task<IActionResult> GetUserMyProfile()
    {
        var userId = User.Claims.FirstOrDefault(u => u.Type == "sid")!.Value;

        var result = await userProfileService.GetUserProfileById(userId);

        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("update-user-profile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileDto userProfile)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")!.Value;
            var result = await userProfileService.UpdateUserProfile(userProfile,userId);
            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<UserProfileDto>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("get-post-favorites")]
    public async Task<IActionResult> GetPostFavorites([FromQuery] PaginationFilter filter)
    {
        var userId = User.Claims.FirstOrDefault(u => u.Type == "sid")!.Value;
        var result = await postService.GetPostFavorites(filter, userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("update-user-image-profile")]
    public async Task<IActionResult> UpdateUserImageProfile(IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")!.Value;

            var result = await userProfileService.UpdateUserImageProfile(userId, imageFile);

            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<UserProfileDto>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-user-image-profile")]
    public async Task<IActionResult> DeleteUserImageProfile()
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")!.Value;

            var result = await userProfileService.DeleteUserImageProfile(userId);

            return StatusCode(result.StatusCode, result);
        }

        var response = new Response<UserProfileDto>(HttpStatusCode.BadRequest, ModelStateErrors());

        return StatusCode(response.StatusCode, response);
    }
}