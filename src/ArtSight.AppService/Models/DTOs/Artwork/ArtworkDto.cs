namespace ArtSight.AppService.Models.DTOs.Artwork;

public class ArtworkDto : PageEntityDto
{
    public Guid? ArtistId { get; set; }
    public string? ArtistName { get; set; }
    public Guid? ExhibitionId { get; set; }
    public string? ExhibitionName { get; set; }
    public Dictionary<Guid, string?>? Genres { get; set; }
    public string? Medium { get; set; }
    public string? Dimensions { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

}
