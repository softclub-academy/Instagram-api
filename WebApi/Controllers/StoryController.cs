using Domain.Dtos.StoryDtos;
using Domain.Responses;
using Infrastructure.Services.StoryServices;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class StoryController : ControllerBase
{
    private readonly IStoryService _storyService;

    public StoryController(IStoryService storyService)
    {
        _storyService = storyService;
    }

    [HttpPost("AddStories")]
    public async Task<Response<GetStoryDto>> AddStories(AddStoryDto add)
    {
        var token = User.Claims.FirstOrDefault(e => e.Type == "sid").Value;
        return await _storyService.AddStory(add,token);
    }

    [HttpDelete("DeleteStory")]
    public async Task<Response<bool>> DeleteStory(int id)
    {
        return await _storyService.DeleteStory(id);
    }
}