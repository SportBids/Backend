using System.ComponentModel.DataAnnotations;

namespace SportBids.Contracts.Account.Requests;

public class ConfirmEmailRequest
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public string Token { get; set; }
}
