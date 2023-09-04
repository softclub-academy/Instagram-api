using Domain.Entities;
using Domain.Entities.Post;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.StoryDtos;

public class StoryDto
{
   
    public int? PostId { get; set; }
}