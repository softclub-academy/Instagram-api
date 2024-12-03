using Domain.Dtos.StoryDtos;
using Domain.Responses;

namespace Infrastructure.Services.StoryServices;

public interface IStoryService
{
   Task<List<GetMyStoryDto>> GetStories(string userId);
   Task<Response<GetMyStoryDto>> GetUserStories(string userId);
   Task<Response<GetStoryDto>> GetStoryById(int id,string token,string userName);
   Task<Response<string>> AddStory(AddStoryDto file,string token);
   Task<Response<string>> StoryLike(int storyId,string userId);
   Task<Response<bool>> DeleteStory(int id);

   Task<Response<GetMyStoryDto>> GetMyStoriesAsync(string userId);
}