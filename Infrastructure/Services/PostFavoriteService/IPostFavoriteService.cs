using Domain.Dtos.PostFavoriteDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.PostFavoriteService;

public interface IPostFavoriteService
{
    Task<PagedResponse<List<GetPostFavoriteDto>>> GetPostFavorites(PaginationFilter filter);
    Task<Response<GetPostFavoriteDto>> GetPostFavoriteById(int id);
    Task<Response<bool>> AddPostFavorite(AddPostFavoriteDto addPostFavorite);
    Task<Response<bool>> DeletePostFavorite(int id);
}