using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.DTOs.ArtworkDetail;
using ArtSight.AppService.Models.Requests;
using ArtSight.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Processors;

public class ArtworkDetailProcessor : PageEntityProcessor<ArtworkDetail, ArtworkDetailDto>, IArtworkDetailProcessor
{
    IArtworkRepository _artworkRepository;

    public ArtworkDetailProcessor(IArtworkDetailRepository artworkDetailRepository, IArtworkRepository artworkRepository, ILocalizationService localizationService, IFavoriteService favoriteService, IRecentlyViewedService recentlyViewedService, IHttpContextAccessor httpContextAccessor) : base(artworkDetailRepository, localizationService, favoriteService, recentlyViewedService, httpContextAccessor)
    {
        _artworkRepository = artworkRepository;
    }

    protected override async Task PopulateCustomFieldsAsync(ArtworkDetail entity, ArtworkDetailDto dto, string languageCode)
    {
        var artwork = await _artworkRepository.GetByIdAsync((Guid)entity.ArtworkId!);
        dto.ArtworkId = (Guid)entity.ArtworkId!;
        dto.ArtworkName = _localizationService.GetLocalizedValueOrDefault(artwork?.Name, languageCode, "Untitled");

    }

    public Task<ActionResult<ArtworkDetailDto>> GetArtworkDetailAsync(LocalizedRequest request)
    {
        return GetAsync(request, "artworkDetail");
    }

    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkDetailListAsync(LocalizedListRequest request)
    {
        return await GetPaginatedListAsync(request, "artworkDetail");
    }

    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetArtworkDetailListByArtworkIdAsync(LocalizedListRequest request)
    {
        var paginatedResult = await _repository.GetPagedListAsync(Guid.Parse(request.ParentId!), request.Offset, request.Limit);
        var cardDtos = paginatedResult.Items!.Select(e => new PageEntityCardDto
        {
            Id = e.Id,
            Name = _localizationService.GetLocalizedValueOrDefault(e.Name, request.LanguageCode, "Untitled")!
        }).ToList();

        var dtoPaginatedResult = new PaginatedResult<PageEntityCardDto>
        {
            Items = cardDtos,
            HasMore = paginatedResult.HasMore
        };

        return new OkObjectResult(dtoPaginatedResult);
    }

}
