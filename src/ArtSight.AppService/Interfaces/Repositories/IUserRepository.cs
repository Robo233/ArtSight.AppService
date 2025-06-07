using ArtSight.AppService.Models.DTOs;
using ArtSight.Core.Models;

namespace ArtSight.AppService.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> FindUserByEmailAsync(string email);
    Task<Guid> AddUserAsync(User user);
    Task<bool> AddToFavoritesAsync(Guid userId, Guid entityId, string entityType);
    Task<bool> RemoveFromFavoritesAsync(Guid userId, Guid entityId, string entityType);
    Task<PaginatedResult<Guid>> GetFavoriteEntityIdsAsync(Guid userId, string entityType, int offset, int limit);
    Task<PaginatedResult<Guid>> GetRecentlyViewedEntityIdsAsync(Guid userId, string entityType, int offset, int limit);
    Task<bool> ReplaceUserAsync(User user);

}
