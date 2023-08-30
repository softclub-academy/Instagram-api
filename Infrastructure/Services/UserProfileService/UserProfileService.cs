using System.Net;
using AutoMapper;
using Domain.Dtos.UserProfileDto;
using Domain.Entities.User;
using Domain.Filters.UserProfileFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UserProfileService;

public class UserProfileService : IUserProfileService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public UserProfileService(DataContext context, IMapper mapper, IFileService fileService)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }
    
    public async Task<PagedResponse<List<GetUserProfileDto>>> GetUserProfiles(UserProfileFilter filter)
    {
        try
        {
            var userProfiles = _context.UserProfiles.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Name))
                userProfiles = userProfiles.Where(u => u.FirstName.ToLower().Contains(filter.Name.ToLower()) ||
                                                       u.LastName.ToLower().Contains(filter.Name.ToLower()));
            var response = await userProfiles
                
                .Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var mapped = _mapper.Map<List<GetUserProfileDto>>(response);
            var totalRecord = userProfiles.Count();
            return new PagedResponse<List<GetUserProfileDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetUserProfileDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetUserProfileDto>> GetUserProfileById(int id)
    {
        try
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            var mapped = _mapper.Map<GetUserProfileDto>(userProfile);
            return new Response<GetUserProfileDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetUserProfileDto>> AddUserProfile(AddUserProfileDto addUserProfile)
    {
        try
        {
            _fileService.CreateFile(addUserProfile.Image);
            var userProfile = _mapper.Map<UserProfile>(addUserProfile);
            await _context.UserProfiles.AddAsync(userProfile);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<GetUserProfileDto>(userProfile);
            return new Response<GetUserProfileDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetUserProfileDto>> UpdateUserProfile(AddUserProfileDto addUserProfile)
    {
        try
        {
            _fileService.CreateFile(addUserProfile.Image);
            var userProfile = _mapper.Map<UserProfile>(addUserProfile);
            _context.UserProfiles.Update(userProfile);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<GetUserProfileDto>(userProfile);
            return new Response<GetUserProfileDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteUserProfile(int id)
    {
        try
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null) return new Response<bool>(HttpStatusCode.BadRequest, "User profile not found");
            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}