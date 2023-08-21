#nullable disable

namespace SportBids.Contracts.Authentication.RefreshToken;

public class RefreshTokenResponse
{
    public string UserName { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
