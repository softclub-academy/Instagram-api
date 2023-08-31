using Domain.Dtos.CategoryDto;
using Domain.Filters.CategoryFilter;
using Domain.Responses;

namespace Infrastructure.Services.CategoryService;

public interface ICategoryService
{
    Task<PagedResponse<List<CategoryDto>>> GetCategoriesByName(CategoryFilter filter);
    Task<Response<CategoryDto>> GetCategoryById(int id);
    Task<Response<CategoryDto>> AddCategory(CategoryDto category);
    Task<Response<CategoryDto>> UpdateCategory(UpdateCategoryDto category);
    Task<Response<bool>> DeleteCategory(int id);
}