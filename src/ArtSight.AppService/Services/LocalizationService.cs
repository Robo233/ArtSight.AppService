using ArtSight.AppService.Interfaces.Services;

namespace ArtSight.AppService.Services;

public class LocalizationService : ILocalizationService
{
    private readonly Dictionary<string, Dictionary<string, string>> _localizations;

    public LocalizationService()
    {
        _localizations = new Dictionary<string, Dictionary<string, string>>
        {
            { "en", new Dictionary<string, string>
                {
                    { "Untitled", "Untitled" },
                    { "Unknown", "Unknown"},
                    { "QuestionPrompt", "Analyze this artwork in details. Use a funny, friendly and informal tone." },
                    { "FollowUpPromptPrefix", "Answer this question only: "}
                }
            },
            { "ro", new Dictionary<string, string>
                {
                    { "Untitled", "Fără titlu" },
                    { "Unknown", "Necunoscut"},
                    { "QuestionPrompt", "Analizează această lucrare de artă în detalii. Vorbește într-un ton amuzant, prietenos și informal." },
                    { "FollowUpPromptPrefix", "Răspunde doar la întrebarea acesta: "}
                }
            }
        };
    }

    public string GetLocalizedString(string languageCode, string key)
    {
        if (_localizations.TryGetValue(languageCode, out var strings) && strings.TryGetValue(key, out var localizedValue))
        {
            return localizedValue;
        }

        return _localizations["en"][key];
    }

    public string? GetLocalizedValueOrDefault(IReadOnlyDictionary<string, string?>? dictionary, string requestedLanguageCode, string? defaultKey = "")
    {
        if (dictionary == null || !dictionary.Any())
        {
            return !string.IsNullOrEmpty(defaultKey) ? GetLocalizedString(requestedLanguageCode, defaultKey) : null;
        }

        if (dictionary.TryGetValue(requestedLanguageCode, out var value) && !string.IsNullOrEmpty(value))
        {
            return value;
        }

        if (dictionary.TryGetValue("en", out value) && !string.IsNullOrEmpty(value))
        {
            return value;
        }

        return dictionary.Values.FirstOrDefault();
    }

    public IEnumerable<string> GetLanguageCodes()
    {
        return _localizations.Keys;
    }

}
