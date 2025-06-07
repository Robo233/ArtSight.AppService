using ArtSight.Core.Models;

namespace ArtSight.AppService.Models.DTOs.Exhibition;

public class ExhibitionDto : PageEntityDto
{
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Address { get; set; }
    public Dictionary<Guid, string?>? Genres { get; set; }
    public List<string?> ImageDescriptions { get; set; } = [];
    public ExhibitionSchedule Schedule { get; set; } = new();
    public ContactInfo ContactInfo { get; set; } = new();

}
