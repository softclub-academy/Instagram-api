using AutoMapper;
using Domain.Dtos.StoryDtos;
using Domain.Dtos.ViewerDtos;
using Domain.Entities;
using Domain.Entities.Post;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.StoryServices;

public class StoryService(
    IFileService fileService,
    IMapper mapper,
    DataContext context,
    IWebHostEnvironment hostEnvironment)
    : IStoryService

{
    public async Task<List<GetMyStoryDto>> GetStories(string userId)
    {
        var query = context.Users
            .Include(x => x.UserProfile)
            .Include(x => x.FollowingRelationShips)
            .Include(x => x.Stories)
            .ThenInclude(x => x.StoryLikes);

        var user = await query.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            return [];
        }

        var currentUser = new GetMyStoryDto
        {
            UserId = user.Id,
            UserImage = user.UserProfile.Image,
            UserName = user.UserName,
            Stories = user.Stories.Select(item => new UserStoryDto
            {
                Id = item.Id,
                CreateAt = item.CreateAt,
                FileName = item.FileName,
                PostId = item.PostId,
                LikedCount = item.StoryLikes.Count
            }).ToList()
        };

        var followings = user.FollowingRelationShips.Select(x => x.FollowingId).Distinct().ToList();

        var newQuery = context.Users
            .Include(x => x.UserProfile)
            .Include(x => x.Stories)
            .ThenInclude(x => x.StoryLikes)
            .Where(x => followings.Any(s => s == x.Id));

        var users = await newQuery.Select(userEl => new GetMyStoryDto
        {
            UserId = userEl.Id,
            UserImage = userEl.UserProfile.Image,
            UserName = userEl.UserName,
            Stories = userEl.Stories.Select(item => new UserStoryDto
            {
                Id = item.Id,
                CreateAt = item.CreateAt,
                FileName = item.FileName,
                PostId = item.PostId,
                Liked = item.StoryLikes.Any(x => x.UserId == userId),
                LikedCount = item.StoryLikes.Count
            }).ToList()
        }).ToListAsync();

        var items = new List<GetMyStoryDto>
        {
            currentUser
        };

        if (users.Count != 0)
        {
            items.AddRange(users.Where(x => x.Stories.Count != 0).ToList());
        }

        return items;
    }

    public async Task<Response<GetMyStoryDto>> GetUserStories(string userId)
    {
        var query = context.Users
            .Include(x => x.UserProfile)
            .Include(x => x.FollowingRelationShips)
            .Include(x => x.Stories)
            .ThenInclude(x => x.StoryLikes);

        var user = await query.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            return new Response<GetMyStoryDto>(HttpStatusCode.NotFound, "user not found");

        var currentUser = new GetMyStoryDto
        {
            UserId = user.Id,
            UserImage = user.UserProfile.Image,
            UserName = user.UserName,
            Stories = user.Stories.Select(item => new UserStoryDto
            {
                Id = item.Id,
                CreateAt = item.CreateAt,
                FileName = item.FileName,
                PostId = item.PostId,
                Liked = item.StoryLikes.Count != 0,
            }).ToList()
        };

        return new Response<GetMyStoryDto>(currentUser);
    }

    public async Task<Response<GetStoryDto>> GetStoryById(int id, string userId, string userName)
    {
        try
        {
            var story = await context.Stories.FirstOrDefaultAsync(e => e.Id == id);
            if (story != null)
            {
                var story2 = await (from st in context.Stories
                                    join pf in context.UserProfiles on st.UserId equals pf.UserId
                                    where st.Id == id
                                    select new GetStoryDto()
                                    {
                                        Id = st.Id,
                                        FileName = st.FileName,
                                        CreateAt = st.CreateAt,
                                        UserId = st.UserId,
                                        PostId = st.PostId,
                                        ViewerDto = story.UserId == userId
                                            ? new ViewerDto()
                                            {
                                                Name = pf.FirstName,
                                                UserName = userName,
                                                ViewCount = st.StoryStat.ViewCount,
                                                ViewLike = st.StoryStat.ViewLike
                                            }
                                            : null
                                    }).AsNoTracking().FirstOrDefaultAsync();
                return new Response<GetStoryDto>(story2);
            }
            else
            {
                return new Response<GetStoryDto>(HttpStatusCode.BadRequest, "Story not found");
            }
        }
        catch (Exception e)
        {
            return new Response<GetStoryDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> AddStory(AddStoryDto file, string userId)
    {
        try
        {
            var file1 = new Story()
            {
                UserId = userId,
                PostId = file.PostId,
            };

            if (file1.PostId == null)
            {
                var fileName = fileService.CreateFile(file.Image).Data;
                file1.FileName = fileName;
            }
            else
            {
                var post = (from p in context.Posts
                            join image in context.Images on p.PostId equals image.PostId
                            select new
                            {
                                Image = image.ImageName
                            }).ToList();
                if (post != null)
                {
                    var img = post[0];
                    file1.FileName = img.Image;
                }
            }

            await context.Stories.AddAsync(file1);
            await context.SaveChangesAsync();

            var stat = new StoryStat()
            {
                StoryId = file1.Id
            };

            await context.StoryStats.AddAsync(stat);
            await context.SaveChangesAsync();

            return new Response<string>(HttpStatusCode.OK, "success");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> StoryLike(int StoryId, string userId)
    {
        try
        {
            var story = await context.Stories.FindAsync(StoryId);
            if (story == null) return new Response<string>(HttpStatusCode.BadRequest, "Story not found");
            var user = await context.StoryLikes.FirstOrDefaultAsync(e =>
                e.UserId == userId && e.StoryId == StoryId);
            var stat = await context.StoryStats.FirstAsync(s => s.StoryId == story.Id);
            if (user == null)
            {
                stat.ViewLike++;
                var storyLike = new StoryLike()
                {
                    StoryId = StoryId,
                    UserId = userId,
                };
                await context.StoryLikes.AddAsync(storyLike);
                await context.SaveChangesAsync();
                return new Response<string>("Liked");
            }
            else
            {
                stat.ViewLike--;
                context.StoryLikes.Remove(user);
                await context.SaveChangesAsync();
                return new Response<string>("Disliked");
            }
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteStory(int id)
    {
        try
        {
            var story = context.Stories.FirstOrDefault(e => e.Id == id);
            if (story != null)
            {
                if (story.FileName != null)
                {
                    var path = Path.Combine(hostEnvironment.WebRootPath, "images", story.FileName);
                    File.Delete(path);
                }

                context.Stories.Remove(story);
                await context.SaveChangesAsync();

                return new Response<bool>(true);
            }

            return new Response<bool>(false);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetMyStoryDto>> GetMyStoriesAsync(string userId)
    {
        var user = await context.Users.Include(x => x.UserProfile).FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            return new Response<GetMyStoryDto>(HttpStatusCode.NotFound, "User not found");
        }

        var response = new GetMyStoryDto
        {
            UserId = user.Id,
            UserImage = user.UserProfile.Image,
            UserName = user.UserName
        };

        var stories = await context.Stories.Include(x => x.StoryLikes).Where(x => x.UserId == user.Id).Select(item => new UserStoryDto
        {
            Id = item.Id,
            CreateAt = item.CreateAt,
            FileName = item.FileName,
            PostId = item.PostId,
            Liked = item.StoryLikes.Count != 0,
        }).ToListAsync();

        response.Stories.AddRange(stories);

        return new Response<GetMyStoryDto>(response);
    }
}