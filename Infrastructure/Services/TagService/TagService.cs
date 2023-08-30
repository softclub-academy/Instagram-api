using System.Net;
using AutoMapper;
using Domain.Dtos.TagDto;
using Domain.Entities.Post;
using Domain.Filters.TagFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.TagService;

public class TagService : ITagService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public TagService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PagedResponse<List<TagDto>>> GetTags(TagFilter filter)
    {
        try
        {
            var tags = _context.Tags.AsQueryable();
            if (!string.IsNullOrEmpty(filter.TagName))
                tags = tags.Where(t => t.TagName.ToLower().Contains(filter.TagName.ToLower()));
            var response = await tags
                .Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var mapped = _mapper.Map<List<TagDto>>(response);
            var totalRecord = tags.Count();
            return new PagedResponse<List<TagDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<TagDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<TagDto>> GetTagById(int id)
    {
        try
        {
            var tag = await _context.Tags.FindAsync(id);
            var mapped = _mapper.Map<TagDto>(tag);
            return new Response<TagDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<TagDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<TagDto>> AddTag(TagDto addTag)
    {
        try
        {
            var tag = _mapper.Map<Tag>(addTag);
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<TagDto>(tag);
            return new Response<TagDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<TagDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<TagDto>> UpdateTag(TagDto addTag)
    {
        try
        {
            var tag = _mapper.Map<Tag>(addTag);
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<TagDto>(tag);
            return new Response<TagDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<TagDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteTag(int id)
    {
        try
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return new Response<bool>(HttpStatusCode.BadRequest, "Tag not found");
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}