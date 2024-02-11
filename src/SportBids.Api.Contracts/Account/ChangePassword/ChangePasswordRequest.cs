#nullable disable

using System.ComponentModel.DataAnnotations;

namespace SportBids.Api.Contracts.Account.ChangePassword;

public class ChangePasswordRequest
{
    [Required(AllowEmptyStrings = false), MinLength(6)]
    public string CurrentPassword { get; set; }

    [Required(AllowEmptyStrings = false), MinLength(6)]
    public string NewPassword { get; set; }
}

