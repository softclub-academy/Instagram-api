using System.Net;
using Domain.Dtos.PostCommentDto;
using Domain.Dtos.PostDto;
using Domain.Dtos.PostFavoriteDto;
using Domain.Filters.PostCommentFilter;
using Domain.Filters.PostFilter;
using Domain.Responses;
using Infrastructure.Services.PostService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class PostController : BaseController
{
    private readonly IPostService _service;

    public PostController(IPostService service)
    {
        _service = service;
    }

    [HttpGet("get-posts")]
    public async Task<IActionResult> GetPosts([FromQuery]PostFilter filter)
    {
        var userId = User.Claims.FirstOrDefault(u => u.Type == "sid")!.Value;
        var result = await _service.GetPosts(filter, userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-post-by-id")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var result = await _service.GetPostById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-following-post")]
    public async Task<IActionResult> GetFollowingPost([FromQuery]PostFollowingFilter filter)
    {
        var result = await _service.GetPostByFollowing(filter);
        return StatusCode(result.StatusCode, result);
    }


    [HttpPost("add-post")]
    public async Task<IActionResult> AddPost([FromForm]AddPostDto post)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(e=>e.Type=="sid")!.Value;
            var result = await _service.AddPost(post, userId);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<PostDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-post")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var result = await _service.DeletePost(id);
        return StatusCode(result.StatusCode, result);
    }
    
    
    [HttpPost("like-Post")]
    public async Task<IActionResult> LikePost(int postId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
        if (userId == null)
        {
            var response = new Response<bool>(HttpStatusCode.BadRequest, "UserNotfound");
            return StatusCode(response.StatusCode, response);
        }
        var result = await _service.LikePost(userId,postId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("view_post")]
    public async Task<IActionResult> ViewPost(int postId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "sid")!.Value;
        var result = await _service.ViewPost(userId, postId);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpGet("get-postcomments")]
    public async Task<IActionResult> GetPostComments([FromQuery]PostCommentFilter filter)
    {
        var result = await _service.GetPostComments(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-postcomment-by-id")]
    public async Task<IActionResult> GetPostCommentById(int id)
    {
        var result = await _service.GetPostCommentById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add_comment")]
    public async Task<IActionResult> AddComment([FromBody]AddPostCommentDto comment)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == "sid").Value;
            var result = await _service.AddComment(comment, userId);
            return StatusCode(result.StatusCode, result);
        }

        var respone = new Response<bool>(HttpStatusCode.BadRequest, ModelStateErrors());
        return StatusCode(respone.StatusCode, respone);
    }

    [HttpDelete("delete_comment")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var result = await _service.DeleteComment(commentId);
        return StatusCode(result.StatusCode, result);
    }
    
    [HttpPost("add-PostFavorite")]
    public async Task<IActionResult> AddPostFavorite([FromBody]AddPostFavoriteDto postFavorite)
    {
        if (ModelState.IsValid)
        {
            var userId=User.Claims.FirstOrDefault(e=>e.Type=="sid").Value;
            var result = await _service.AddPostFavorite(postFavorite, userId);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<PostFavoriteDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }
}