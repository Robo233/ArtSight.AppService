using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.DTOs.Genre;
using ArtSight.AppService.Models.Extensions;
using ArtSight.AppService.Models.Requests;
using ArtSight.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Processors;

public class GenreProcessor : PageEntityProcessor<Genre, GenreDto>, IGenreProcessor
{
    public GenreProcessor(IGenreRepository genreRepository, ILocalizationService localizationService, IFavoriteService favoriteService, IRecentlyViewedService recentlyViewedService, IHttpContextAccessor httpContextAccessor) : base(genreRepository, localizationService, favoriteService, recentlyViewedService, httpContextAccessor)
    {

    }

    protected override Task PopulateCustomFieldsAsync(Genre entity, GenreDto dto, string languageCode)
    {
        dto.ImageDescriptions = entity.ImageDescriptions.GetDescriptionsInLanguage(languageCode);
        return Task.CompletedTask;
    }

    public Task<ActionResult<GenreDto>> GetGenreAsync(LocalizedRequest request)
    {
        return GetAsync(request, "genre");
    }

    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetGenreListAsync(LocalizedListRequest request)
    {
        return await GetPaginatedListAsync(request, "genre");
    }

}
