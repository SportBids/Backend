using System.Security.Claims;
using SportBids.Domain.Entities;

namespace SportBids.Application.Interfaces.Authentication;

public interface IJwtFactory
{
    string GenerateAccessToken(AppUser user, IEnumerable<Claim> userClaims);
    RefreshToken GenerateRefreshToken(string IPAddress);
}
