using ArtSight.AppService.Setup;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.Core.Models;
using ArtSight.Core.MongoDb.Interfaces;
using MongoDB.Driver;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Interfaces.Services;

namespace ArtSight.AppService.Repositories;

public class ExhibitionRepository : PageEntityMongoOperations<Exhibition>, IExhibitionRepository
{
    public ExhibitionRepository(IMongoConnection<AppDbConnectionSettings> mongoConnection, ILocalizationService localizationService) : base(mongoConnection, localizationService)
    {

    }

    public async Task<Exhibition?> GetByIdAsync(Guid exhibitionId)
    {
        return await base.GetByIdAsync(exhibitionId);
    }

    public async Task<List<Exhibition>?> GetAllExhibitionsAsync()
    {
        var filter = Builders<Exhibition>.Filter.Empty;
        var exhibitions = await FindEntitiesAsync(filter);
        return exhibitions;
    }

    public async Task<List<Exhibition>> GetListByIdsAsync(List<Guid> ids)
    {
        return await GetByIdsAsync(ids);
    }

    public async Task<PaginatedResult<Exhibition>> GetPagedListAsync(Guid? parentId, int offset, int limit)
    {
        return await base.GetPagedListAsync(
            parentId: parentId,
            parentSelector: null,
            offset: offset,
            limit: limit,
            sortSelector: x => x.Tracking.CreatedUtc);
    }

    public async new Task<PaginatedResult<PageEntityCardDto>> FilterAsync(string? searchString, IEnumerable<Guid>? filterIds, Guid? createdBy, string languageCode, int offset = 0)
    {
        return await base.FilterAsync(searchString, filterIds, createdBy, languageCode, offset);
    }

}
