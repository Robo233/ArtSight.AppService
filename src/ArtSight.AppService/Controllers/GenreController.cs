using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Models.DTOs.Genre;
using ArtSight.AppService.Models.DTOs;

namespace ArtSight.AppService.Controllers;

[ApiController]
[Route("artsight/genres")]
public class GenreController : Controller
{
    private readonly IGenreProcessor _genreProcessor;

    public GenreController(IGenreProcessor genreProcessor)
    {
        _genreProcessor = genreProcessor;
    }

    [HttpGet]
    [Route("{Id}")]
    public async Task<ActionResult<GenreDto>> GetGenre(LocalizedRequest request)
    {
        return await _genreProcessor.GetGenreAsync(request);
    }

    [HttpGet]
    [Route("list")]
    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetGenreList(LocalizedListRequest request)
    {
        return await _genreProcessor.GetGenreListAsync(request);
    }

}
