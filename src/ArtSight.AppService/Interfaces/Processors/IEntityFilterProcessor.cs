using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Interfaces.Processors;

public interface IEntityFilterProcessor
{
    Task<ActionResult<PaginatedResult<PageEntityCardDto>>> FilterEntitiesAsync(EntityFilterRequest request);
    Task<ActionResult<List<SearchResponseDto>>> GlobalFilterAsync(BaseEntityFilterRequest request);

}
