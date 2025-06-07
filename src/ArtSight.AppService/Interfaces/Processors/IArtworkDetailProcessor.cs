using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Models.DTOs.ArtworkDetail;
using ArtSight.AppService.Models.DTOs;

namespace ArtSight.AppService.Interfaces.Processors;

public interface IArtworkDetailProcessor
{
    Task<ActionResult<ArtworkDetailDto>> GetArtworkDetailAsync(LocalizedRequest request);
    Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkDetailListAsync(LocalizedListRequest request);
    Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkDetailListByArtworkIdAsync(LocalizedListRequest request);

}
