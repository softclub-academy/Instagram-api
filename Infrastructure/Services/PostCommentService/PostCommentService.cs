using System.Net;
using AutoMapper;
using Domain.Dtos.PostCommentDto;
using Domain.Entities.Post;
using Domain.Filters.PostCommentFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.PostCommentService;

public class PostCommentService : IPostCommentService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PostCommentService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

    public async Task<Response<GetPostCommentDto>> AddPostComment(AddPostCommentDto addPostComment)
    {
        try
        {
            var user = await _context.UserLogs.FirstOrDefaultAsync(u => u.UserId == addPostComment.UserId && u.LogoutDate == null);
            if (user == null) return new Response<GetPostCommentDto>(HttpStatusCode.BadRequest, "Login or register first!!!");
            var post = await _context.Posts.FindAsync(addPostComment.PostId);
            if (post == null)
                return new Response<GetPostCommentDto>(HttpStatusCode.BadRequest, "Post not found");
            var comment = _mapper.Map<PostComment>(addPostComment);
            await _context.PostComments.AddAsync(comment);
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<GetPostCommentDto>(comment);
            return new Response<GetPostCommentDto>(mapped);
        }
        catch (Exception e)
        {
            return new Response<GetPostCommentDto>(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<Response<bool>> DeletePostComment(int id)
    {
        try
        {
            var comment = await _context.PostComments.FindAsync(id);
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