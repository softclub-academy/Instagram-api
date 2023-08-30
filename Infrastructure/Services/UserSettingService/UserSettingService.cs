using System.Net;
using AutoMapper;
using Domain.Dtos.UserSettingDto;
using Domain.Entities.User;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UserSettingService;

public class UserSettingService : IUserSettingService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserSettingService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PagedResponse<List<UserSettingDto>>> GetUserSettings(PaginationFilter filter)
    {
        try
        {
            var userSettings = _context.UserSettings.AsQueryable();
            var response = await userSettings.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ToListAsync();
            var mapped = _mapper.Map<List<UserSettingDto>>(response);
            var totalRecord = userSettings.Count();
            return new PagedResponse<List<UserSettingDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<UserSettingDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<UserSettingDto>> GetUserSettingById(int id)
    {
        try
        {
            var userSetting = await _context.UserSettings.FindAsync(id);
            var mapped = _mapper.Map<UserSettingDto>(userSetting);
            return new Response<UserSettingDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<UserSettingDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<UserSettingDto>> AddUserSetting(UserSettingDto addUserSetting)
    {
        try
        {
            var userSetting = _mapper.Map<UserSetting>(addUserSetting);
            await _context.UserSettings.AddAsync(userSetting);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<UserSettingDto>(userSetting);
            return new Response<UserSettingDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<UserSettingDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<UserSettingDto>> UpdateUserSetting(UserSettingDto addUserSetting)
    {
        try
        {
            var userSetting = _mapper.Map<UserSetting>(addUserSetting);
            _context.UserSettings.Update(userSetting);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<UserSettingDto>(userSetting);
            return new Response<UserSettingDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<UserSettingDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteUserSetting(int id)
    {
        try
        {
            var userSetting = await _context.UserSettings.FindAsync(id);
            if (userSetting == null) return new Response<bool>(HttpStatusCode.BadRequest, "User setting not found");
            _context.UserSettings.Remove(userSetting);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}