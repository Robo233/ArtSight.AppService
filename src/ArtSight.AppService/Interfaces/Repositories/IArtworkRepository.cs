using ArtSight.AppService.Models.DTOs;
using ArtSight.Core.Models;

namespace ArtSight.AppService.Interfaces.Repositories;

public interface IArtworkRepository : IPageEntityRepository<Artwork>
{
    Task<List<Artwork>?> GetAllArtworksAsync();
    Task<PaginatedResult<Artwork>> GetPagedListAsync(Guid? parentId, string parentEntityType, int offset, int limit);

}
