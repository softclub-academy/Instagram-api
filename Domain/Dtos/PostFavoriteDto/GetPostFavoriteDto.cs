namespace Domain.Dtos.PostFavoriteDto;

public class GetPostFavoriteDto : PostFavoriteDto
{
    public int PostFavoriteId { get; set; }
    public DateTime DateFavorited { get; set; }
}