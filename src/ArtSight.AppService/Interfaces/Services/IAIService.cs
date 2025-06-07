namespace ArtSight.AppService.Interfaces.Services;

public interface IAIService
{
    Task<(string response, string conversationToken)> StartConversationAsync(string imagePath, string prompt);
    Task<string> ContinueConversationAsync(string prompt, string conversationToken, string languageCode);

}
