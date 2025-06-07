using System.Security.Claims;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Interfaces.Services;

namespace ArtSight.AppService.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FavoriteService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> IsEntityFavoritedAsync(Guid entityId, string entityType)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            return false;
        }

        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userIdGuid))
        {
            return false;
        }

        var appUser = await _userRepository.GetUserByIdAsync(userIdGuid);
        if (appUser == null)
        {
            return false;
        }

        return entityType switch
        {
            "artwork" => appUser.FavoriteArtworks?.Contains(entityId) ?? false,
            "artworkDetail" => appUser.FavoriteArtworkDetails?.Contains(entityId) ?? false,
            "exhibition" => appUser.FavoriteExhibitions?.Contains(entityId) ?? false,
            "artist" => appUser.FavoriteArtists?.Contains(entityId) ?? false,
            "genre" => appUser.FavoriteGenres?.Contains(entityId) ?? false,
            _ => false
        };
    }
}
