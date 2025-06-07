using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Models.Requests;

public class LocalizedListRequest
{
    [FromQuery]
    public int Offset { get; set; }

    [FromQuery]
    public int Limit { get; set; }

    [FromQuery]
    public string? ParentId { get; set; }

    [FromQuery]
    [DefaultValue("en")]
    public string LanguageCode { get; set; } = "en";

}
