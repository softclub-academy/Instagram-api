﻿using Domain.Dtos.StoryDtos;
using Domain.Dtos.StoryViewDtos;
using Domain.Responses;
using Infrastructure.Services.StoryServices;
using Infrastructure.Services.StoryViewServices;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class StoryController(
    IStoryService storyService,
    IStoryViewService storyViewService) : BaseController
{

    [HttpGet("get-stories")]
    public async Task<IActionResult> GetStories()
    {
        var userId = User.Claims.FirstOrDefault(e => e.Type == "sid")!.Value;

        var result = await storyService.GetStories(userId);

        return StatusCode(200, result);
    }

    [HttpGet("get-user-stories/{userId}")]
    public async Task<IActionResult> GetUserStories(string userId)
    {
        var result = await storyService.GetUserStories(userId);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-my-stories")]
    public async Task<IActionResult> GetMyStoriesAsync()
    {
        var userId = User.Claims.FirstOrDefault(e => e.Type == "sid")!.Value;

        var result = await storyService.GetMyStoriesAsync(userId);

        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("LikeStory")]
    public async Task<Response<string>> LikeStory(int storyId)
    {
        var userId =User.Claims.FirstOrDefault(e => e.Type == "sid")!.Value;

        return await storyService.StoryLike(storyId,userId);
    }
    
    [HttpGet("GetStoryById")]
    public async Task<Response<GetStoryDto>> GetStoryById(int id)
    {
        var token = User.Claims.FirstOrDefault(e => e.Type == "sid")!.Value;

        var userName = User.Claims.FirstOrDefault(e => e.Type == "name")!.Value;

        return await storyService.GetStoryById(id, token,userName);
    }

    [HttpPost("AddStories")]
    public async Task<Response<string>> AddStories(AddStoryDto add)
    {
        var token = User.Claims.FirstOrDefault(e => e.Type == "sid")!.Value;

        return await storyService.AddStory(add, token);
    }

    [HttpDelete("DeleteStory")]
    public async Task<Response<bool>> DeleteStory(int id)
    {
        return await storyService.DeleteStory(id);
    }
    
    [HttpPost("add-story-view")]
    public Task<Response<GetStoryViewDto>> AddStoryView(AddStoryViewDto model)
    {
        var token = User.Claims.FirstOrDefault(e => e.Type == "sid")!.Value;

        return storyViewService.AddStoryView(model, token);
    } 
}