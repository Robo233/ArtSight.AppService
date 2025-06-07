using ArtSight.Core.Models;

namespace ArtSight.AppService.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);

}
