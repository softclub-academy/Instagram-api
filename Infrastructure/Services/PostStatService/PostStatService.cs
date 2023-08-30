using System.Net;
using AutoMapper;
using Domain.Dtos.PostStatDto;
using Domain.Entities.Post;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.PostStatService;

public class PostStatService : IPostStatService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PostStatService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResponse<List<PostStatDto>>> GetPostStats(PaginationFilter filter)
    {
        try
        {
            var postStat = _context.PostStats.AsQueryable();
            var response = await postStat.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ToListAsync();
            var mapped = _mapper.Map<List<PostStatDto>>(response);
            var totalRecord = postStat.Count();
            return new PagedResponse<List<PostStatDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<PostStatDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> AddPostStat(int userId, int id)
    {
        try
        {
            var user = await _context.UserLogs.FirstOrDefaultAsync(u => u.UserId == userId && u.LogoutDate == null);
            if (user == null) return new Response<bool>(HttpStatusCode.BadRequest, "Login first!!!");
            var userStat = await _context.StatUserIds.FindAsync(userId);
            if (userStat != null) return new Response<bool>(HttpStatusCode.BadRequest, "You have already set like");
            var postStat = await _context.PostStats.FirstOrDefaultAsync(p => p.PostId == id);
            if (postStat == null) return new Response<bool>(HttpStatusCode.BadRequest, "Post not found");
            postStat.LikeCount++;
            var user0 = new StatUserId()
            {
                UserId = userId,
                PostStatId = id
            };
            await _context.StatUserIds.AddAsync(user0);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeletePostStat(int userId, int id)
    {
        try
        {
            var postStat = await _context.PostStats.FindAsync(id);
            if (postStat == null) return new Response<bool>(HttpStatusCode.BadRequest, "Post not found");
            var userStat = await _context.StatUserIds.FirstOrDefaultAsync(u => u.UserId == userId);
            if (userStat == null)
                return new Response<bool>(HttpStatusCode.BadRequest, "You have already delete the like");
            postStat.LikeCount--;
            _context.StatUserIds.Remove(userStat);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}