using System.Net;
using AutoMapper;
using Domain.Dtos.PostDto;
using Domain.Entities.Post;
using Domain.Filters.PostFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.PostService;

public class PostService : IPostService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IFileService _service;

    public PostService(DataContext context, IMapper mapper, 
        IFileService fileService,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
        _userManager = userManager;
    }

    public async Task<PagedResponse<List<GetPostDto>>> GetPosts(PostFilter filter)
    {
        try
        {
            var posts = _context.Posts.AsQueryable();
            if (filter.UserId != null)
                posts = posts.Where(p => p.UserId == filter.UserId);
            if (!string.IsNullOrEmpty(filter.Content))
                posts = posts.Where(p => p.Content.ToLower().Contains(filter.Content.ToLower()));
            if (!string.IsNullOrEmpty(filter.Title))
                posts = posts.Where(p => p.Title.ToLower().Contains(filter.Title.ToLower()));
            var response = await posts
                .Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var result = await (from p in posts
                select new GetPostDto()
                {
                    PostId = p.PostId,
                    UserId = p.UserId,
                    Title = p.Title,
                    Content = p.Content,
                    Status = p.Status,
                    DatePublished = p.DatePublished.ToShortDateString(),
                    Images = _context.Images.Where(i => i.PostId == p.PostId).Select(i => i.Path).ToList()
                }).ToListAsync();
            var totalRecord = posts.Count();
            return new PagedResponse<List<GetPostDto>>(result, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetPostDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetPostDto>> GetPostById(int id)
    {
        try
        {
            var post = await _context.Posts.Select(p => new GetPostDto()
            {
                PostId = p.PostId,
                UserId = p.UserId,
                Title = p.Title,
                Content = p.Content,
                Status = p.Status,
                DatePublished = p.DatePublished.ToShortDateString(),
                Images = _context.Images.Where(i => i.PostId == p.PostId).Select(i => i.Path).ToList()
            }).FirstOrDefaultAsync(p => p.PostId == id);
            return new Response<GetPostDto>(post);
        }
        catch (Exception e)
        {
            return new Response<GetPostDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<PagedResponse<List<GetPostDto>>> GetPostByFollowing(PostFollowingFilter filter)
    {
        try
        {
            var posts = await (from p in _context.Posts
                join u in _context.Users on p.UserId equals u.Id
                join f in _context.FollowingRelationShips on u.Id equals f.FollowingId
                where f.UserId == filter.UserId
                select new GetPostDto()
                {
                    PostId = p.PostId,
                    UserId = p.UserId,
                    Title = p.Title,
                    Content = p.Content,
                    Status = p.Status,
                    DatePublished = p.DatePublished.ToShortDateString(),
                    Images = _context.Images.Where(i => i.PostId == p.PostId).Select(i => i.Path).ToList(),
                    
                }).ToListAsync();
            var totalRecord = posts.Count;
            return new PagedResponse<List<GetPostDto>>(posts, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetPostDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetPostDto>> AddPost(AddPostDto addPost)
    {
        try
        {
            var post = _mapper.Map<Post>(addPost);  
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            var postStat = new PostLike()
            {
                PostId = post.PostId
            };
            var postView = new PostView()
            {
                PostId = post.PostId
            };
            var list = new List<string>();
            foreach (var image in addPost.Images)
            {
                var path = _fileService.CreateFile(image);
                var images = new Image()
                {
                    PostId = post.PostId,
                    Path = path.Data
                };
                list.Add(images.Path);
                await _context.Images.AddAsync(images);
                await _context.SaveChangesAsync();
            }
            await _context.PostViews.AddAsync(postView);
            await _context.PostStats.AddAsync(postStat);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<GetPostDto>(post);
            mapped.Images = list;
            return new Response<GetPostDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetPostDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetPostDto>> UpdatePost(AddPostDto addPost)
    {
        try
        {
            var post = _mapper.Map<Post>(addPost);  
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            var postStat = new PostLike()
            {
                PostId = post.PostId
            };
            var list = new List<string>();
            foreach (var image in addPost.Images)
            {
                var path = _fileService.CreateFile(image);
                var images = new Image()
                {
                    PostId = post.PostId,
                    Path = path.Data
                };
                list.Add(images.Path);
                _context.Images.Update(images);
                await _context.SaveChangesAsync();
            }
            await _context.PostStats.AddAsync(postStat);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<GetPostDto>(post);
            mapped.Images = list;
            return new Response<GetPostDto>(mapped);
            /*var post = _mapper.Map<Post>(addPost);
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<GetPostDto>(post);
            return new Response<GetPostDto>(mapped);*/
        }
        catch (Exception e)
        {
            return new Response<GetPostDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeletePost(int id)
    {
        try
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return new Response<bool>(HttpStatusCode.BadRequest, "Post not found");
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> LikePost(string? userId, int postId)
    {
        var stats = _context.PostStats.FirstOrDefault(e => e.PostId == postId);
        var existingStatUser =
            _context.StatUserIds.FirstOrDefault(st => st.UserId == userId && st.PostStatId == stats.PostId);
        if (existingStatUser == null)
        {
            var newPostUserLike = new StatUserId()
            {
                UserId = userId,
                PostStatId = stats.PostId
            };
            await _context.StatUserIds.AddRangeAsync(newPostUserLike);
            stats.LikeCount++;
            await _context.SaveChangesAsync();
            

            return new Response<bool>(true);
        }

        _context.StatUserIds.Remove(existingStatUser);
        stats.LikeCount--;
        await _context.SaveChangesAsync();
        return new Response<bool>(true);
        
    }
}