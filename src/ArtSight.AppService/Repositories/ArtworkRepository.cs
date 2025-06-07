using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.Core.Models;
using ArtSight.Core.MongoDb.Interfaces;
using ArtSight.AppService.Setup;
using MongoDB.Driver;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Interfaces.Services;

namespace ArtSight.AppService.Repositories;

public class ArtworkRepository : PageEntityMongoOperations<Artwork>, IArtworkRepository
{
    private readonly IMongoConnection<AppDbConnectionSettings> _mongoConnection;

    public ArtworkRepository(IMongoConnection<AppDbConnectionSettings> mongoConnection, ILocalizationService localizationService) : base(mongoConnection, localizationService)
    {
        _mongoConnection = mongoConnection;
    }

    public async Task<Artwork?> GetByIdAsync(Guid artworkId)
    {
        return await base.GetByIdAsync(artworkId);
    }

    public async Task<List<Artwork>> GetListByIdsAsync(List<Guid> ids)
    {
        return await GetByIdsAsync(ids);
    }

    public async Task<List<Artwork>?> GetAllArtworksAsync()
    {
        var filter = Builders<Artwork>.Filter.Empty;
        var artworks = await FindEntitiesAsync(filter);
        return artworks;
    }

    public async Task<PaginatedResult<Artwork>> GetPagedListAsync(Guid? parentId, int offset, int limit)
    {
        return await base.GetPagedListAsync(
            parentId: parentId,
            parentSelector: null,
            offset: offset,
            limit: limit,
            sortSelector: x => x.Tracking.CreatedUtc);
    }

    public async Task<PaginatedResult<Artwork>> GetPagedListAsync(Guid? parentId, string parentEntityType, int offset, int limit)
    {
        var filter = Builders<Artwork>.Filter.Empty;
        if (parentId.HasValue)
        {
            filter = parentEntityType.ToLower() switch
            {
                "artist" => Builders<Artwork>.Filter.Eq(a => a.ArtistId, parentId.Value),
                "exhibition" => Builders<Artwork>.Filter.Eq(a => a.ExhibitionId, parentId.Value),
                "genre" => Builders<Artwork>.Filter.AnyEq(a => a.GenreIds, parentId.Value),
                _ => throw new ArgumentException($"Invalid parentEntityType: {parentEntityType}"),
            };
        }

        var sortDefinition = Builders<Artwork>.Sort.Combine(
            Builders<Artwork>.Sort.Ascending(x => x.Tracking.CreatedUtc),
            Builders<Artwork>.Sort.Ascending(x => x.Id)
        );

        var findOptions = new FindOptions<Artwork>
        {
            Skip = offset,
            Limit = limit,
            Sort = sortDefinition
        };

        var results = await FindEntitiesAsync(filter, findOptions);

        var collection = _mongoConnection.GetCollection<Artwork>();
        var totalCount = await collection.CountDocumentsAsync(filter);

        bool hasMore = (offset + limit) < totalCount;

        return new PaginatedResult<Artwork>
        {
            Items = results,
            HasMore = hasMore
        };
    }

    public async new Task<PaginatedResult<PageEntityCardDto>> FilterAsync(string? searchString, IEnumerable<Guid>? filterIds, Guid? createdBy, string languageCode, int offset = 0)
    {
        return await base.FilterAsync(searchString, filterIds, createdBy, languageCode, offset);
    }

}
