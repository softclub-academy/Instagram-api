using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.LocationDto;

public class GetLocationDto
{
    public int LocationId { get; set; }
    [Required]
    public string City { get; set; } = null!;
    [Required]
    public string State { get; set; } = null!;
    [Required]
    public string ZipCode { get; set; } = null!;
    [Required]
    public string Country { get; set; } = null!;
}