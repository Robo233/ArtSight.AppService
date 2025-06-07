using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Models.Requests;

public class BaseEntityFilterRequest
{
    [FromQuery]
    public string? SearchString { get; set; }

    [FromQuery]
    public bool? IsFavorited { get; set; }

    [FromQuery]
    public bool? IsCreatedByUser { get; set; }

    [FromQuery]
    public bool? IsRecentlyViewed { get; set; }

    [FromQuery]
    public string LanguageCode { get; set; } = "en";

}
