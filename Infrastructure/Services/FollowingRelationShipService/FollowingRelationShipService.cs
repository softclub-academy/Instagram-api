using System.Net;
using AutoMapper;
using Domain.Dtos.FollowingRelationshipDto;
using Domain.Entities.User;
using Domain.Filters.FollowingRelationShipFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.FollowingRelationShipService;

public class FollowingRelationShipService : IFollowingRelationShipService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public FollowingRelationShipService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PagedResponse<List<GetFollowingRelationShipDto>>> GetFollowingRelationShip(
        FollowingRelationShipFilter filter)
    {
        try
        {
            var followingRelationShips = _context.FollowingRelationShips.AsQueryable();
            if (filter.Username != null)
                followingRelationShips =
                    followingRelationShips.Where(f => f.User.UserName == filter.Username);
            var response = await followingRelationShips
                .Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var mapped = _mapper.Map<List<GetFollowingRelationShipDto>>(response);
            var totalRecord = followingRelationShips.Count();
            return new PagedResponse<List<GetFollowingRelationShipDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetFollowingRelationShipDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetFollowingRelationShipDto>> GetFollowingRelationShipById(int id)
    {
        try
        {
            var following = await _context.FollowingRelationShips.FindAsync(id);
            var mapped = _mapper.Map<GetFollowingRelationShipDto>(following);
            return new Response<GetFollowingRelationShipDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetFollowingRelationShipDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> AddFollowingRelationShip(AddFollowingRelationShipDto followingRelationShip)
    {
        try
        {
            if (followingRelationShip.FollowingId == followingRelationShip.UserId)
                return new Response<bool>(HttpStatusCode.BadRequest, "You will not be able to subscribe to yourself");
            var user = await _context.Users.FindAsync(followingRelationShip.UserId);
            var followingUser = await _context.Users.FindAsync(followingRelationShip.FollowingId);
            if (user == null || followingUser == null)
                return new Response<bool>(HttpStatusCode.BadRequest, "User not found");
            var following = _mapper.Map<FollowingRelationShip>(followingRelationShip);
            await _context.FollowingRelationShips.AddAsync(following);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteFollowingRelationShip(string userId, string followingId)
    {
        try
        {
            var following =
                await _context.FollowingRelationShips.FirstOrDefaultAsync(f => f.UserId == userId && f.FollowingId == followingId);
            if (following == null)
                return new Response<bool>(HttpStatusCode.BadRequest, "Following relation ship not found");
            _context.FollowingRelationShips.Remove(following);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}