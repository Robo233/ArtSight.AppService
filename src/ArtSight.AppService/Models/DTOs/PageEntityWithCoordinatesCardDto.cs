namespace ArtSight.AppService.Models.DTOs;

public class PageEntityWithCoordinatesCardDto : PageEntityCardDto
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Address { get; set; }

}
