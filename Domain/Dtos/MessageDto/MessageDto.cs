using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.MessageDto;

public class MessageDto
{
    [Required]
    public int ChatId { get; set; }
    public string? MessageText { get; set; }
    public IFormFile? File { get; set; }
}