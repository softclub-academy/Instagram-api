using System.Net;
using AutoMapper;
using Domain.Dtos.UserDto;
using Domain.Entities.User;
using Domain.Filters.UserFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UserService;

public class UserService(DataContext context, IMapper mapper,
        UserManager<IdentityUser> userManager)
    : IUserService
{
    public async Task<PagedResponse<List<GetUserDto>>> GetUsers(UserFilter filter)
    {
        try
        {
            var users = context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(filter.UserName))
                users = users.Where(u => u.UserName!.ToLower().Contains(filter.UserName.ToLower()));
            if (!string.IsNullOrEmpty(filter.Email))
                users = users.Where(u => u.Email!.ToLower().Contains(filter.Email.ToLower()));
            var response = users
                .Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);
            var result = await (from u in users
                select new GetUserDto()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Avatar = u.UserProfile.Image,
                    UserType = u.UserType,
                    DateRegistered = u.DateRegistred
                }).AsNoTracking().ToListAsync();
            mapper.Map<List<GetUserDto>>(response);
            var totalRecord = users.Count();
            
            return new PagedResponse<List<GetUserDto>>(result, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetUserDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetUserDto>> GetUserById(string id)
    {
        try
        {
            var user = await (from u in context.Users
                where u.Id == id
                select new GetUserDto()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Avatar = u.UserProfile.Image,
                    UserType = u.UserType,
                    DateRegistered = u.DateRegistred
                }).AsNoTracking().FirstOrDefaultAsync();
            return new Response<GetUserDto>(user);
        }
        catch (Exception e)
        {
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetUserDto>> UpdateUser(AddUserDto addUser)
    {
        try
        {
            var user = mapper.Map<User>(addUser);
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, "Yor account updated successfully");
        }
        catch (Exception e)
        {
            return new Response<GetUserDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteUser(string id)
    {
        try
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) return new Response<bool>(HttpStatusCode.BadRequest, "User not found");
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}