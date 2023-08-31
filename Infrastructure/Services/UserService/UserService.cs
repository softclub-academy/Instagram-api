﻿using System.Net;
using AutoMapper;
using Domain.Dtos.UserDto;
using Domain.Entities.User;
using Domain.Filters.UserFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UserService;

public class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(DataContext context, IMapper mapper,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
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

    public async Task<Response<GetUserDto>> UpdateUser(AddUserDto addUser)
    {
        try
        {
            var user = _mapper.Map<User>(addUser);
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
}