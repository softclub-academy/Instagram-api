using System.ComponentModel.DataAnnotations;
using Domain.Entities.Post;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.User;

public class User : IdentityUser
{
    public DateTime DateRegistred { get; set; }
    [MaxLength(45)]
    public UserType UserType { get; set; }
    [MaxLength(45)]
    public string? AccountStatus { get; set; }
    public UserSetting UserSetting { get; set; }
    public List<UserProfile> UserProfiles { get; set; }
    public ExternalAccount ExternalAccount { get; set; }
    public List<FollowingRelationShip> FollowingRelationShips { get; set; }
    public List<Post.Post> Posts { get; set; }
    public List<PostComment> PostComments { get; set; }
    public List<PostFavorite> PostFavorites { get; set; }
    public List<PostUserLike> StatUserIds { get; set; }
}