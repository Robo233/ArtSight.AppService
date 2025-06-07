using ArtSight.AppService.Setup;
using ArtSight.Core.MongoDb.Setup;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.Core.Models;
using ArtSight.Core.ModelsBase;
using ArtSight.Core.MongoDb.Interfaces;
using MongoDB.Driver;
using ArtSight.AppService.Models.DTOs;

namespace ArtSight.AppService.Repositories;

public class UserRepository : MongoOperations<AppDbConnectionSettings, User, Guid, EntityTracking>, IUserRepository
{
    private readonly IMongoConnection<AppDbConnectionSettings> _mongoConnection;

    public UserRepository(IMongoConnection<AppDbConnectionSettings> mongoConnection) : base(mongoConnection)
    {
        _mongoConnection = mongoConnection;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await GetByIdAsync(userId);
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        var filter = Builders<User>.Filter.Eq(a => a.Email, email);
        var user = await FindEntityAsync(filter);
        return user;
    }

    public async Task<Guid> AddUserAsync(User user)
    {
        return await AddEntityAsync(user);
    }

    private static string? FavoritesField(string entityType)
    {
        string favoritesField = entityType switch
        {
            "artwork" => nameof(User.FavoriteArtworks),
            "artworkDetail" => nameof(User.FavoriteArtworkDetails),
            "exhibition" => nameof(User.FavoriteExhibitions),
            "artist" => nameof(User.FavoriteArtists),
            "genre" => nameof(User.FavoriteGenres),
            _ => throw new ArgumentException("Invalid entity type.")
        };
        return favoritesField;
    }

    public async Task<bool> AddToFavoritesAsync(Guid userId, Guid entityId, string entityType)
    {
        try
        {
            string? favoritesField = FavoritesField(entityType);

            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.AddToSet(favoritesField, entityId);
            var updateResult = await UpdateEntityAsync(userId, update);

            return updateResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] AddToFavoritesAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveFromFavoritesAsync(Guid userId, Guid entityId, string entityType)
    {
        try
        {
            string? favoritesField = FavoritesField(entityType);

            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Pull(favoritesField, entityId);
            var updateResult = await UpdateEntityAsync(userId, update);

            return updateResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] RemoveFromFavoritesAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<PaginatedResult<Guid>> GetFavoriteEntityIdsAsync(Guid userId, string entityType, int offset, int limit)
    {
        try
        {
            string? favoritesField = FavoritesField(entityType);

            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var projection = Builders<User>.Projection.Include(favoritesField);
            var user = await _mongoConnection
                        .GetCollection<User>()
                        .Find(filter)
                        .FirstOrDefaultAsync();

            if (user == null)
            {
                return new PaginatedResult<Guid>
                {
                    Items = [],
                    HasMore = false
                };
            }

            var favoriteIds = entityType switch
            {
                "artwork" => user.FavoriteArtworks ?? [],
                "artworkDetail" => user.FavoriteArtworkDetails ?? [],
                "exhibition" => user.FavoriteExhibitions ?? [],
                "artist" => user.FavoriteArtists ?? [],
                "genre" => user.FavoriteGenres ?? [],
                _ => []
            };

            var pagedItems = favoriteIds
                .Skip(offset)
                .Take(limit)
                .ToList();

            bool hasMore = offset + limit < favoriteIds.Count;

            return new PaginatedResult<Guid>
            {
                Items = pagedItems,
                HasMore = hasMore
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetFavoriteEntityIdsAsync: {ex.Message}");
            return new PaginatedResult<Guid>
            {
                Items = [],
                HasMore = false
            };
        }
    }

    public async Task<PaginatedResult<Guid>> GetRecentlyViewedEntityIdsAsync(Guid userId, string entityType, int offset, int limit)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var user = await _mongoConnection
                        .GetCollection<User>()
                        .Find(filter)
                        .FirstOrDefaultAsync();

            if (user == null)
            {
                return new PaginatedResult<Guid>
                {
                    Items = [],
                    HasMore = false
                };
            }

            var recentlyViewedIds = entityType.ToLower() switch
            {
                "artwork" => user.RecentlyViewedArtworks ?? [],
                "artworkdetail" => user.RecentlyViewedArtworkDetails ?? [],
                "exhibition" => user.RecentlyViewedExhibitions ?? [],
                "artist" => user.RecentlyViewedArtists ?? [],
                "genre" => user.RecentlyViewedGenres ?? [],
                _ => []
            };

            var pagedItems = recentlyViewedIds
                .Skip(offset)
                .Take(limit)
                .ToList();

            bool hasMore = offset + limit < recentlyViewedIds.Count;

            return new PaginatedResult<Guid>
            {
                Items = pagedItems,
                HasMore = hasMore
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetRecentlyViewedEntityIdsAsync: {ex.Message}");
            return new PaginatedResult<Guid>
            {
                Items = [],
                HasMore = false
            };
        }
    }

    public async Task<bool> ReplaceUserAsync(User user)
    {
        return await base.ReplaceEntityAsync(user);
    }

}
