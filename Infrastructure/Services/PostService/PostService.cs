using System.Net;
using AutoMapper;
using Domain.Dtos.PostCommentDto;
using Domain.Dtos.PostDto;
using Domain.Entities.Post;
using Domain.Filters.PostFilter;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.PostService;

public class PostService : IPostService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public PostService(DataContext context, IMapper mapper,
        IFileService fileService)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<PagedResponse<List<GetPostDto>>> GetPosts(PostFilter filter, string userId)
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
            var result = await (from p in posts
                select new GetPostDto()
                {
                    PostId = p.PostId,
                    UserId = p.UserId,
                    Title = p.Title,
                    Content = p.Content,
                    DatePublished = p.DatePublished.ToShortDateString(),
                    Images = p.Images.Select(i => i.ImageName).ToList(),
                    PostLike = p.PostLike.PostUserLikes.Any(l => l.UserId == userId && l.PostLikeId == p.PostId),
                    PostLikeCount = p.PostLike.LikeCount,
                    UserLikes = p.PostLike.PostUserLikes.Select(u => u.UserId).ToList(),
                    PostView = p.PostView.ViewCount,
                    UserViews = p.PostView.PostViewUsers.Select(u => u.UserId).ToList(),
                    CommentCount = p.PostComments.Count(),
                    Comments = p.PostComments.Select(s => new GetPostCommentDto()
                    {
                        CommentId = s.PostCommentId,
                        UserId = s.UserId,
                        Comment = s.Comment,
                        DateCommented = s.DateCommented
                    }).OrderByDescending(c => c.DateCommented).ToList(),
                    PostFavorite = p.PostFavorites.FirstOrDefault(k => k.PostId == p.PostId && k.UserId == p.UserId) !=
                                   null
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
            var post = await (from p in _context.Posts
                where p.PostId == id
                select new GetPostDto()
                {
                    PostId = p.PostId,
                    UserId = p.UserId,
                    Title = p.Title,
                    Content = p.Content,
                    DatePublished = p.DatePublished.ToShortDateString(),
                    Images = p.Images.Select(i => i.ImageName).ToList(),
                    // _context.Images.Where(i => i.PostId == p.PostId).Select(i => i.ImageName).ToList(),
                    PostLikeCount = p.PostLike.LikeCount,
                    PostView = p.PostView.ViewCount,
                    CommentCount = p.PostComments.Count(),
                    PostFavorite = p.PostFavorites.FirstOrDefault(k => k.PostId == p.PostId && k.UserId == p.UserId) !=
                                   null
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
                join f in _context.FollowingRelationShips on p.UserId equals f.FollowingId
                where f.UserId == filter.UserId
                select new GetPostDto()
                {
                    PostId = p.PostId,
                    UserId = p.UserId,
                    Title = p.Title,
                    Content = p.Content,
                    DatePublished = p.DatePublished.ToShortDateString(),
                    Images = p.Images.Select(i => i.ImageName).ToList(),
                    // _context.Images.Where(i => i.PostId == p.PostId).Select(i => i.ImageName).ToList(),
                    PostLikeCount = p.PostLike.LikeCount,
                    PostView = p.PostView.ViewCount,
                    CommentCount = p.PostComments.Count(),
                    PostFavorite =
                        p.PostFavorites.FirstOrDefault(k => k.PostId == p.PostId && k.UserId == p.UserId) == null
                            ? false
                            : true
                }).ToListAsync();
            var totalRecord = posts.Count();
            return new PagedResponse<List<GetPostDto>>(posts, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetPostDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<int>> AddPost(AddPostDto addPost, string userId)
    {
        try
        {
            var post = _mapper.Map<Post>(addPost);
            post.UserId = userId;
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            var postLike = new PostLike()
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
                var imageName = _fileService.CreateFile(image);
                var images = new Image()
                {
                    PostId = post.PostId,
                    ImageName = imageName.Data!
                };
                list.Add(images.ImageName);
                await _context.Images.AddAsync(images);
                await _context.SaveChangesAsync();
            }

            await _context.PostViews.AddAsync(postView);
            await _context.PostLikes.AddAsync(postLike);
            await _context.SaveChangesAsync();

            return new Response<int>(post.PostId);
        }
        catch (Exception e)
        {
            return new Response<int>(HttpStatusCode.InternalServerError, e.Message);
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
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> LikePost(string userId, int postId)
    {
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        if (userId == null) throw new ArgumentNullException(nameof(userId));
        var stats = await _context.PostLikes.FirstOrDefaultAsync(e => e.PostId == postId);
        if (stats == null) return new Response<bool>(HttpStatusCode.BadRequest, "stats not found");

        var existingStatUser =
            _context.PostUserLikes.FirstOrDefault(st => st.UserId == userId && st.PostLikeId == stats.PostId);
        if (existingStatUser == null)
        {
            var newPostUserLike = new PostUserLike()
            {
                UserId = userId,
                PostLikeId = stats!.PostId
            };
            await _context.PostUserLikes.AddAsync(newPostUserLike);
            stats.LikeCount++;
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }

        _context.PostUserLikes.Remove(existingStatUser);
        stats.LikeCount--;
        await _context.SaveChangesAsync();
        return new Response<bool>(false);
    }

    public async Task<Response<bool>> ViewPost(string userId, int postId)
    {
        try
        {
            var postViewUser =
                await _context.PostViewUsers.FirstOrDefaultAsync(p => p.UserId == userId && p.PostViewId == postId);
            if (postViewUser != null) return new Response<bool>(true);

            var post = await _context.PostViews.FindAsync(postId);
            post!.ViewCount++;
            var postView = new PostViewUser()
            {
                UserId = userId,
                PostViewId = postId
            };
            await _context.PostViewUsers.AddAsync(postView);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> AddComment(AddPostCommentDto addPostComment, string userId)
    {
        try
        {
            var post = await _context.Posts.FindAsync(addPostComment.PostId);
            if (post == null)
                return new Response<bool>(HttpStatusCode.BadRequest, "Post not found");
            var comment = _mapper.Map<PostComment>(addPostComment);
            comment.UserId = userId;
            await _context.PostComments.AddAsync(comment);
            await _context.SaveChangesAsync();
            var postCommentLike = new PostCommentLike() { PostCommentId = comment.PostCommentId };
            await _context.PostCommentLikes.AddAsync(postCommentLike);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteComment(int commentId)
    {
        try
        {
            var comment = await _context.PostComments.FindAsync(commentId);
            if (comment == null) return new Response<bool>(HttpStatusCode.BadRequest, "Comment not found");
            _context.PostComments.Remove(comment);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}