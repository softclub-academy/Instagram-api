using Domain.Dtos.PostTagDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.PostTagService;

public interface IPostTagService
{
    Task<PagedResponse<List<PostTagDto>>> GetPostTags(PaginationFilter filter);
    Task<Response<PostTagDto>> GetPostTagById(int id);
    Task<Response<PostTagDto>> AddPostTag(PostTagDto addPostTag);
    Task<Response<PostTagDto>> UpdatePostTag(PostTagDto addPostTag);
    Task<Response<bool>> DeletePostTag(int id);
}