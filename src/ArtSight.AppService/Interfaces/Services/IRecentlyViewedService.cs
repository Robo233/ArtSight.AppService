namespace ArtSight.AppService.Interfaces.Services;

public interface IRecentlyViewedService
{
    void AddRecentlyViewedEntity(Guid entityId, string entityType);

}
