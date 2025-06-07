using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Models.DTOs.Artwork;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.DTOs.Exhibition;

namespace ArtSight.AppService.Interfaces.Processors;

public interface IArtworkProcessor
{
    Task<ActionResult<ArtworkDto>> GetArtworkAsync(LocalizedRequest request);
    Task<ActionResult<AllPageEntitesWithCoordinatesCardDto>> GetAllArtworksAsync(ByLanguageCodeRequest request);
    Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkListAsync(LocalizedListRequest request);
    Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkListByArtistIdAsync(LocalizedListRequest request);
    Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkListByExhibitionIdAsync(LocalizedListRequest request);
    Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkListByGenreIdAsync(LocalizedListRequest request);

}
