#nullable disable

using SportBids.Domain.Models;

namespace SportBids.Application.Authentication.Common;

public class AuthResult : User
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
