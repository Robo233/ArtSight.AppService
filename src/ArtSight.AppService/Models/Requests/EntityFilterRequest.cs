using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Models.Requests;

public class EntityFilterRequest : BaseEntityFilterRequest
{
    [FromQuery]
    public string? EntityType { get; set; }

    [FromQuery]
    public int Offset { get; set; } = 0;

}
