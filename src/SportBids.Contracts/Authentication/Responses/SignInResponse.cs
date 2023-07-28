#nullable disable

namespace SportBids.Contracts.Authentication.Responses;

public record SignInResponse
{
    public string Username { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}
