using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Models.Requests;

public class LocalizedRequest
{
    [FromRoute]
    public string? Id { get; set; }

    [FromQuery]
    public string LanguageCode { get; set; } = "en";

}
