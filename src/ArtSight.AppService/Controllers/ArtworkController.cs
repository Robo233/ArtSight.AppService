using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Models.DTOs.Artwork;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.DTOs.Exhibition;

namespace ArtSight.AppService.Controllers;

[ApiController]
[Route("artsight/artworks")]
public class ArtworkController : Controller
{
    private readonly IArtworkProcessor _artworkProcessor;

    public ArtworkController(IArtworkProcessor artworkProcessor)
    {
        _artworkProcessor = artworkProcessor;
    }

    [HttpGet]
    [Route("{Id}")]
    public async Task<ActionResult<ArtworkDto>> GetArtwork(LocalizedRequest request)
    {
        return await _artworkProcessor.GetArtworkAsync(request);
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<AllPageEntitesWithCoordinatesCardDto>> GetAllArtworks(ByLanguageCodeRequest request)
    {
        return await _artworkProcessor.GetAllArtworksAsync(request);
    }

    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworksList(LocalizedListRequest request)
    {
        return await _artworkProcessor.GetArtworkListAsync(request);
    }

    [HttpGet]
    [Route("artistId")]
    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworksListByArtistId(LocalizedListRequest request)
    {
        return await _artworkProcessor.GetArtworkListByArtistIdAsync(request);
    }

    [HttpGet]
    [Route("exhibitionId")]
    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworksListByExhibitionId(LocalizedListRequest request)
    {
        return await _artworkProcessor.GetArtworkListByExhibitionIdAsync(request);
    }

    [HttpGet]
    [Route("genreId")]
    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworksListByGenreId(LocalizedListRequest request)
    {
        return await _artworkProcessor.GetArtworkListByGenreIdAsync(request);
    }
}
