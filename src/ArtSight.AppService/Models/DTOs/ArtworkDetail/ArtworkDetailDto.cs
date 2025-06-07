namespace ArtSight.AppService.Models.DTOs.ArtworkDetail;

public class ArtworkDetailDto : PageEntityDto
{
    public Guid ArtworkId { get; set; }
    public string? ArtworkName { get; set; }

}
