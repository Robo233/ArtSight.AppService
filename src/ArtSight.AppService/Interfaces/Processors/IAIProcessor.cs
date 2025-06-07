using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ArtSight.AppService.Interfaces.Processors;

public interface IAIProcessor
{
    Task<ActionResult> StartConversationAsync(LocalizedRequest request);
    Task<ActionResult> ContinueConversationAsync(AIPromptRequest request);

}
