using System.Net;
using AutoMapper;
using Domain.Dtos.PostCommentDto;
using Domain.Dtos.PostDto;
using Domain.Dtos.PostFavoriteDto;
using Domain.Entities.Post;
using Domain.Filters;
using Domain.Filters.PostCommentFilter;
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
                    DatePublished = p.DatePublished,
                    Images = p.Images.Select(i => i.ImageName).ToList(),
                    PostLike = p.PostLike.PostUserLikes.Any(l => l.UserId == userId && l.PostLikeId == p.PostId),
                    PostLikeCount = p.PostLike.LikeCount,
                    UserLikes = p.PostLike.PostUserLikes.Select(u => u.UserId).ToList(),
                    PostView = p.PostView.ViewCount,
                    UserViews = p.PostView.PostViewUsers.Select(u => u.UserId).ToList(),
                    CommentCount = p.PostComments.Count(),
                    PostFavorite = p.PostFavorite.PostFavoriteUsers.Any(l => l.UserId == userId && l.PostFavoriteId == p.PostId),
                    UserFavorite = p.PostFavorite.PostFavoriteUsers.Select(u => u.UserId).ToList(),
                    Comments = p.PostComments.Select(s => new GetPostCommentDto()
                    {
                        PostCommentId = s.PostCommentId,
                        UserId = s.UserId,
                        Comment = s.Comment,
                        DateCommented = s.DateCommented
                    }).OrderByDescending(c => c.DateCommented).ToList(),
                }).ToListAsync();
            var totalRecord = posts.Count();
            return new PagedResponse<List<GetPostDto>>(result, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetPostDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetPostDto>> GetPostById(int id, string userId)
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
                    DatePublished = p.DatePublished,
                    Images = p.Images.Select(i => i.ImageName).ToList(),
                    PostLike = p.PostLike.PostUserLikes.Any(l => l.UserId == userId && l.PostLikeId == p.PostId),
                    PostLikeCount = p.PostLike.LikeCount,
                    UserLikes = p.PostLike.PostUserLikes.Select(u => u.UserId).ToList(),
                    PostView = p.PostView.ViewCount,
                    UserViews = p.PostView.PostViewUsers.Select(u => u.UserId).ToList(),
                    CommentCount = p.PostComments.Count(),
                    PostFavorite = p.PostFavorite.PostFavoriteUsers.Any(l => l.UserId == userId && l.PostFavoriteId == p.PostId),
                    UserFavorite = p.PostFavorite.PostFavoriteUsers.Select(u => u.UserId).ToList(),
                    Comments = p.PostComments.Select(s => new GetPostCommentDto()
                    {
                        PostCommentId = s.PostCommentId,
                        UserId = s.UserId,
                        Comment = s.Comment,
                        DateCommented = s.DateCommented
                    }).OrderByDescending(c => c.DateCommented).ToList(),
                }).FirstOrDefaultAsync(p => p.PostId == id);
            return new Response<GetPostDto>(post);
        }
        catch (Exception e)
        {
            return new Response<GetPostDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<PagedResponse<List<GetPostDto>>> GetPostByFollowing(PostFollowingFilter filter, string userId)
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
                    DatePublished = p.DatePublished,
                    Images = p.Images.Select(i => i.ImageName).ToList(),
                    PostLike = p.PostLike.PostUserLikes.Any(l => l.UserId == userId && l.PostLikeId == p.PostId),
                    PostLikeCount = p.PostLike.LikeCount,
                    UserLikes = p.PostLike.PostUserLikes.Select(u => u.UserId).ToList(),
                    PostView = p.PostView.ViewCount,
                    UserViews = p.PostView.PostViewUsers.Select(u => u.UserId).ToList(),
                    CommentCount = p.PostComments.Count(),
                    PostFavorite = p.PostFavorite.PostFavoriteUsers.Any(l => l.UserId == userId && l.PostFavoriteId == p.PostId),
                    UserFavorite = p.PostFavorite.PostFavoriteUsers.Select(u => u.UserId).ToList(),
                    Comments = p.PostComments.Select(s => new GetPostCommentDto()
                    {
                        PostCommentId = s.PostCommentId,
                        UserId = s.UserId,
                        Comment = s.Comment,
                        DateCommented = s.DateCommented
                    }).OrderByDescending(c => c.DateCommented).ToList(),
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
            var postFavorite = new PostFavorite()
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
            await _context.PostFavorites.AddAsync(postFavorite);
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
        var stats = await _context.PostLikes.FirstOrDefaultAsync(e => e.PostId == postId);
        if (stats == null) return new Response<bool>(HttpStatusCode.BadRequest, "stats not found");

        var existingStatUser =
            _context.PostUserLikes.FirstOrDefault(st => st.UserId == userId && st.PostLikeId == stats.PostId);
        if (existingStatUser == null)
        {
            var newPostUserLike = new PostUserLike()
            {
                UserId = userId,
                PostLikeId = stats.PostId
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

    public async Task<PagedResponse<List<GetPostCommentDto>>> GetPostComments(PostCommentFilter filter)
    {
        try
        {
            var comments = _context.PostComments.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Comment))
                comments = comments.Where(c => c.Comment.ToLower().Contains(filter.Comment.ToLower()));
            var response = await comments
                .Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var totalRecord = comments.Count();
            var mapped = _mapper.Map<List<GetPostCommentDto>>(response);
            return new PagedResponse<List<GetPostCommentDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetPostCommentDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetPostCommentDto>> GetPostCommentById(int id)
    {
        try
        {
            var comment = await _context.PostComments.FindAsync(id);
            var mapped = _mapper.Map<GetPostCommentDto>(comment);
            return new Response<GetPostCommentDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetPostCommentDto>(HttpStatusCode.BadRequest, e.Message);
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

    public async Task<PagedResponse<List<GetPostFavoriteDto>>> GetPostFavorites(PaginationFilter filter, string userId)
    {
        try
        {
            var posts = _context.Posts.AsQueryable();
            var res = (from p in _context.PostFavorites
                select new GetPostDto()
                {
                    PostId = p.Post.PostId,
                    UserId = p.Post.UserId,
                    
                }).Where(p => p.UserId == userId);
            var response = await posts.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ToListAsync();
            var mapped = _mapper.Map<List<GetPostFavoriteDto>>(response);
            var totalRecord = posts.Count();
            return new PagedResponse<List<GetPostFavoriteDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetPostFavoriteDto>>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<GetPostFavoriteDto>> GetPostFavoriteById(int id)
    {
        try
        {
            var post = await _context.PostFavorites.FindAsync(id);
            var mapped = _mapper.Map<GetPostFavoriteDto>(post);
            return new Response<GetPostFavoriteDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetPostFavoriteDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> AddPostFavorite(AddPostFavoriteDto addPostFavorite, string userId)
    {
        var favorite = await _context.PostFavorites.FirstOrDefaultAsync(e => e.PostId == addPostFavorite.PostId);
        if (favorite == null) return new Response<bool>(HttpStatusCode.BadRequest, "Post not found");

        var existingFavoriteUser =
            _context.PostFavoriteUsers.FirstOrDefault(st => st.UserId == userId && st.PostFavoriteId == favorite.PostId);
        if (existingFavoriteUser == null)
        {
            var newPostFavorite = new PostFavoriteUser()
            {
                UserId = userId,
                PostFavoriteId = favorite.PostId
            };
            await _context.PostFavoriteUsers.AddAsync(newPostFavorite);
            favorite.FavoriteCount++;
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }

        _context.PostFavoriteUsers.Remove(existingFavoriteUser);
        favorite.FavoriteCount--;
        await _context.SaveChangesAsync();
        return new Response<bool>(false);
    }

    public Task<Response<bool>> DeletePostFavorite(int id)
    {
        throw new NotImplementedException();
    }
}