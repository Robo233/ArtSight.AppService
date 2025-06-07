using ArtSight.Core.Models;

namespace ArtSight.AppService.Interfaces.Repositories;

public interface IGenreRepository : IPageEntityRepository<Genre>
{
    Task<Dictionary<Guid, string?>?> GetGenresDictionaryByIdsAsync(List<Guid> genreIds, string LanguageCode);

}
