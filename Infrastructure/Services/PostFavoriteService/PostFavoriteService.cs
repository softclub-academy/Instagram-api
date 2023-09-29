using System.Net;
using AutoMapper;
using Domain.Dtos.PostCommentDto;
using Domain.Dtos.PostDto;
using Domain.Dtos.PostFavoriteDto;
using Domain.Entities.Post;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.PostFavoriteService;

public class PostFavoriteService : IPostFavoriteService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PostFavoriteService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResponse<List<GetPostDto>>> GetPostFavorites(PaginationFilter filter, string userId)
    {
        try
        {
            var posts = _context.Posts.AsQueryable();
            var response = await posts.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ToListAsync();
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
                    PostFavorite =
                        p.PostFavorite.PostFavoriteUsers.Any(l => l.UserId == userId && l.PostFavoriteId == p.PostId),
                    UserFavorite = p.PostFavorite.PostFavoriteUsers.Select(u => u.UserId).ToList(),
                    Comments = p.PostComments.Select(s => new GetPostCommentDto()
                    {
                        PostCommentId = s.PostCommentId,
                        UserId = s.UserId,
                        Comment = s.Comment,
                        DateCommented = s.DateCommented
                    }).OrderByDescending(c => c.DateCommented).ToList(),
                })
                .Where(p => p.PostFavorite == true)
                .ToListAsync();
            var totalRecord = posts.Count();
            return new PagedResponse<List<GetPostDto>>(result, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            return new PagedResponse<List<GetPostDto>>(HttpStatusCode.BadRequest, e.Message);
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

    public async Task<Response<bool>> AddPostFavorite(AddPostFavoriteDto addPostFavorite)
    {
        try
        {
            // var favpost = await _context.PostFavorites.FirstOrDefaultAsync(e =>
            //     e.PostId == addPostFavorite.PostId && e.UserId == addPostFavorite.UserId);
            // if (favpost == null)
            // {
            //     var post = _mapper.Map<PostFavorite>(addPostFavorite);
            //     await _context.PostFavorites.AddAsync(post);
            //     await _context.SaveChangesAsync();
            //     return new Response<bool>(true);
            // }
            //
            // _context.PostFavorites.Remove(favpost);
            // await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeletePostFavorite(int id)
    {
        try
        {
            var post = await _context.PostFavorites.FindAsync(id);
            if (post == null) return new Response<bool>(HttpStatusCode.BadRequest, "Post not found");
            _context.PostFavorites.Remove(post);
            await _context.SaveChangesAsync();
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.BadRequest, e.Message);
        }
    }
}