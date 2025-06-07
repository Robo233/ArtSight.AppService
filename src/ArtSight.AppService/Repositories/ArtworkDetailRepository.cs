using ArtSight.AppService.Setup;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.Core.Models;
using ArtSight.Core.MongoDb.Interfaces;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Interfaces.Services;

namespace ArtSight.AppService.Repositories;

public class ArtworkDetailRepository : PageEntityMongoOperations<ArtworkDetail>, IArtworkDetailRepository
{
    public ArtworkDetailRepository(IMongoConnection<AppDbConnectionSettings> mongoConnection, ILocalizationService localizationService) : base(mongoConnection, localizationService)
    {

    }

    public async Task<ArtworkDetail?> GetByIdAsync(Guid artistId)
    {
        return await base.GetByIdAsync(artistId);
    }

    public async Task<List<ArtworkDetail>> GetListByIdsAsync(List<Guid> ids)
    {
        return await GetByIdsAsync(ids);
    }

    public async Task<PaginatedResult<ArtworkDetail>> GetPagedListAsync(Guid? parentId, int offset, int limit)
    {
        return await base.GetPagedListAsync(
            parentId: parentId,
            parentSelector: x => x.ArtworkId,
            offset: offset,
            limit: limit,
            sortSelector: x => x.Tracking.CreatedUtc);
    }

    public async new Task<PaginatedResult<PageEntityCardDto>> FilterAsync(string? searchString, IEnumerable<Guid>? filterIds, Guid? createdBy, string languageCode, int offset = 0)
    {
        return await base.FilterAsync(searchString, filterIds, createdBy, languageCode, offset);
    }

}
