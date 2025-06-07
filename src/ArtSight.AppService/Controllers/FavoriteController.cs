using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Controllers;

[ApiController]
[Route("artsight/favorites")]
[Authorize]
public class FavoriteController : ControllerBase
{
    private readonly IFavoriteProcessor _favoritesProcessor;

    public FavoriteController(IFavoriteProcessor favoritesProcessor)
    {
        _favoritesProcessor = favoritesProcessor;
    }

    [HttpPost]
    [Route("add/{Id}/{EntityType}")]
    public async Task<IActionResult> AddFavorite(EntityActionRequest request)
    {
        return await _favoritesProcessor.AddFavoriteAsync(request);
    }

    [HttpDelete]
    [Route("remove/{Id}/{EntityType}")]
    public async Task<IActionResult> RemoveFavorite(EntityActionRequest request)
    {
        return await _favoritesProcessor.RemoveFavoriteAsync(request);
    }

}
