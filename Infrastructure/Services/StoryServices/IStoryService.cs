using Domain.Dtos.StoryDtos;
using Domain.Entities.User;
using Domain.Responses;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.StoryServices;

public interface IStoryService
{
   Task<Response<GetStoryDto>> AddStory(AddStoryDto file);
}