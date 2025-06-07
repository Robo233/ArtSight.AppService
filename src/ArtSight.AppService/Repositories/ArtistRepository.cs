using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Setup;
using ArtSight.Core.Models;
using ArtSight.Core.MongoDb.Interfaces;

namespace ArtSight.AppService.Repositories;

public class ArtistRepository : PageEntityMongoOperations<Artist>, IArtistRepository
{
    public ArtistRepository(IMongoConnection<AppDbConnectionSettings> mongoConnection, ILocalizationService localizationService) : base(mongoConnection, localizationService)
    {

    }

    public async Task<Artist?> GetByIdAsync(Guid artistId)
    {
        return await base.GetByIdAsync(artistId);
    }

    public async Task<List<Artist>> GetListByIdsAsync(List<Guid> ids)
    {
        return await GetByIdsAsync(ids);
    }

    public async Task<PaginatedResult<Artist>> GetPagedListAsync(Guid? parentId, int offset, int limit)
    {
        var result = await base.GetPagedListAsync(
            parentId: parentId,
            parentSelector: null,
            offset: offset,
            limit: limit,
            sortSelector: x => x.Tracking.CreatedUtc);

        return result;
    }

    public async new Task<PaginatedResult<PageEntityCardDto>> FilterAsync(string? searchString, IEnumerable<Guid>? filterIds, Guid? createdBy, string languageCode, int offset = 0)
    {
        return await base.FilterAsync(searchString, filterIds, createdBy, languageCode, offset);
    }
}
