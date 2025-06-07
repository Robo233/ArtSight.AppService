using ArtSight.AppService.Setup;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.Core.Models;
using ArtSight.Core.MongoDb.Interfaces;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models.DTOs;

namespace ArtSight.AppService.Repositories;

public class GenreRepository : PageEntityMongoOperations<Genre>, IGenreRepository
{
    private readonly ILocalizationService _localizationService;

    public GenreRepository(IMongoConnection<AppDbConnectionSettings> mongoConnection, ILocalizationService localizationService) : base(mongoConnection, localizationService)
    {
        _localizationService = localizationService;
    }

    public async Task<Genre?> GetByIdAsync(Guid artistId)
    {
        return await base.GetByIdAsync(artistId);
    }

    public async Task<Dictionary<Guid, string?>?> GetGenresDictionaryByIdsAsync(List<Guid> genreIds, string LanguageCode)
    {
        var genreList = await GetByIdsAsync(genreIds);

        if (genreList.Count == 0)
        {
            return null;
        }

        var genres = genreList
            .Where(g => g.Name != null)
            .ToDictionary(
                g => g.Id,
                g => _localizationService.GetLocalizedValueOrDefault(g.Name, LanguageCode)
            );

        return genres;
    }

    public async Task<List<Genre>> GetListByIdsAsync(List<Guid> ids)
    {
        return await GetByIdsAsync(ids);
    }

    public async Task<PaginatedResult<Genre>> GetPagedListAsync(Guid? parentId, int offset, int limit)
    {
        return await GetPagedListAsync(
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
