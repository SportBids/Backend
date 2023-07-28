#nullable disable

using System.ComponentModel.DataAnnotations;

namespace SportBids.Contracts.Authentication.Requests;

public record SignInRequest()
{
    [Required(AllowEmptyStrings = false)]
    public string UserName { get; init; }
    
    [Required(AllowEmptyStrings = false)]
    public string Password { get; init; }
}
