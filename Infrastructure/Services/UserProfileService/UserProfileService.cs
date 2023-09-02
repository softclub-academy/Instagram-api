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

    public async Task<Response<GetUserProfileDto>> GetUserProfileById(string id)
    {
        try
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == id);
            if (userProfile != null)
            {
                var mapped = _mapper.Map<GetUserProfileDto>(userProfile);
                return new Response<GetUserProfileDto>(mapped);
            }
            else
            {
                return new Response<GetUserProfileDto>(HttpStatusCode.NotFound, "not found");
            }
        }
        catch (Exception e)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }


    // add user profile

    #region AddUserProfile

    public async Task<Response<GetUserProfileDto>> AddUserProfile(AddUserProfileDto addUserProfile)
    {
        try
        {
            var existing = await _context.Users.FirstOrDefaultAsync(x => x.Id == addUserProfile.UserId);
            if (existing != null)
            {
                var filename = _fileService.CreateFile(addUserProfile.Image);
                var userProfile = _mapper.Map<UserProfile>(addUserProfile);
                userProfile.Image = filename.Data;
                await _context.UserProfiles.AddAsync(userProfile);
                await _context.SaveChangesAsync();
                var mapped = _mapper.Map<GetUserProfileDto>(userProfile);
                return new Response<GetUserProfileDto>(mapped);
            }

            else if(existing.Id == addUserProfile.UserId)
            {
                return new Response<GetUserProfileDto>(HttpStatusCode.NotFound, "not found");
            }
            else
            {
                return new Response<GetUserProfileDto>(HttpStatusCode.NotFound, "not found");
            }
        }
        catch (Exception e)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateUserProfile

    public async Task<Response<GetUserProfileDto>> UpdateUserProfile(UpdateUserProfileDto addUserProfile)
    {
        try
        {
            var existing = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == addUserProfile.UserId);

            if (existing != null)
            {
                // var userProfile = new UserProfile();
                // userProfile.FirstName = addUserProfile.FirstName;
                // userProfile.LastName = addUserProfile.LastName;
                // userProfile.UserId = addUserProfile.UserId;
                // userProfile.About = addUserProfile.About;
                // userProfile.LocationId = addUserProfile.LocationId;
                // userProfile.Occupation = addUserProfile.Occupation;
                // userProfile.DateUpdated = addUserProfile.DateUpdated;
                // userProfile.DOB = userProfile.DOB;
                if (addUserProfile.UserId != null) existing.UserId = addUserProfile.UserId;
                existing.UserId = existing.UserId;
                if (addUserProfile.FirstName != null) existing.FirstName = addUserProfile.FirstName;
                existing.FirstName = existing.FirstName;
                if (addUserProfile.LastName != null) existing.LastName = addUserProfile.LastName;
                existing.LastName = existing.LastName;
                if (addUserProfile.About != null) existing.About = addUserProfile.About;
                existing.About = existing.About;
                if (addUserProfile.Occupation != null) existing.Occupation = addUserProfile.Occupation;
                existing.Occupation = existing.Occupation;
                if (existing.LocationId != null) existing.LocationId = addUserProfile.LocationId;
                existing.LocationId = existing.LocationId;
                existing.DOB = existing.DOB;
                if (existing.DateUpdated != null) existing.DateUpdated = addUserProfile.DateUpdated;
                existing.DateUpdated = DateTime.UtcNow;
                if (existing.Image != null)
                {
                    if (addUserProfile != null && existing.Image != null)
                    {
                        _fileService.DeleteFile(existing.Image);
                        existing.Image = _fileService.CreateFile(addUserProfile.Image).Data;
                        await _context.SaveChangesAsync();
                    }
                    else if (addUserProfile.Image == null)
                    {
                        existing.Image = existing.Image;
                        await _context.SaveChangesAsync();
                    }
                }
                else if (existing.Image == null && addUserProfile.Image != null)
                {
                    existing.Image = _fileService.CreateFile(addUserProfile.Image).ToString();
                    await _context.SaveChangesAsync();
                }


                await _context.SaveChangesAsync();
                var mapped = new GetUserProfileDto()
                {
                    About = existing.About,
                    Image = existing.Image,
                    Occupation = existing.Occupation,
                    FirstName = existing.FirstName,
                    LastName = existing.LastName,
                    DateUpdated = existing.DateUpdated,
                    LocationId = existing.LocationId,
                    UserId = existing.UserId,
                    DOB = existing.DOB,
                };

                return new Response<GetUserProfileDto>(mapped);
            }

            return new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, "mot found");
        }
        catch (Exception e)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.InternalServerError, "error");
        }
    }

    #endregion
}