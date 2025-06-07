using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Interfaces.Processors;

public interface IFavoriteProcessor
{
    Task<ActionResult> AddFavoriteAsync(EntityActionRequest request);
    Task<ActionResult> RemoveFavoriteAsync(EntityActionRequest request);

}
