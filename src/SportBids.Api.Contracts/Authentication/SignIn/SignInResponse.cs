#nullable disable

using SportBids;

namespace SportBids.Contracts.Authentication.SignIn;

public record SignInResponse
{
    public string UserName { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
