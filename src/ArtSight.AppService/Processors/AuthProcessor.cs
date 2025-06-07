using ArtSight.AppService.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using ArtSight.AppService.Interfaces.Processors;
using ArtSight.AppService.Interfaces.Repositories;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.Core.Models;
using ArtSight.AppService.Models.DTOs.Auth;

namespace ArtSight.AppService.Processors;

public class AuthProcessor : IAuthProcessor
{
    private readonly string? _adminEmail;

    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthProcessor(IUserRepository userRepository, ITokenService tokenService, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _adminEmail = configuration["AdminEmail"];
    }

    public async Task<ActionResult<GoogleLoginDto>> GoogleLoginAsync(GoogleLoginRequest request)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
            if (payload == null)
            {
                return new UnauthorizedObjectResult("Invalid Google token");
            }

            var user = await _userRepository.FindUserByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = payload.Email,
                    IsAdmin = payload.Email.ToLower() == _adminEmail!.ToLower()
                };
                await _userRepository.AddUserAsync(user);
            }


            var token = _tokenService.GenerateToken(user);
            var response = new GoogleLoginDto { Token = token, Email = user.Email };

            return new OkObjectResult(response);
        }
        catch (InvalidJwtException ex)
        {
            return new UnauthorizedObjectResult("Invalid Google token: " + ex.Message);
        }
        catch (Exception ex)
        {
            return new ObjectResult("An error occurred: " + ex.Message) { StatusCode = 500 };
        }
    }

}
