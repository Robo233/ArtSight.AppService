using System.Security.Claims;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Interfaces.Services;

namespace ArtSight.AppService.Services;

public class RecentlyViewedService : IRecentlyViewedService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RecentlyViewedService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public void AddRecentlyViewedEntity(Guid entityId, string entityType)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User;
        if (userClaims?.Identity?.IsAuthenticated != true)
        {
            return;
        }

        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? userClaims.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userIdGuid))
        {
            return;
        }

        var appUser = _userRepository.GetUserByIdAsync(userIdGuid).GetAwaiter().GetResult();
        if (appUser == null)
        {
            return;
        }

        switch (entityType)
        {
            case "artwork":
                appUser.RecentlyViewedArtworks.Remove(entityId);
                appUser.RecentlyViewedArtworks.Add(entityId);
                break;
            case "artworkDetail":
                appUser.RecentlyViewedArtworkDetails.Remove(entityId);
                appUser.RecentlyViewedArtworkDetails.Add(entityId);
                break;
            case "exhibition":
                appUser.RecentlyViewedExhibitions.Remove(entityId);
                appUser.RecentlyViewedExhibitions.Add(entityId);
                break;
            case "artist":
                appUser.RecentlyViewedArtists.Remove(entityId);
                appUser.RecentlyViewedArtists.Add(entityId);
                break;
            case "genre":
                appUser.RecentlyViewedGenres.Remove(entityId);
                appUser.RecentlyViewedGenres.Add(entityId);
                break;
            default:
                break;
        }

        _userRepository.ReplaceUserAsync(appUser).GetAwaiter().GetResult();
    }
}
