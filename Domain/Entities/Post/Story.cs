using System.ComponentModel.DataAnnotations;
using Domain.Entities.Post;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Story
{
    [Key]
    public int Id { get; set; }
    public string FileName { get; set; }
    public int? PostId { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; }

    public List<StoryView> StoryViews { get; set; }
    public Post.Post Post{ get; set; }
    public StoryStat StoryStat{ get; set; }
    public User.User User { get; set; }
}