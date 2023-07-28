#nullable disable

namespace SportBids.Infrastructure.Authentication;

public class JwtSettings
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecretKey { get; init; }
    public int AccessTokenExpiryMinutes { get; init; }
    public int RefreshTokenExpiryMinutes { get; init; }
}
