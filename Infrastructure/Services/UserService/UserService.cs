using System.Net;
using AutoMapper;
using Domain.Dtos.UserDto;
using Domain.Dtos.UserLogDto;
using Domain.Entities;
using Domain.Entities.User;
using Domain.Filters.UserFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UserService;

public class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PagedResponse<List<GetUserDto>>> GetUsers(UserFilter filter)
    {
        try
        {
            var users = _context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(filter.UserName))
                users = users.Where(u => u.UserName.ToLower().Contains(filter.UserName.ToLower()));
            if (!string.IsNullOrEmpty(filter.Email))
                users = users.Where(u => u.Email.ToLower().Contains(filter.Email.ToLower()));
            var response = await users
                .Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var mapped = _mapper.Map<List<GetUserDto>>(response);
            var totalRecord = users.Count();
            return new PagedResponse<List<GetUserDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetUserDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetUserDto>> GetUserById(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            var mapped = _mapper.Map<GetUserDto>(user);
            return new Response<GetUserDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<string>> UserRegister(AddUserDto addUser)
    {
        try
        {
            var user = _mapper.Map<User>(addUser);
            if (user.Password != user.PasswordSalt)
                return new Response<string>(HttpStatusCode.BadRequest, "Registered error");
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new Response<string>($"Success! Your registered by id {user.UserId}");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<string>> UserLogin(UserLoginDto addUser)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.UserName == addUser.UserName && u.Password == addUser.Password);
            if (user == null) return new Response<string>("Your userName or password is incorrect!!!");
            var userLog = new UserLog()
            {
                UserId = user.UserId,
                LoginDate = DateTime.UtcNow
            };
            await _context.UserLogs.AddAsync(userLog);
            await _context.SaveChangesAsync();
            return new Response<string>("You have successfully logged in");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<string>> UserLogout(int id)
    {
        try
        {
            var userLogout = await _context.UserLogs.FindAsync(id);
            if (userLogout == null) return new Response<string>("Errors!!!");
            if (userLogout.LogoutDate != null) return new Response<string>("Your have already logged out");
            userLogout.LogoutDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new Response<string>("Your have successfully logged out");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetUserDto>> UpdateUser(AddUserDto addUser)
    {
        try
        {
            var user = _mapper.Map<User>(addUser);
            if (user.Password != user.PasswordSalt)
                return new Response<GetUserDto>(HttpStatusCode.BadRequest, "Updated error");
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, "Yor account updated successfully");
        }
        catch (Exception e)
        {
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return new Response<bool>(HttpStatusCode.BadRequest, "User not found");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<PagedResponse<List<GetUserLogDto>>> GetUserLogsByUserId(UserLogFilter filter)
    {
        try
        {
            var logs = _context.UserLogs.AsQueryable();
            if (filter.UserId != null)
                logs = logs.Where(l => l.UserId == filter.UserId);
            var response = await logs.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ToListAsync();
            var mapped = _mapper.Map<List<GetUserLogDto>>(response);
            var totalRecord = logs.Count();
            return new PagedResponse<List<GetUserLogDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetUserLogDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetUserLogDto>> GetUserLogById(int id)
    {
        try
        {
            var log = await _context.UserLogs.FindAsync(id);
            if (log == null) return new Response<GetUserLogDto>(HttpStatusCode.BadRequest, "Log not found");
            var mapped = _mapper.Map<GetUserLogDto>(log);
            return new Response<GetUserLogDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetUserLogDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}