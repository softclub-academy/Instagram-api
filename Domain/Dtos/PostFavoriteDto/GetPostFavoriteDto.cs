namespace Domain.Dtos.PostFavoriteDto;

public class GetPostFavoriteDto : PostFavoriteDto
{
    public string UserId { get; set; }
    public int PostFavoriteId { get; set; }
    public DateTime DateFavorited { get; set; }
}