using ArtSight.Core.MongoDb.Setup;
using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Processors;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Services;
using ArtSight.AppService.Repositories;
using ArtSight.AppService.Interfaces.Repositories;

namespace ArtSight.AppService.Setup;

public static class ServiceExtensions
{
    public static WebApplicationBuilder ConfigureService(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.InitializeMongoDb<AppDbConnectionSettings>(builder.Configuration, "MongoDbSettings");

        services.AddSingleton<IArtworkProcessor, ArtworkProcessor>();
        services.AddSingleton<IExhibitionProcessor, ExhibitionProcessor>();
        services.AddSingleton<IAuthProcessor, AuthProcessor>();
        services.AddSingleton<IFavoriteProcessor, FavoriteProcessor>();
        services.AddSingleton<IArtworkDetailProcessor, ArtworkDetailProcessor>();
        services.AddSingleton<IAIProcessor, AIProcessor>();
        services.AddSingleton<IArtistProcessor, ArtistProcessor>();
        services.AddSingleton<IGenreProcessor, GenreProcessor>();
        services.AddSingleton<IEntityFilterProcessor, EntityFilterProcessor>();

        services.AddSingleton<ILocalizationService, LocalizationService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IFavoriteService, FavoriteService>();
        services.AddSingleton<IAIService, AIService>();
        services.AddSingleton<IRecentlyViewedService, RecentlyViewedService>();

        services.AddSingleton<IArtworkRepository, ArtworkRepository>();
        services.AddSingleton<IExhibitionRepository, ExhibitionRepository>();
        services.AddSingleton<IArtworkDetailRepository, ArtworkDetailRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IArtistRepository, ArtistRepository>();
        services.AddSingleton<IGenreRepository, GenreRepository>();

        return builder;
    }

}
