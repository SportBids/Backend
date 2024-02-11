using System.ComponentModel.DataAnnotations;

namespace SportBids.Api.Contracts.Authentication.SignOut;

public class SignOutRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}

