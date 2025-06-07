namespace ArtSight.AppService.Interfaces.Services;

public interface IFavoriteService
{
    Task<bool> IsEntityFavoritedAsync(Guid entityId, string entityType);

}
