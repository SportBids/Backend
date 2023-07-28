namespace SportBids.Application.Interfaces.Authentication;

public interface IJwtFactory
{
    string GenerateAccessToken(Guid userId);
    string GenerateRefreshToken();
}
