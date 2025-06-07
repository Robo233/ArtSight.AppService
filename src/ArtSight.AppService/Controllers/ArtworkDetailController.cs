using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Models.DTOs.ArtworkDetail;
using ArtSight.AppService.Models.DTOs;

namespace ArtSight.AppService.Controllers;

[ApiController]
[Route("artsight/artworkDetails")]
public class ArtworkDetailController : Controller
{
    private readonly IArtworkDetailProcessor _artworkDetailProcessor;

    public ArtworkDetailController(IArtworkDetailProcessor artworkDetailProcessor)
    {
        _artworkDetailProcessor = artworkDetailProcessor;
    }

    [HttpGet]
    [Route("{Id}")]
    public async Task<ActionResult<ArtworkDetailDto>> GetArtworkDetail(LocalizedRequest request)
    {
        return await _artworkDetailProcessor.GetArtworkDetailAsync(request);
    }

    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkDetailsList(LocalizedListRequest request)
    {
        return await _artworkDetailProcessor.GetArtworkDetailListAsync(request);
    }

    [HttpGet]
    [Route("artworkId")]
    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkDetailsListByArtworkId(LocalizedListRequest request)
    {
        return await _artworkDetailProcessor.GetArtworkDetailListByArtworkIdAsync(request);
    }

}
