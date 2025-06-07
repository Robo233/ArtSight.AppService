using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Models.DTOs.Artist;
using ArtSight.AppService.Models.DTOs;

namespace ArtSight.AppService.Interfaces.Processors;

public interface IArtistProcessor
{
    Task<ActionResult<ArtistDto>> GetArtistAsync(LocalizedRequest request);
    Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtistListAsync(LocalizedListRequest request);

}
