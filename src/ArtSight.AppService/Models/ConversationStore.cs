using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace ArtSight.AppService.Models;

public class ConversationItem
{
    [JsonPropertyName("role")]
    public string? Role { get; set; }

    [JsonPropertyName("content")]
    public object[] Content { get; set; } = [];
}

public class TextContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "text";

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public class ImageUrlContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "image_url";

    [JsonPropertyName("image_url")]
    public ImageUrl ImageUrl { get; set; } = new();
}

public class ImageUrl
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

public static class ConversationStore
{
    public static ConcurrentDictionary<string, List<ConversationItem>> Conversations { get; set; } = new ConcurrentDictionary<string, List<ConversationItem>>();

}
