using System.Net.Http.Headers;
using System.Text.Json;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models;

namespace ArtSight.AppService.Services;

public class AIService : IAIService
{
    private readonly string? _apiKey;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalizationService _localizationService;

    public AIService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILocalizationService localizationService)
    {
        _apiKey = configuration["Keys:OpenAIAPIKey"];
        _httpClientFactory = httpClientFactory;
        _localizationService = localizationService;
    }

    public async Task<(string response, string conversationToken)> StartConversationAsync(string imagePath, string prompt)
    {
        byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
        string base64Image = Convert.ToBase64String(imageBytes);
        string dataUrl = $"data:image/jpeg;base64,{base64Image}";

        var conversation = new List<ConversationItem>
        {
            new() {
                Role = "user",
                Content =
                [
                    new TextContent { Text = prompt },
                    new ImageUrlContent { ImageUrl = new ImageUrl { Url = dataUrl } }
                ]
            }
        };

        string conversationToken = Guid.NewGuid().ToString();
        ConversationStore.Conversations[conversationToken] = conversation;

        string apiResponse = await SendOpenAiRequest(conversation.Cast<object>().ToList());
        string extractedResponse = ExtractMessageFromResponse(apiResponse);

        return (extractedResponse, conversationToken);
    }

    public async Task<string> ContinueConversationAsync(string prompt, string conversationToken, string languageCode)
    {
        if (!ConversationStore.Conversations.TryGetValue(conversationToken, out var conversation))
        {
            return "No conversation context found. Please start the conversation first.";
        }

        string prefix = _localizationService.GetLocalizedString(languageCode, "FollowUpPromptPrefix");

        foreach (var item in conversation.Where(x => x.Role == "user"))
        {
            for (int i = 0; i < item.Content.Length; i++)
            {
                if (item.Content[i] is TextContent textContent)
                {
                    if (textContent.Text!.StartsWith(prefix))
                    {
                        textContent.Text = textContent.Text.Substring(prefix.Length);
                    }
                }
            }
        }

        var newMessage = new ConversationItem
        {
            Role = "user",
            Content =
            [
                new TextContent { Text = prefix + prompt }
            ]
        };

        conversation.Add(newMessage);

        string apiResponse = await SendOpenAiRequest(conversation.Cast<object>().ToList());
        return ExtractMessageFromResponse(apiResponse);
    }

    static string ExtractMessageFromResponse(string jsonResponse)
    {
        using var doc = JsonDocument.Parse(jsonResponse);
        JsonElement root = doc.RootElement;

        if (root.TryGetProperty("response", out JsonElement innerJsonElement))
        {
            string innerJson = innerJsonElement.GetString()!;
            using var innerDoc = JsonDocument.Parse(innerJson);
            root = innerDoc.RootElement;
        }

        if (root.TryGetProperty("choices", out JsonElement choices) && choices.ValueKind == JsonValueKind.Array && choices.GetArrayLength() > 0)
        {
            var firstChoice = choices[0];

            if (firstChoice.TryGetProperty("message", out JsonElement messageElement) && messageElement.TryGetProperty("content", out JsonElement contentElement))
            {
                return contentElement.GetString()!;
            }
        }

        return string.Empty;
    }

    private async Task<string> SendOpenAiRequest(List<object> messages)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var requestBody = new
        {
            model = "gpt-4o",
            messages,
            max_tokens = 500
        };

        var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);
        var jsonResponse = await response.Content.ReadAsStringAsync();

        return jsonResponse;
    }
}
