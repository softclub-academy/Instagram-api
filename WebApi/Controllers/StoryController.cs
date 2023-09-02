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
        return await _storyService.AddStory(add);
    }
}