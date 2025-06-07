namespace ArtSight.AppService.Models.DTOs;

public class PageEntityDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? DescriptionLanguageCode { get; set; }
    public double? MainImageAspectRatio { get; set; }
    public bool IsFavorite { get; set; }
    public bool IsOwner { get; set; }

}
