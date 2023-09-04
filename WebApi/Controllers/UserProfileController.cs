using System.Net;
using Domain.Dtos.UserProfileDto;
using Domain.Filters.UserProfileFilter;
using Domain.Responses;
using Infrastructure.Services.StatisticFollowAndPostService;
using Infrastructure.Services.UserProfileService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UserProfileController : BaseController
{
    private readonly IUserProfileService _userProfileService;
    private readonly IStatisticFollowAndPostService _statisticFollowAndPostService;

    public UserProfileController( IUserProfileService userProfileService, IStatisticFollowAndPostService statisticFollowAndPostService)
    {
        _userProfileService = userProfileService;
        _statisticFollowAndPostService = statisticFollowAndPostService;
    }

   

    [HttpGet("get-UserProfile-by-id")]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var result = await _userProfileService.GetUserProfileById(id);
        return StatusCode(result.StatusCode, result);
    }

   
    
    [HttpPut("update-UserProfile")]
    public async Task<IActionResult> UpdateUserProfile([FromForm]UpdateUserProfileDto userProfile)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
            var result = await _userProfileService.UpdateUserProfile(userProfile,userId);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<UserProfileDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    
    // statistic profile
    
    

    

    [HttpGet("CounterProfile")]
    public async Task<Response<GetStatistic>> GetCountPost()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
        var post = await _statisticFollowAndPostService.GetUserPost(userId);
        var following = await _statisticFollowAndPostService.GetFollowing(userId);
        var follower = await _statisticFollowAndPostService.GetFollowers(userId);
        var test = new GetStatistic()
        {
            Post = post.Data,
            Follower = follower.Data,
            Following = following.Data
        };
        return new Response<GetStatistic>(test);
    }
   
  
}