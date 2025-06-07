using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.DTOs.Exhibition;
using ArtSight.AppService.Models.Extensions;
using ArtSight.AppService.Models.Requests;
using ArtSight.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Processors;

public class ExhibitionProcessor : PageEntityProcessor<Exhibition, ExhibitionDto>, IExhibitionProcessor
{
    private readonly IExhibitionRepository _exhibitionRepository;
    private readonly IGenreRepository _genreRepository;

    public ExhibitionProcessor(IExhibitionRepository exhibitionRepository, IGenreRepository genreRepository, ILocalizationService localizationService, IFavoriteService favoriteService, IRecentlyViewedService recentlyViewedService, IHttpContextAccessor httpContextAccessor) : base(exhibitionRepository, localizationService, favoriteService, recentlyViewedService, httpContextAccessor)
    {
        _exhibitionRepository = exhibitionRepository;
        _genreRepository = genreRepository;
    }

    protected override async Task PopulateCustomFieldsAsync(Exhibition entity, ExhibitionDto dto, string languageCode)
    {
        var genres = await _genreRepository.GetGenresDictionaryByIdsAsync(entity.GenreIds, languageCode);
        dto.Genres = genres;
        dto.Latitude = entity.Location?.Coordinates?.Latitude != null ? (decimal?)entity.Location.Coordinates.Latitude : null;
        dto.Longitude = entity.Location?.Coordinates?.Longitude != null ? (decimal?)entity.Location.Coordinates.Longitude : null;
        dto.Address = _localizationService.GetLocalizedValueOrDefault(entity.Address, languageCode);
        dto.ImageDescriptions = entity.ImageDescriptions.GetDescriptionsInLanguage(languageCode);
        dto.Schedule = entity.Schedule;
        dto.ContactInfo = new ContactInfo
        {
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email,
            Website = entity.Website,
            SocialMedia = entity.SocialMedia
        };
    }

    public Task<ActionResult<ExhibitionDto>> GetExhibitionAsync(LocalizedRequest request)
    {
        return GetAsync(request, "exhibition");
    }

    public async Task<ActionResult<AllPageEntitesWithCoordinatesCardDto>> GetAllExhibitionsAsync(ByLanguageCodeRequest request)
    {
        var exhibitions = await _exhibitionRepository.GetAllExhibitionsAsync();
        var exhibitionWithCoordinatesDtos = exhibitions!
            .Where(e => e.Location?.Coordinates != null)
            .Select(exhibition => new PageEntityWithCoordinatesCardDto
            {
                Id = exhibition.Id,
                Name = _localizationService.GetLocalizedValueOrDefault(exhibition.Name, request.LanguageCode, "Untitled")!,
                Latitude = (decimal)exhibition.Location!.Coordinates.Latitude,
                Longitude = (decimal)exhibition.Location.Coordinates.Longitude,
                Address = _localizationService.GetLocalizedValueOrDefault(exhibition.Address, request.LanguageCode),
            })
            .ToList();

        var allExhibitionsDto = new AllPageEntitesWithCoordinatesCardDto
        {
            Entities = exhibitionWithCoordinatesDtos
        };
        return new OkObjectResult(allExhibitionsDto);
    }

    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetExhibitionListAsync(LocalizedListRequest request)
    {
        return await GetPaginatedListAsync(request, "exhibition");
    }

}
