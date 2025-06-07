using System.Security.Claims;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.Requests;
using ArtSight.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Processors;

public abstract class PageEntityProcessor<TEntity, TDto> where TEntity : PageEntity where TDto : PageEntityDto, new()
{
    protected readonly ILocalizationService _localizationService;
    protected readonly IFavoriteService _favoriteService;
    protected readonly IRecentlyViewedService _recentlyViewedService;
    protected readonly IPageEntityRepository<TEntity> _repository;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected PageEntityProcessor(IPageEntityRepository<TEntity> repository, ILocalizationService localizationService, IFavoriteService favoriteService, IRecentlyViewedService recentlyViewedService, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _localizationService = localizationService;
        _favoriteService = favoriteService;
        _httpContextAccessor = httpContextAccessor;
        _recentlyViewedService = recentlyViewedService;
    }

    public async Task<ActionResult<TDto>> GetAsync(LocalizedRequest request, string entityTypeName)
    {
        var entityId = Guid.Parse(request.Id!);
        var entity = await _repository.GetByIdAsync(entityId);
        if (entity == null)
            return new NoContentResult();

        var isFavorite = await _favoriteService.IsEntityFavoritedAsync(entityId, entityTypeName);

        _recentlyViewedService.AddRecentlyViewedEntity(entityId, entityTypeName);

        var dto = new TDto();
        PopulateCommonFields(entity, dto, request.LanguageCode, isFavorite);

        await PopulateCustomFieldsAsync(entity, dto, request.LanguageCode);

        return new OkObjectResult(dto);
    }

    protected Guid? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userIdString = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("sub")?.Value;
        if (Guid.TryParse(userIdString, out var userId))
            return userId;
        return null;
    }

    protected virtual void PopulateCommonFields(TEntity entity, TDto dto, string languageCode, bool isFavorite)
    {
        dto.Id = entity.Id;
        dto.Name = _localizationService.GetLocalizedValueOrDefault(entity.Name, languageCode, "Untitled");
        var description = _localizationService.GetLocalizedValueOrDefault(entity.Description, languageCode);
        var finalLanguageCode = entity.Description?.FirstOrDefault(kvp => kvp.Value == description).Key;
        dto.Description = description;
        dto.DescriptionLanguageCode = finalLanguageCode;
        dto.MainImageAspectRatio = entity.MainImageAspectRatio;
        dto.IsFavorite = isFavorite;
        var currentUserId = GetCurrentUserId();
        var isAdmin = _httpContextAccessor.HttpContext?.User?.IsInRole("Admin") ?? false;
        dto.IsOwner = isAdmin || (currentUserId.HasValue && entity.OwnerId.HasValue && entity.OwnerId == currentUserId);
    }

    protected abstract Task PopulateCustomFieldsAsync(TEntity entity, TDto dto, string languageCode);

    public virtual async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> GetPaginatedListAsync(LocalizedListRequest request, string entityTypeName)
    {
        Guid? parentGuid = null;
        if (!string.IsNullOrEmpty(request.ParentId))
            parentGuid = Guid.Parse(request.ParentId);

        var paginatedResult = await _repository.GetPagedListAsync(
            parentGuid,
            request.Offset,
            request.Limit
        );

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
