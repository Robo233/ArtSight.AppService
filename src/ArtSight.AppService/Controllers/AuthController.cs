using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Models.Requests;
using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Models.DTOs.Auth;

namespace ArtSight.AppService.Controllers;

[ApiController]
[Route("artsight/auth")]
public class AuthController : Controller
{
    private readonly IAuthProcessor _authProcessor;

    public AuthController(IAuthProcessor authProcessor)
    {
        _authProcessor = authProcessor;
    }

    [HttpPost]
    [Route("google-login")]
    public async Task<ActionResult<GoogleLoginDto>> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        return await _authProcessor.GoogleLoginAsync(request);

    }

}
