namespace ArtSight.AppService.Models.Extensions;

public static class ImageDescriptionsExtensions
{
    public static List<string?> GetDescriptionsInLanguage(this List<Dictionary<string, string?>> imageDescriptions, string languageCode)
    {
        if (imageDescriptions == null || string.IsNullOrEmpty(languageCode))
        {
            return [];
        }

        return [.. imageDescriptions
            .Where(dict => dict.ContainsKey(languageCode))
            .Select(dict => dict[languageCode])];
    }
}
