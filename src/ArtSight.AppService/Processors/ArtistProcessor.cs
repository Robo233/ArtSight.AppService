using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.DTOs.Artist;
using ArtSight.AppService.Models.Extensions;
using ArtSight.AppService.Models.Requests;
using ArtSight.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Processors;

public class ArtistProcessor : PageEntityProcessor<Artist, ArtistDto>, IArtistProcessor
{
    private readonly IGenreRepository _genreRepository;

    public ArtistProcessor(IArtistRepository artistRepository, IGenreRepository genreRepository, ILocalizationService localizationService, IFavoriteService favoriteService, IRecentlyViewedService recentlyViewedService, IHttpContextAccessor httpContextAccessor) : base(artistRepository, localizationService, favoriteService, recentlyViewedService, httpContextAccessor)
    {
        _genreRepository = genreRepository;
    }

    protected override async Task PopulateCustomFieldsAsync(Artist entity, ArtistDto dto, string languageCode)
    {
        var genres = await _genreRepository.GetGenresDictionaryByIdsAsync(entity.GenreIds, languageCode);
        dto.Genres = genres;
        dto.DateOfBirth = entity.DateOfBirth;
        dto.DateOfDeath = entity.DateOfDeath;
        dto.ImageDescriptions = entity.ImageDescriptions.GetDescriptionsInLanguage(languageCode);
        dto.ContactInfo = new ContactInfo
        {
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email,
            Website = entity.Website,
            SocialMedia = entity.SocialMedia
        };
    }

    public Task<ActionResult<ArtistDto>> GetArtistAsync(LocalizedRequest request)
    {
        return GetAsync(request, "artist");
    }

    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtistListAsync(LocalizedListRequest request)
    {
        return await GetPaginatedListAsync(request, "artist");
    }

}
