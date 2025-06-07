using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Models.DTOs.Exhibition;
using ArtSight.AppService.Models.DTOs;

namespace ArtSight.AppService.Interfaces.Processors;

public interface IExhibitionProcessor
{
    Task<ActionResult<ExhibitionDto>> GetExhibitionAsync(LocalizedRequest request);
    Task<ActionResult<AllPageEntitesWithCoordinatesCardDto>> GetAllExhibitionsAsync(ByLanguageCodeRequest request);
    Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetExhibitionListAsync(LocalizedListRequest request);

}
