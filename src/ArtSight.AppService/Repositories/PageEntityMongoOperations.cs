using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models.DTOs;
using ArtSight.AppService.Setup;
using ArtSight.Core.Models;
using ArtSight.Core.ModelsBase;
using ArtSight.Core.MongoDb.Interfaces;
using ArtSight.Core.MongoDb.Setup;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ArtSight.AppService.Repositories;

public enum SortDirection
{
    Ascending,
    Descending
}

public class PageEntityMongoOperations<TEntity> : MongoOperations<AppDbConnectionSettings, TEntity, Guid, EntityTracking> where TEntity : PageEntity
{
    private readonly IMongoConnection<AppDbConnectionSettings> _mongoConnection;
    private readonly ILocalizationService _localizationService;
    private readonly int limit = 3;

    public PageEntityMongoOperations(IMongoConnection<AppDbConnectionSettings> mongoConnection, ILocalizationService localizationService) : base(mongoConnection)
    {
        _mongoConnection = mongoConnection;
        _localizationService = localizationService;
    }

    public async Task<PaginatedResult<TEntity>> GetPagedListAsync<TField>(Guid? parentId, Expression<Func<TEntity, Guid?>>? parentSelector, int offset, int limit, Expression<Func<TEntity, TField>>? sortSelector = null, SortDirection sortDirection = SortDirection.Descending)
    {
        var filter = Builders<TEntity>.Filter.Empty;
        if (parentId.HasValue && parentSelector != null)
        {
            filter = Builders<TEntity>.Filter.Eq(parentSelector, parentId.Value);
        }

        var findOptions = new FindOptions<TEntity>
        {
            Skip = offset,
            Limit = limit
        };

        if (sortSelector != null)
        {
            var expressionField = new ExpressionFieldDefinition<TEntity, TField>(sortSelector);
            findOptions.Sort = sortDirection == SortDirection.Ascending
                ? Builders<TEntity>.Sort.Ascending(expressionField)
                : Builders<TEntity>.Sort.Descending(expressionField);
        }

        var results = await FindEntitiesAsync(filter, findOptions) ?? [];

        var collection = _mongoConnection.GetCollection<TEntity>();
        var totalCount = await collection.CountDocumentsAsync(filter);

        bool hasMore = (offset + limit) < totalCount;

        return new PaginatedResult<TEntity>
        {
            Items = results,
            HasMore = hasMore
        };
    }

    public async Task<PaginatedResult<PageEntityCardDto>> FilterAsync(string? searchString, IEnumerable<Guid>? filterIds, Guid? createdBy, string languageCode, int offset = 0)
    {
        var builder = Builders<TEntity>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            var languageCodes = _localizationService.GetLanguageCodes();
            var regexFilters = languageCodes
                .Select(code => builder.Regex($"Name.{code}", new BsonRegularExpression(searchString, "i")))
                .ToList();

            filter &= builder.Or(regexFilters);
        }

        var filterIdsList = filterIds?.ToList() ?? [];

        if (filterIds != null)
        {
            if (filterIdsList.Count == 0)
            {
                return new PaginatedResult<PageEntityCardDto>
                {
                    Items = [],
                    HasMore = false
                };
            }
            filter &= builder.In(e => e.Id, filterIdsList);
        }

        if (createdBy.HasValue)
        {
            filter &= builder.Eq(e => e.OwnerId, createdBy.Value);
        }

        List<TEntity> rawItems;
        long totalCount;
        var collection = _data.GetCollection<TEntity>();

        if (filterIds != null)
        {
            var pipeline = collection.Aggregate()
                .Match(filter)
                .AppendStage<TEntity>(new BsonDocument("$addFields",
                    new BsonDocument("orderIndex",
                        new BsonDocument("$indexOfArray", new BsonArray
                        {
                            new BsonArray(filterIdsList.Select(id => new BsonBinaryData(id, GuidRepresentation.Standard))),
                            "$_id"
                        }))))
                .Sort(Builders<TEntity>.Sort.Descending("orderIndex"))
                .Skip(offset)
                .Limit(limit);
            rawItems = await pipeline.ToListAsync();

            totalCount = await collection.CountDocumentsAsync(filter);
        }
        else
        {
            rawItems = await collection.Find(filter)
                .Skip(offset)
                .Limit(limit)
                .ToListAsync();
            totalCount = await collection.CountDocumentsAsync(filter);
        }

        var items = rawItems.Select(e => new PageEntityCardDto
        {
            Id = e.Id,
            Name = GetMatchedName(e.Name, searchString, languageCode)
        }).ToList();

        return new PaginatedResult<PageEntityCardDto>
        {
            Items = items,
            HasMore = (offset + limit) < totalCount
        };
    }

    private string? GetMatchedName(IReadOnlyDictionary<string, string?> nameDictionary, string? searchString, string languageCode)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return _localizationService.GetLocalizedValueOrDefault(nameDictionary, languageCode, "Untitled");
        }

        var languageCodes = _localizationService.GetLanguageCodes();
        foreach (var code in languageCodes)
        {
            if (nameDictionary.TryGetValue(code, out var nameValue) && !string.IsNullOrWhiteSpace(nameValue))
            {
                if (Regex.IsMatch(nameValue, searchString, RegexOptions.IgnoreCase))
                {
                    return nameValue;
                }
            }
        }
        return nameDictionary.Values.FirstOrDefault();
    }

}
