using SportBids.Domain.Entities;

namespace SportBids.Application.Interfaces.Authentication;

public interface IJwtFactory
{
    string GenerateAccessToken(Guid userId);
    RefreshToken GenerateRefreshToken(string IPAddress);
}
