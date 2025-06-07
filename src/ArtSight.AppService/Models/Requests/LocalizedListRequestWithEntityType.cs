using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Models.Requests;

public class LocalizedListRequestWithEntityType
{
    [FromQuery]
    public int Offset { get; set; }

    [FromQuery]
    public int Limit { get; set; }

    [FromQuery]
    public string? EntityType { get; set; }

    [FromQuery]
    [DefaultValue("en")]
    public string LanguageCode { get; set; } = "en";

}
