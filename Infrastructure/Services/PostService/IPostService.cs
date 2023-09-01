using Domain.Dtos.PostDto;
using Domain.Filters.PostFilter;
using Domain.Filters.UserFilter;
using Domain.Responses;

namespace Infrastructure.Services.PostService;

public interface IPostService
{
    Task<PagedResponse<List<GetPostDto>>> GetPosts(PostFilter filter);
    Task<Response<GetPostDto>> GetPostById(int id);
    Task<PagedResponse<List<GetPostDto>>> GetPostByFollowing(PostFollowingFilter filter);
    Task<Response<GetPostDto>> AddPost(AddPostDto addPost);
    Task<Response<GetPostDto>> UpdatePost(AddPostDto addPost);
    Task<Response<bool>> DeletePost(int id);
    Task<Response<bool>> LikePost(string? userId,int postId);
}