using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS8618

namespace ArtSight.AppService.Models.Requests;

public class EntityActionRequest
{
    [FromRoute]
    public string Id { get; set; }

    [FromRoute]
    public string? EntityType { get; set; }

}
