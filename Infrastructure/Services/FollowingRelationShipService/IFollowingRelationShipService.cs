using Domain.Dtos.FollowingRelationshipDto;
using Domain.Filters.FollowingRelationShipFilter;
using Domain.Responses;

namespace Infrastructure.Services.FollowingRelationShipService;

public interface IFollowingRelationShipService
{
    Task<PagedResponse<List<GetFollowingRelationShipDto>>> GetFollowingRelationShip(FollowingRelationShipFilter filter);
    Task<Response<GetFollowingRelationShipDto>> GetFollowingRelationShipById(int id);
    Task<Response<bool>> AddFollowingRelationShip(string followingUserId, string userId);
    Task<Response<bool>> DeleteFollowingRelationShip(string userId, string followingUserId);
}