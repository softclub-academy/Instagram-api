using Domain.Dtos.UserProfileDto;
using Domain.Filters.UserProfileFilter;
using Domain.Responses;

namespace Infrastructure.Services.UserProfileService;

public interface IUserProfileService
{
    Task<PagedResponse<List<GetUserProfileDto>>> GetUserProfiles(UserProfileFilter filter);
    Task<Response<GetUserProfileDto>> GetUserProfileById(int id);
    Task<Response<GetUserProfileDto>> AddUserProfile(AddUserProfileDto addUserProfile);
    Task<Response<GetUserProfileDto>> UpdateUserProfile(AddUserProfileDto addUserProfile);
    Task<Response<bool>> DeleteUserProfile(int id);
}