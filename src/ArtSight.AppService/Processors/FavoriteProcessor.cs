using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Interfaces.Repositories;
using System.Security.Claims;
using ArtSight.AppService.Interfaces.Services;

namespace ArtSight.AppService.Processors;

public class FavoriteProcessor : IFavoriteProcessor
{
    private readonly IUserRepository _userRepository;
    private readonly ILocalizationService _localizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FavoriteProcessor(IUserRepository userRepository, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _localizationService = localizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ActionResult> AddFavoriteAsync(EntityActionRequest request)
    {
        try
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;

            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new UnauthorizedResult();
            }

            if (!Guid.TryParse(userId, out var userIdGuid))
            {
                return new BadRequestObjectResult("Invalid user ID format.");
            }

            var isUpdated = await _userRepository.AddToFavoritesAsync(userIdGuid, Guid.Parse(request.Id), request.EntityType!);

            return isUpdated ? new OkResult() : new BadRequestObjectResult("Failed to add entity to favorites or entity is already in favorites.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] AddFavoriteAsync: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ActionResult> RemoveFavoriteAsync(EntityActionRequest request)
    {
        try
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? user?.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new UnauthorizedResult();
            }

            if (!Guid.TryParse(userId, out var userIdGuid))
            {
                return new BadRequestObjectResult("Invalid user ID format.");
            }

            var isUpdated = await _userRepository.RemoveFromFavoritesAsync(userIdGuid, Guid.Parse(request.Id), request.EntityType!);

            return isUpdated
                ? new OkResult()
                : new BadRequestObjectResult("Failed to remove entity from favorites or entity is not in favorites.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] RemoveFavoriteAsync: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

}
