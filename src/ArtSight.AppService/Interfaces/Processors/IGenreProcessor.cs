using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Models.DTOs.Genre;
using ArtSight.AppService.Models.DTOs;

namespace ArtSight.AppService.Interfaces.Processors;

public interface IGenreProcessor
{
    Task<ActionResult<GenreDto>> GetGenreAsync(LocalizedRequest request);
    Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetGenreListAsync(LocalizedListRequest request);

}
