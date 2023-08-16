#nullable disable

using System.ComponentModel.DataAnnotations;

namespace SportBids.Contracts.Account.SignUp;

public class SignUpRequest
{
    [Required(AllowEmptyStrings = false)]
    public string UserName { get; init; }

    [Required(AllowEmptyStrings = false)]
    public string Password { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [MaxLength(50)]
    public string FirstName { get; init; }

    [MaxLength(50)]
    public string LastName { get; init; }
}
