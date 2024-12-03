using System.Net;
using Domain.Dtos.LocationDto;
using Domain.Dtos.UserProfileDto;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UserProfileService;

public class UserProfileService(DataContext context, IFileService fileService)
    : IUserProfileService
{
    public async Task<Response<GetUserProfileDto>> GetUserProfileById(string id)
    {
        try
        {
            var userProfile = await (from p in context.UserProfiles
                where p.UserId == id
                select new GetUserProfileDto()
                {
                    UserName = p.User.UserName!,
                    Gender = p.Gender.ToString()!,
                    Occupation = p.Occupation,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    DateUpdated = p.DateUpdated,
                    LocationId = p.LocationId,
                    Dob = p.Dob,
                    About = p.About,
                    Image = p.Image!,
                    PostCount = p.User.Posts.Count,
                    SubscribersCount = context.FollowingRelationShips.Count(x => x.FollowingId == id),
                    SubscriptionsCount = p.User.FollowingRelationShips.Count // context.FollowingRelationShips.Count(x => x.UserId == id)
                }).AsNoTracking().FirstOrDefaultAsync();
            
            if (userProfile != null)
            {
                
                return new Response<GetUserProfileDto>(userProfile);
            }
            return new Response<GetUserProfileDto>(HttpStatusCode.NotFound, "User not found");
        }
        catch (Exception e)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetUserProfileDto>> UpdateUserImageProfile(string userId, IFormFile imageFile)
    {
        var existing = await context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
        if (existing == null)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, "User not found");
        }

        if (existing.Image != null)
        {
            fileService.DeleteFile(existing.Image);
        }

        existing.Image = fileService.CreateFile(imageFile).Data;

        await context.SaveChangesAsync();

        return new Response<GetUserProfileDto>(HttpStatusCode.OK, "success");
    }

    public async Task<Response<GetUserProfileDto>> DeleteUserImageProfile(string userId)
    {
        var existing = await context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
        if (existing == null)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, "User not found");
        }

        if (existing.Image != null)
        {
            fileService.DeleteFile(existing.Image);
        }

        existing.Image = null;

        await context.SaveChangesAsync();

        return new Response<GetUserProfileDto>(HttpStatusCode.OK, "success");
    }

    public async Task<Response<GetUserProfileDto>> UpdateUserProfile(UpdateUserProfileDto addUserProfile, string userId)
    {
        try
        {
            var existing = await context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existing == null)
            {
                return new Response<GetUserProfileDto>(HttpStatusCode.BadRequest, "User profile not found");
            }

            //if (addUserProfile.FirstName != null) existing.FirstName = addUserProfile.FirstName;
            //existing.FirstName = existing.FirstName;
            //if (addUserProfile.LastName != null) existing.LastName = addUserProfile.LastName;
            //existing.LastName = existing.LastName;
            //if (addUserProfile.About != null) existing.About = addUserProfile.About;
            //existing.About = existing.About;
            //if (addUserProfile.Occupation != null) existing.Occupation = addUserProfile.Occupation;
            //existing.Occupation = existing.Occupation;
            //var loc = await context.Locations.FirstOrDefaultAsync(x => x.LocationId == addUserProfile.LocationId);
            //if (loc == null)
            //{
            //    return new Response<GetUserProfileDto>(HttpStatusCode.NotFound, "not found this location");
            //}
            //else
            //{
            //    existing.LocationId = addUserProfile.LocationId;
            //}

            existing.Gender = addUserProfile.Gender;
            existing.About = addUserProfile.About;
            existing.DateUpdated = DateTime.UtcNow;

            //if (addUserProfile.Image != null)
            //{
            //    if (existing.Image != null)
            //    {
            //        fileService.DeleteFile(existing.Image);
            //    }
            //
            //    existing.Image = fileService.CreateFile(addUserProfile.Image).Data;
            //}

            await context.SaveChangesAsync();

            return new Response<GetUserProfileDto>(HttpStatusCode.OK, "success");
        }
        catch (Exception e)
        {
            return new Response<GetUserProfileDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> AddLocationAsync(string userId, AddLocationDto model)
    {
        var user = await context.UserProfiles.FirstOrDefaultAsync(c => c.UserId == userId);
        if(user == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "User not found");
        }

        var location = new Location
        {
            Country = model.Country,
            State = model.State,
            City = model.City,
            ZipCode = model.ZipCode
        };

        await context.Locations.AddAsync(location);
        await context.SaveChangesAsync();

        var userProfile = await context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if(userProfile != null)
        {
            userProfile.LocationId = location.LocationId;

            await context.SaveChangesAsync();
        }

        return new Response<string>(HttpStatusCode.OK, "Success");
    }

    public async Task<Response<string>> UpdagteLocationAsync(string userId, UpdateLocationDto model)
    {
        var location = await context.Locations.FirstOrDefaultAsync(x => x.LocationId == model.LocationId);
        if(location == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "User location not found");
        }

        location.Country = model.Country;
        location.State = model.State;
        location.City = model.City;
        location.ZipCode = model.ZipCode;

        context.Locations.Update(location);
        await context.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "User location update");
    }

    public async Task<Response<GetIsFollowUserProfileDto>> GetIsFollowUserProfileById(string userId, string followingUserId)
    {
        try
        {
            var userProfile = await(from p in context.UserProfiles
                                    where p.UserId == followingUserId
                                    select new GetIsFollowUserProfileDto()
                                    {
                                        UserName = p.User.UserName!,
                                        Gender = p.Gender.ToString()!,
                                        Occupation = p.Occupation,
                                        FirstName = p.FirstName,
                                        LastName = p.LastName,
                                        DateUpdated = p.DateUpdated,
                                        LocationId = p.LocationId,
                                        Dob = p.Dob,
                                        About = p.About,
                                        Image = p.Image!,
                                        PostCount = p.User.Posts.Count,
                                        SubscribersCount = context.FollowingRelationShips.Count(x => x.FollowingId == followingUserId),
                                        SubscriptionsCount = p.User.FollowingRelationShips.Count,
                                        IsSubscriber = context.FollowingRelationShips.Any(x =>
                                                (x.UserId == userId && x.FollowingId == p.UserId) ||
                                                (x.UserId == p.UserId && x.FollowingId == followingUserId))
                                    }).AsNoTracking().FirstOrDefaultAsync();

            if (userProfile != null)
            {

                return new Response<GetIsFollowUserProfileDto>(userProfile);
            }
            return new Response<GetIsFollowUserProfileDto>(HttpStatusCode.NotFound, "User not found");
        }
        catch (Exception e)
        {
            return new Response<GetIsFollowUserProfileDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}