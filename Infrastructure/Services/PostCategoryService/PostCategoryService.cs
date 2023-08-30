using System.Net;
using AutoMapper;
using Domain.Dtos.PostCategoryDto;
using Domain.Dtos.PostFavoriteDto;
using Domain.Entities.Post;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.PostCategoryService;

public class PostCategoryService : IPostCategoryService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PostCategoryService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PagedResponse<List<PostCategoryDto>>> GetPostCategories(PaginationFilter filter)
    {
        try
        {
            var postCategories = _context.PostCategories.AsQueryable();
            var response = await postCategories.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ToListAsync();
            var mapped = _mapper.Map<List<PostCategoryDto>>(response);
            var totalRecord = postCategories.Count();
            return new PagedResponse<List<PostCategoryDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<PostCategoryDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<PostCategoryDto>> GetPostCategoryById(int id)
    {
        try
        {
            var postCategory = await _context.PostCategories.FindAsync(id);
            var mapped = _mapper.Map<PostCategoryDto>(postCategory);
            return new Response<PostCategoryDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<PostCategoryDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<PostCategoryDto>> AddPostCategory(PostCategoryDto addPostCategory)
    {
        try
        {
            var postCategory = _mapper.Map<PostCategory>(addPostCategory);
            await _context.PostCategories.AddAsync(postCategory);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<PostCategoryDto>(postCategory);
            return new Response<PostCategoryDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<PostCategoryDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<PostCategoryDto>> UpdatePostCategory(PostCategoryDto addPostCategory)
    {
        try
        {
            var postCategory = _mapper.Map<PostCategory>(addPostCategory);
            _context.PostCategories.Update(postCategory);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<PostCategoryDto>(postCategory);
            return new Response<PostCategoryDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<PostCategoryDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeletePostCategory(int id)
    {
        try
        {
            var postCategory = await _context.PostCategories.FindAsync(id);
            if (postCategory == null) return new Response<bool>(HttpStatusCode.BadRequest, "Post category not found");
            _context.PostCategories.Remove(postCategory);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}