using System.Net;
using AutoMapper;
using Domain.Dtos.PostFavoriteDto;
using Domain.Entities.Post;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.PostFavoriteService;

public class PostFavoriteService : IPostFavoriteService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PostFavoriteService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PagedResponse<List<GetPostFavoriteDto>>> GetPostFavorites(PaginationFilter filter)
    {
        try
        {
            var posts = _context.PostFavorites.AsQueryable();
            var response = await posts.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ToListAsync();
            var mapped = _mapper.Map<List<GetPostFavoriteDto>>(response);
            var totalRecord = posts.Count();
            return new PagedResponse<List<GetPostFavoriteDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetPostFavoriteDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetPostFavoriteDto>> GetPostFavoriteById(int id)
    {
        try
        {
            var post = await _context.PostFavorites.FindAsync(id);
            var mapped = _mapper.Map<GetPostFavoriteDto>(post);
            return new Response<GetPostFavoriteDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetPostFavoriteDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> AddPostFavorite(AddPostFavoriteDto addPostFavorite)
    {
        try
        {
            var favpost=await _context.PostFavorites.FirstOrDefaultAsync(e=>e.PostId==addPostFavorite.PostId && e.UserId==addPostFavorite.UserId);
            if (favpost==null){
            var post = _mapper.Map<PostFavorite>(addPostFavorite);
            await _context.PostFavorites.AddAsync(post);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);}
            else{
             _context.PostFavorites.Remove(favpost);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);}
            
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeletePostFavorite(int id)
    {
        try
        {
            var post = await _context.PostFavorites.FindAsync(id);
            if (post == null) return new Response<bool>(HttpStatusCode.BadRequest, "Post not found");
            _context.PostFavorites.Remove(post);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}