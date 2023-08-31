using Domain.Dtos.PostCategoryDto;
using Domain.Dtos.PostStatDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.PostCategoryService;

public interface IPostCategoryService
{
    Task<PagedResponse<List<PostCategoryDto>>> GetPostCategories(PaginationFilter filter);
    Task<Response<PostCategoryDto>> GetPostCategoryById(int id);
    Task<Response<PostCategoryDto>> AddPostCategory(PostCategoryDto addPostCategory);
    Task<Response<PostCategoryDto>> UpdatePostCategory(UpdatePostCategoryDto addPostCategory);
    Task<Response<bool>> DeletePostCategory(int id);
}