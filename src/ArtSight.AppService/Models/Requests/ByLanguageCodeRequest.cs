using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace ArtSight.AppService.Models.Requests;

public class ByLanguageCodeRequest
{
    [FromQuery]
    [DefaultValue("en")]
    public string LanguageCode { get; set; } = "en";

}
