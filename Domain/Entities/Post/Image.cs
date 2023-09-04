﻿using System.ComponentModel.DataAnnotations;
using Domain.Entities.User;

namespace Domain.Entities.Post;

public class Image
{
    [Key]
    public int? ImageId { get; set; }
    public int? UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
    public int? PostId { get; set; }
    public Post? Post { get; set; }
    [MaxLength(250)]
    public string Path { get; set; }
}