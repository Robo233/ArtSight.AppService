using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models.DTOs.Artwork;
using ArtSight.Core.Models;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.DTOs.Exhibition;

namespace ArtSight.AppService.Processors;

public class ArtworkProcessor : PageEntityProcessor<Artwork, ArtworkDto>, IArtworkProcessor
{
    private readonly IArtworkRepository _artworkRepository;
    private readonly IArtistRepository _artistRepository;
    private readonly IExhibitionRepository _exhibitionRepository;
    private readonly IGenreRepository _genreRepository;

    public ArtworkProcessor(IArtworkRepository artworkRepository, IArtistRepository artistRepository, IExhibitionRepository exhibitionRepository, IGenreRepository genreRepository, ILocalizationService localizationService, IFavoriteService favoriteService, IRecentlyViewedService recentlyViewedService, IHttpContextAccessor httpContextAccessor) : base(artworkRepository, localizationService, favoriteService, recentlyViewedService, httpContextAccessor)
    {
        _artworkRepository = artworkRepository;
        _artistRepository = artistRepository;
        _exhibitionRepository = exhibitionRepository;
        _genreRepository = genreRepository;
    }

    protected override async Task PopulateCustomFieldsAsync(Artwork entity, ArtworkDto dto, string languageCode)
    {
        var artist = entity.ArtistId.HasValue
            ? await _artistRepository.GetByIdAsync(entity.ArtistId.Value)
            : null;

        var exhibition = entity.ExhibitionId.HasValue
            ? await _exhibitionRepository.GetByIdAsync(entity.ExhibitionId.Value)
            : null;

        var genres = await _genreRepository.GetGenresDictionaryByIdsAsync(entity.GenreIds, languageCode);

        dto.ArtistId = entity.ArtistId;
        dto.ArtistName = _localizationService.GetLocalizedValueOrDefault(artist?.Name, languageCode, "Unknown");

        dto.ExhibitionId = entity.ExhibitionId;
        dto.ExhibitionName = exhibition != null
            ? _localizationService.GetLocalizedValueOrDefault(exhibition.Name, languageCode, "Untitled")
            : null;
        dto.Genres = genres;
        dto.Medium = _localizationService.GetLocalizedValueOrDefault(entity.Medium, languageCode);
        dto.Dimensions = entity.Dimensions;
        dto.Latitude = entity.Location?.Coordinates?.Latitude != null ? (decimal?)entity.Location.Coordinates.Latitude : null;
        dto.Longitude = entity.Location?.Coordinates?.Longitude != null ? (decimal?)entity.Location.Coordinates.Longitude : null;
    }

    public Task<ActionResult<ArtworkDto>> GetArtworkAsync(LocalizedRequest request)
    {
        return GetAsync(request, "artwork");
    }

    public async Task<ActionResult<AllPageEntitesWithCoordinatesCardDto>> GetAllArtworksAsync(ByLanguageCodeRequest request)
    {
        var artworks = await _artworkRepository.GetAllArtworksAsync();
        var artworkWithCoordinatesDtos = artworks!
            .Where(e => e.Location?.Coordinates != null)
            .Select(artwork => new PageEntityWithCoordinatesCardDto
            {
                Id = artwork.Id,
                Name = _localizationService.GetLocalizedValueOrDefault(artwork.Name, request.LanguageCode, "Untitled")!,
                Latitude = (decimal)artwork.Location!.Coordinates.Latitude,
                Longitude = (decimal)artwork.Location.Coordinates.Longitude,
            })
            .ToList();

        var allArtworksDto = new AllPageEntitesWithCoordinatesCardDto
        {
            Entities = artworkWithCoordinatesDtos
        };
        return new OkObjectResult(allArtworksDto);
    }

    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkListAsync(LocalizedListRequest request)
    {
        return await GetPaginatedListAsync(request, "artwork");
    }

    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkListByArtistIdAsync(LocalizedListRequest request)
    {
        return await GetPageEntityCardDtos(Guid.Parse(request.ParentId!), "artist", request.Offset, request.Limit, request.LanguageCode);
    }

    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkListByExhibitionIdAsync(LocalizedListRequest request)
    {
        return await GetPageEntityCardDtos(Guid.Parse(request.ParentId!), "exhibition", request.Offset, request.Limit, request.LanguageCode);
    }

    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkListByGenreIdAsync(LocalizedListRequest request)
    {
        return await GetPageEntityCardDtos(Guid.Parse(request.ParentId!), "genre", request.Offset, request.Limit, request.LanguageCode);
    }

    private async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetPageEntityCardDtos(Guid? parentId, string parentEntityType, int offset, int limit, string languageCode)
    {
        var paginatedResult = await _artworkRepository.GetPagedListAsync(parentId, parentEntityType, offset, limit);
        var cardDtos = paginatedResult.Items!.Select(e => new PageEntityCardDto
        {
            Id = e.Id,
            Name = _localizationService.GetLocalizedValueOrDefault(e.Name, languageCode, "Untitled")!
        }).ToList();

        var dtoPaginatedResult = new PaginatedResult<PageEntityCardDto>
        {
            Items = cardDtos,
            HasMore = paginatedResult.HasMore
        };

        return new OkObjectResult(dtoPaginatedResult);
    }

}
