using Domain.Dtos.LocationDto;
using Domain.Dtos.UserProfileDto;
using Domain.Responses;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.UserProfileService;

public interface IUserProfileService
{
    Task<Response<GetUserProfileDto>> GetUserProfileById(string userId);
    Task<Response<GetUserProfileDto>> UpdateUserProfile(UpdateUserProfileDto addUserProfile,string userId);
    Task<Response<GetUserProfileDto>> UpdateUserImageProfile(string userId, IFormFile imageFile);
    Task<Response<GetUserProfileDto>> DeleteUserImageProfile(string userId);
    Task<Response<string>> AddLocationAsync(string userId, AddLocationDto model);
    Task<Response<string>> UpdagteLocationAsync(string userId, UpdateLocationDto model);

    Task<Response<GetIsFollowUserProfileDto>> GetIsFollowUserProfileById(string userId, string followingUserId);
}