using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Post;

public class Tag
{
    public int TagId { get; set; }
    [MaxLength(45)]
    public string TagName { get; set; }
    public List<PostTag> PostTags { get; set; }
}