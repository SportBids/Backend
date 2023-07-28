#nullable disable

namespace SportBids.Application.Authentication.Common;

public class AuthResult
{
    public AuthResult(string accessToken = null, string refreshToken = null)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
