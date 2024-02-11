#nullable disable

using System.ComponentModel.DataAnnotations;

namespace SportBids.Api.Contracts.Account.EditAccount;

public class ConfirmEmailRequest
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string Token { get; set; }
}
