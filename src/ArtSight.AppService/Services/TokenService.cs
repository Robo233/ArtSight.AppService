using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ArtSight.AppService.Interfaces.Services;
using ArtSight.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace ArtSight.AppService.Services;

public class TokenService : ITokenService
{
    private readonly string? _key;

    public TokenService(IConfiguration configuration)
    {
        _key = configuration["Keys:JwtSettingsSecretKey"];
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new ("sub", user.Id.ToString()),
            new ("email", user.Email!)
        };

        if (user.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_key!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "yourdomain.com",
            audience: "yourdomain.com",
            claims: claims,
            expires: DateTime.Now.AddYears(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
