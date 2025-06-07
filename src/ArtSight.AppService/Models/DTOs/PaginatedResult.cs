namespace ArtSight.AppService.Models.DTOs;

public class PaginatedResult<TEntity>
{
    public List<TEntity>? Items { get; set; } = [];
    public bool HasMore { get; set; }

}
