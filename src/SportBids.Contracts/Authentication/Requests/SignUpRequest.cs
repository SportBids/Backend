#nullable disable

using System.ComponentModel.DataAnnotations;

namespace SportBids.Contracts.Authentication.Requests;

public record SignUpRequest : SignInRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }
    
    [MaxLength(50)]
    public string FirstName { get; init; }
    
    [MaxLength(50)]
    public string LastName { get; init; }
}
