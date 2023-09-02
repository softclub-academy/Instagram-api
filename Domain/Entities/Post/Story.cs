using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Story
{
    [Key]
    public int Id { get; set; }
    public List<string> Images { get; set; }
    public int? PostId { get; set; }
    public Post.Post Post{ get; set; }
    public StoryStat StoryStat{ get; set; }
}