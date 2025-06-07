using ArtSight.AppService.Models.DTOs;
using ArtSight.Core.Models;

namespace ArtSight.AppService.Interfaces.Repositories;

public interface IPageEntityRepository<TEntity> where TEntity : PageEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<PaginatedResult<TEntity>> GetPagedListAsync(Guid? parentId, int offset, int limit);
    Task<List<TEntity>> GetListByIdsAsync(List<Guid> ids);
    Task<PaginatedResult<PageEntityCardDto>> FilterAsync(string? searchString, IEnumerable<Guid>? favoriteIds, Guid? createdBy, string languageCode, int offset = 0);

}
