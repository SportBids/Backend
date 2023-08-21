#nullable disable

namespace SportBids.Contracts.Account.SignUp;

public class SignUpResponse
{
    public string UserName { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
};
