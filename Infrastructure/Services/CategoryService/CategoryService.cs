using System.Net;
using AutoMapper;
using Domain.Dtos.CategoryDto;
using Domain.Entities.Post;
using Domain.Filters.CategoryFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CategoryService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResponse<List<CategoryDto>>> GetCategoriesByName(CategoryFilter filter)
    {
        try
        {
            var categories = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(filter.CategoryName))
                categories = categories.Where(c => c.CategoryName.ToLower().Contains(filter.CategoryName.ToLower()));

            var response = await categories
                .Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var totalRecord = categories.Count();
            var mapped = _mapper.Map<List<CategoryDto>>(response);
            return new PagedResponse<List<CategoryDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<CategoryDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<CategoryDto>> GetCategoryById(int id)
    {
        try
        {
            var category = await _context.Categories.FindAsync(id);
            var mapped = _mapper.Map<CategoryDto>(category);
            return new Response<CategoryDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<CategoryDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<CategoryDto>> AddCategory(CategoryDto categoryDto)
    {
        try
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<CategoryDto>(category);
            return new Response<CategoryDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<CategoryDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<CategoryDto>> UpdateCategory(CategoryDto categoryDto)
    {
        try
        {
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<CategoryDto>(category);
            return new Response<CategoryDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<CategoryDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteCategory(int id)
    {
        try
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return new Response<bool>(HttpStatusCode.BadRequest, "Category not found");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}