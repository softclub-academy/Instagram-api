using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class UserLog
{
    [Key]
    public int LogId { get; set; }
    public int UserId { get; set; }
    public User.User User { get; set; }
    public DateTime LoginDate { get; set; }
    public DateTime? LogoutDate { get; set; }
}