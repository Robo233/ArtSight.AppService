namespace ArtSight.AppService.Models.DTOs;

public class SearchResponseDto
{
    public string? EntityType { get; set; }
    public PaginatedResult<PageEntityCardDto>? Result { get; set; }

}
