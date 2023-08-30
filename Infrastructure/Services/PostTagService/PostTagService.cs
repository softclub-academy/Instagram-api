using System.Net;
using AutoMapper;
using Domain.Dtos.PostTagDto;
using Domain.Entities.Post;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.PostTagService;

public class PostTagService : IPostTagService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PostTagService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResponse<List<PostTagDto>>> GetPostTags(PaginationFilter filter)
    {
        try
        {
            var postTags = _context.PostTags.AsQueryable();
            var response = await postTags.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ToListAsync();
            var mapped = _mapper.Map<List<PostTagDto>>(response);
            var totalRecord = postTags.Count();
            return new PagedResponse<List<PostTagDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<PostTagDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<PostTagDto>> GetPostTagById(int id)
    {
        try
        {
            var postTag = await _context.PostTags.FindAsync(id);
            var mapped = _mapper.Map<PostTagDto>(postTag);
            return new Response<PostTagDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<PostTagDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<PostTagDto>> AddPostTag(PostTagDto addPostTag)
    {
        try
        {
            var postTag = _mapper.Map<PostTag>(addPostTag);
            await _context.PostTags.AddAsync(postTag);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<PostTagDto>(postTag);
            return new Response<PostTagDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<PostTagDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<PostTagDto>> UpdatePostTag(PostTagDto addPostTag)
    {
        try
        {
            var postTag = _mapper.Map<PostTag>(addPostTag);
            _context.PostTags.Update(postTag);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<PostTagDto>(postTag);
            return new Response<PostTagDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<PostTagDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeletePostTag(int id)
    {
        try
        {
            var postTag = await _context.PostTags.FindAsync(id);
            if (postTag == null) return new Response<bool>(HttpStatusCode.BadRequest, "Post Tag not found");
            _context.PostTags.Remove(postTag);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }

    }
}