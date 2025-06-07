namespace ArtSight.AppService.Interfaces.Services;

public interface ILocalizationService
{
    string GetLocalizedString(string languageCode, string key);
    string? GetLocalizedValueOrDefault(IReadOnlyDictionary<string, string?>? dictionary, string requestedLanguageCode, string defaultKey = "");
    IEnumerable<string> GetLanguageCodes();

}
