using Domain.Dtos.PostCommentDto;
using Domain.Dtos.PostDto;
using Domain.Dtos.PostFavoriteDto;
using Domain.Filters;
using Domain.Filters.PostCommentFilter;
using Domain.Filters.PostFilter;
using Domain.Responses;

namespace Infrastructure.Services.PostService;

public interface IPostService
{
    Task<PagedResponse<List<GetPostDto>>> GetPosts(PostFilter filter, string userId);
    Task<Response<GetPostDto>> GetPostById(int id, string userId);
    Task<PagedResponse<List<GetPostDto>>> GetPostByFollowing(PostFollowingFilter filter, string userId);
    Task<Response<int>> AddPost(AddPostDto addPost, string userId);
    Task<Response<bool>> DeletePost(int id);
    Task<Response<bool>> LikePost(string? userId,int postId);
    Task<Response<bool>> ViewPost(string userId, int postId);
    Task<PagedResponse<List<GetPostCommentDto>>> GetPostComments(PostCommentFilter filter);
    Task<Response<GetPostCommentDto>> GetPostCommentById(int id);
    Task<Response<bool>> AddComment(AddPostCommentDto comment, string userId);
    Task<Response<bool>> DeleteComment(int commentId);
    Task<PagedResponse<List<GetPostFavoriteDto>>> GetPostFavorites(PaginationFilter filter, string userId);
    Task<Response<GetPostFavoriteDto>> GetPostFavoriteById(int id);
    Task<Response<bool>> AddPostFavorite(AddPostFavoriteDto addPostFavorite, string userId);
    Task<Response<bool>> DeletePostFavorite(int id);
}
