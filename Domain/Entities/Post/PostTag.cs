using Microsoft.EntityFrameworkCore.Metadata;

namespace Domain.Entities.Post;

public class PostTag
{
    public int PostTagId { get; set; }
    public int TagId { get; set; }
    public Tag Tag { get; set; }
    public int PodtId { get; set; }
    public Post Post { get; set; }
}