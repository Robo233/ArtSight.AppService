using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using ArtSight.AppService.Models.DTOs.Auth;

namespace ArtSight.AppService.Interfaces.Processors;

public interface IAuthProcessor
{
    Task<ActionResult<GoogleLoginDto>> GoogleLoginAsync([FromBody] GoogleLoginRequest request);

}
