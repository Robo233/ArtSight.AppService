using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Controllers;

[ApiController]
[Route("artsight/entityfilter")]
public class EntityFilterController : Controller
{
    private readonly IEntityFilterProcessor _entityFilterProcessor;

    public EntityFilterController(IEntityFilterProcessor entityFilterProcessor)
    {
        _entityFilterProcessor = entityFilterProcessor;
    }

    [HttpGet]
    public async Task<ActionResult<List<SearchResponseDto>>> GlobalFilter(BaseEntityFilterRequest request)
    {
        return await _entityFilterProcessor.GlobalFilterAsync(request);
    }

    [HttpGet]
    [Route("by-entity")]
    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> FilterByEntity(EntityFilterRequest request)
    {
        return await _entityFilterProcessor.FilterEntitiesAsync(request);
    }

}
