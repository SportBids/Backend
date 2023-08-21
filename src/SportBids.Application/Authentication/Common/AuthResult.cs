using SportBids.Domain.Entities;

namespace SportBids.Application.Authentication.Common;

public class AuthResult
{
    public string UserName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
