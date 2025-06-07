using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.DTOs.Exhibition;
using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Controllers;

[ApiController]
[Route("artsight/exhibitions")]
public class ExhibitionController : ControllerBase
{
    private readonly IExhibitionProcessor _exhibitionProcessor;

    public ExhibitionController(IExhibitionProcessor exhibitionProcessor)
    {
        _exhibitionProcessor = exhibitionProcessor;
    }

    [HttpGet]
    [Route("{Id}")]
    public async Task<ActionResult<ExhibitionDto>> GetExhibition(LocalizedRequest request)
    {
        return await _exhibitionProcessor.GetExhibitionAsync(request);
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<AllPageEntitesWithCoordinatesCardDto>> GetAllExhibitions(ByLanguageCodeRequest request)
    {
        return await _exhibitionProcessor.GetAllExhibitionsAsync(request);
    }

    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetExhibitionList(LocalizedListRequest request)
    {
        return await _exhibitionProcessor.GetExhibitionListAsync(request);
    }

}
