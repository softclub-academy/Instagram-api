using Domain.Dtos.PostStatDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.PostStatService;

public interface IPostStatService
{
    Task<PagedResponse<List<PostStatDto>>> GetPostStats(PaginationFilter filter);
    Task<Response<bool>> AddPostStat(string userId, int id);
    Task<Response<bool>> DeletePostStat(string userId, int id);
}