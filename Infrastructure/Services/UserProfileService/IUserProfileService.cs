using Domain.Dtos.UserProfileDto;
using Domain.Filters.UserProfileFilter;
using Domain.Responses;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.UserProfileService;

public interface IUserProfileService
{
    Task<Response<GetUserProfileDto>> GetUserProfileById(string userId);
    Task<Response<GetUserProfileDto>> UpdateUserProfile(UpdateUserProfileDto addUserProfile,string userId);
    Task<Response<GetUserProfileDto>> UpdateUserImageProfile(string userId, IFormFile imageFile);
    Task<Response<GetUserProfileDto>> DeleteUserImageProfile(string userId);
}