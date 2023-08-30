using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Post;

public class StatUserId
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public User.User User { get; set; }
    public int PostStatId { get; set; }
    public PostStat PostStat { get; set; }
}