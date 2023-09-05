using System.Net;
using AutoMapper;
using Domain.Dtos.UserProfileDto;
using Domain.Entities.User;
using Domain.Filters.UserProfileFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.AspNetCore.Mvc;
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


    #region UpdateUserProfile

    public async Task<Response<GetUserProfileDto>> UpdateUserProfile(UpdateUserProfileDto addUserProfile, string userId)
    {
        try
        {
            var existing = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);


            if (existing != null)
            {
                if (addUserProfile.FirstName != null) existing.FirstName = addUserProfile.FirstName;
                existing.FirstName = existing.FirstName;
                if (addUserProfile.LastName != null) existing.LastName = addUserProfile.LastName;
                existing.LastName = existing.LastName;
                if (addUserProfile.About != null) existing.About = addUserProfile.About;
                existing.About = existing.About;
                if (addUserProfile.Occupation != null) existing.Occupation = addUserProfile.Occupation;
                existing.Occupation = existing.Occupation;
                var loc = await _context.Locations.FirstOrDefaultAsync(x => x.LocationId == addUserProfile.LocationId);
                if (loc == null) 
                {
                    return new Response<GetUserProfileDto>(HttpStatusCode.NotFound, "not found this location");
                }
                else
                {
                    existing.LocationId = addUserProfile.LocationId;
                }

                if (addUserProfile.DOB != null)
                {
                    existing.DOB = addUserProfile.DOB;
                }

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
                    existing.Image = _fileService.CreateFile(addUserProfile.Image).Data;
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
                    DOB = existing.DOB,
                };

                return new Response<GetUserProfileDto>(mapped);
            }

            return new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, "mot found");
        }
        catch (Exception e)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}