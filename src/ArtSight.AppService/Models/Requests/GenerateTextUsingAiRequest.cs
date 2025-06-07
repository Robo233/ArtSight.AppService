namespace ArtSight.AppService.Models.Requests;

public class GenerateTextUsingAiRequest
{
    public string? Id { get; set; }
    public string? Prompt { get; set; }
    public string? EntityType { get; set; }
    public string LanguageCode { get; set; } = "en";

}
