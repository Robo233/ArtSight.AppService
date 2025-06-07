using ArtSight.Core.Models;

namespace ArtSight.AppService.Interfaces.Repositories;

public interface IExhibitionRepository : IPageEntityRepository<Exhibition>
{
    Task<List<Exhibition>?> GetAllExhibitionsAsync();

}
