using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtSight.AppService.Processors;

public class EntityFilterProcessor : IEntityFilterProcessor
{
    private readonly IArtworkRepository _artworkRepository;
    private readonly IArtworkDetailRepository _artworkDetailRepository;
    private readonly IArtistRepository _artistRepository;
    private readonly IExhibitionRepository _exhibitionRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EntityFilterProcessor(IArtworkRepository artworkRepository, IArtworkDetailRepository artworkDetailRepository, IArtistRepository artistRepository, IExhibitionRepository exhibitionRepository, IGenreRepository genreRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _artworkRepository = artworkRepository;
        _artworkDetailRepository = artworkDetailRepository;
        _artistRepository = artistRepository;
        _exhibitionRepository = exhibitionRepository;
        _genreRepository = genreRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ActionResult<PaginatedResult<PageEntityCardDto>>> FilterEntitiesAsync(EntityFilterRequest request)
    {
        Guid? userId = null;
        IEnumerable<Guid>? filterIds = null;

        if (request.IsFavorited == true || request.IsCreatedByUser == true || request.IsRecentlyViewed == true)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userIdString = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? user?.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var parsedUserId))
            {
                return new UnauthorizedResult();
            }
            userId = parsedUserId;

            if (request.IsRecentlyViewed == true)
            {
                filterIds = await _userRepository
                    .GetRecentlyViewedEntityIdsAsync(userId.Value, request.EntityType!, 0, int.MaxValue)
                    .ContinueWith(t => t.Result.Items);
            }
            else if (request.IsFavorited == true)
            {
                filterIds = await _userRepository
                    .GetFavoriteEntityIdsAsync(userId.Value, request.EntityType!, 0, int.MaxValue)
                    .ContinueWith(t => t.Result.Items);
            }
        }

        if (string.IsNullOrWhiteSpace(request.EntityType))
        {
            return new BadRequestObjectResult("EntityType is required for this endpoint.");
        }

        PaginatedResult<PageEntityCardDto> result = request.EntityType.ToLower() switch
        {
            "artwork" => await _artworkRepository.FilterAsync(
                request.SearchString,
                filterIds,
                request.IsCreatedByUser == true ? userId : null,
                request.LanguageCode,
                request.Offset
                ),
            "artworkdetail" => await _artworkDetailRepository.FilterAsync(
                request.SearchString,
                filterIds,
                request.IsCreatedByUser == true ? userId : null,
                request.LanguageCode,
                request.Offset
                ),
            "artist" => await _artistRepository.FilterAsync(
                request.SearchString,
                filterIds,
                request.IsCreatedByUser == true ? userId : null,
                request.LanguageCode,
                request.Offset
                ),
            "exhibition" => await _exhibitionRepository.FilterAsync(
                request.SearchString,
                filterIds,
                request.IsCreatedByUser == true ? userId : null,
                request.LanguageCode,
                request.Offset
                ),
            "genre" => await _genreRepository.FilterAsync(
                request.SearchString,
                filterIds,
                request.IsCreatedByUser == true ? userId : null,
                request.LanguageCode,
                request.Offset),
            _ => throw new ArgumentException($"Invalid EntityType: {request.EntityType}")
        };

        return new OkObjectResult(result);
    }

    public async Task<ActionResult<List<SearchResponseDto>>> GlobalFilterAsync(BaseEntityFilterRequest request)
    {
        Guid? userId = null;
        if (request.IsFavorited == true || request.IsCreatedByUser == true || request.IsRecentlyViewed == true)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userIdString = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? user?.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var parsedUserId))
            {
                return new UnauthorizedResult();
            }
            userId = parsedUserId;
        }

        int globalOffset = 0;

        var artworkFilterIds = request.IsRecentlyViewed == true
            ? await _userRepository.GetRecentlyViewedEntityIdsAsync(userId!.Value, "artwork", 0, int.MaxValue)
                  .ContinueWith(t => t.Result.Items)
            : request.IsFavorited == true
                ? await _userRepository.GetFavoriteEntityIdsAsync(userId!.Value, "artwork", 0, int.MaxValue)
                      .ContinueWith(t => t.Result.Items)
                : null;

        var artworkDetailFilterIds = request.IsRecentlyViewed == true
            ? await _userRepository.GetRecentlyViewedEntityIdsAsync(userId!.Value, "artworkdetail", 0, int.MaxValue)
                  .ContinueWith(t => t.Result.Items)
            : request.IsFavorited == true
                ? await _userRepository.GetFavoriteEntityIdsAsync(userId!.Value, "artworkdetail", 0, int.MaxValue)
                      .ContinueWith(t => t.Result.Items)
                : null;

        var artistFilterIds = request.IsRecentlyViewed == true
            ? await _userRepository.GetRecentlyViewedEntityIdsAsync(userId!.Value, "artist", 0, int.MaxValue)
                  .ContinueWith(t => t.Result.Items)
            : request.IsFavorited == true
                ? await _userRepository.GetFavoriteEntityIdsAsync(userId!.Value, "artist", 0, int.MaxValue)
                      .ContinueWith(t => t.Result.Items)
                : null;

        var exhibitionFilterIds = request.IsRecentlyViewed == true
            ? await _userRepository.GetRecentlyViewedEntityIdsAsync(userId!.Value, "exhibition", 0, int.MaxValue)
                  .ContinueWith(t => t.Result.Items)
            : request.IsFavorited == true
                ? await _userRepository.GetFavoriteEntityIdsAsync(userId!.Value, "exhibition", 0, int.MaxValue)
                      .ContinueWith(t => t.Result.Items)
                : null;

        var genreFilterIds = request.IsRecentlyViewed == true
            ? await _userRepository.GetRecentlyViewedEntityIdsAsync(userId!.Value, "genre", 0, int.MaxValue)
                  .ContinueWith(t => t.Result.Items)
            : request.IsFavorited == true
                ? await _userRepository.GetFavoriteEntityIdsAsync(userId!.Value, "genre", 0, int.MaxValue)
                      .ContinueWith(t => t.Result.Items)
                : null;

        var artworkResult = await _artworkRepository.FilterAsync(
            request.SearchString,
            artworkFilterIds,
            request.IsCreatedByUser == true ? userId : null,
            request.LanguageCode,
            globalOffset
            );

        var artworkDetailResult = await _artworkDetailRepository.FilterAsync(
            request.SearchString,
            artworkDetailFilterIds,
            request.IsCreatedByUser == true ? userId : null,
            request.LanguageCode,
            globalOffset
            );

        var artistResult = await _artistRepository.FilterAsync(
            request.SearchString,
            artistFilterIds,
            request.IsCreatedByUser == true ? userId : null,
            request.LanguageCode,
            globalOffset
           );

        var exhibitionResult = await _exhibitionRepository.FilterAsync(
            request.SearchString,
            exhibitionFilterIds,
            request.IsCreatedByUser == true ? userId : null,
            request.LanguageCode,
            globalOffset
            );

        var genreResult = await _genreRepository.FilterAsync(
            request.SearchString,
            genreFilterIds,
            request.IsCreatedByUser == true ? userId : null,
            request.LanguageCode,
            globalOffset);

        var result = new List<SearchResponseDto>
            {
                new() { EntityType = "artwork", Result = artworkResult },
                new() { EntityType = "artworkDetail", Result = artworkDetailResult },
                new() { EntityType = "artist", Result = artistResult },
                new() { EntityType = "exhibition", Result = exhibitionResult },
                new() { EntityType = "genre", Result = genreResult }
            };

        return new OkObjectResult(result);
    }
}
