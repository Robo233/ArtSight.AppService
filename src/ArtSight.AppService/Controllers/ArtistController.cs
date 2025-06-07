using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Models.DTOs.Artist;
using ArtSight.AppService.Models.Requests;
using ArtSight.AppService.Models.DTOs;

namespace ArtSight.AppService.Controllers;

[ApiController]
[Route("artsight/artists")]
public class ArtistController : Controller
{
    private readonly IArtistProcessor _artistProcessor;

    public ArtistController(IArtistProcessor artistProcessor)
    {
        _artistProcessor = artistProcessor;
    }

    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtistList(LocalizedListRequest request)
    {
        return await _artistProcessor.GetArtistListAsync(request);
    }

    [HttpGet]
    [Route("{Id}")]
    public async Task<ActionResult<ArtistDto>> GetArtist(LocalizedRequest request)
    {
        return await _artistProcessor.GetArtistAsync(request);
    }

}
