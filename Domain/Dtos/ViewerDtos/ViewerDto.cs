namespace Domain.Dtos.ViewerDtos;

public class ViewerDto
{
    public string UserName { get; set; }
    public string Name { get; set; }
    public int? ViewCount { get; set; }
    public int? ViewLike { get; set; }
}