using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Interfaces.Processors;

namespace ArtSight.AppService.Controllers;

[ApiController]
[Route("artsight/ai")]
public class AIController : Controller
{
    private readonly IAIProcessor _aiProcessor;

    public AIController(IAIProcessor aiProcessor)
    {
        _aiProcessor = aiProcessor;
    }

    [HttpPost]
    public async Task<ActionResult> StartConversation([FromBody] LocalizedRequest request)
    {
        return await _aiProcessor.StartConversationAsync(request);
    }

    [HttpPost("followup")]
    public async Task<ActionResult> ContinueConversation([FromBody] AIPromptRequest request)
    {
        return await _aiProcessor.ContinueConversationAsync(request);
    }

}
