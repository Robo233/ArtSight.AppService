using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Processors;

public class AIProcessor : IAIProcessor
{
    private readonly ILocalizationService _localizationService;
    private readonly IAIService _aiService;

    private readonly string? _mediaPath;

    public AIProcessor(ILocalizationService localizationService, IAIService aiService, IConfiguration configuration)
    {
        _localizationService = localizationService;
        _aiService = aiService;
        _mediaPath = $"{configuration["Paths:MediaPath"]!}/artwork";

    }

    public async Task<ActionResult> StartConversationAsync(LocalizedRequest request)
    {
        var imagePath = $"{_mediaPath}/{request.Id}/Images/MainImages/main_image.jpg";
        string prompt = _localizationService.GetLocalizedString(request.LanguageCode, "QuestionPrompt");

        (string response, string conversationToken) = await _aiService.StartConversationAsync(imagePath, prompt);
        return new OkObjectResult(new { response, conversationToken });
    }

    public async Task<ActionResult> ContinueConversationAsync(AIPromptRequest request)
    {
        string conversationToken = request.ConversationToken!;
        string response = await _aiService.ContinueConversationAsync(request.Prompt!, conversationToken, request.LanguageCode);
        return new OkObjectResult(new { response });
    }

}
