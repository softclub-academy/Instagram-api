using System.ComponentModel.DataAnnotations;
using Domain.Entities.Post;
using Domain.Enums;

namespace Domain.Entities.User;

public class UserProfile
{
    public int UserProfileId { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    [MaxLength(45)]
    public string FirstName { get; set; }
    [MaxLength(45)]
    public string LastName { get; set; }
    public int LocationId { get; set; }
    public string Image { get; set; }
    public Location Location { get; set; }
    public Gender Gender { get; set; }
    public DateTime DOB { get; set; }
    [MaxLength(45)]
    public string Occupation { get; set; }
    public string About { get; set; }
    public DateTime DateUpdated { get; set; }
    public List<Image> Images { get; set; }
}