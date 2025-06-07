using ArtSight.Core.Models;

namespace ArtSight.AppService.Models.DTOs.Artist;

public class ArtistDto : PageEntityDto
{
    public string? DateOfBirth { get; set; }
    public string? DateOfDeath { get; set; }
    public Dictionary<Guid, string?>? Genres { get; set; }
    public List<string?> ImageDescriptions { get; set; } = [];
    public ContactInfo ContactInfo { get; set; } = new();

}
