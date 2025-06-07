using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Models.Requests;

public class AIPromptRequest
{
    [FromQuery]
    public string? Prompt { get; set; }

    public string? ConversationToken { get; set; }

    [FromQuery]
    public string LanguageCode { get; set; } = "en";

}
