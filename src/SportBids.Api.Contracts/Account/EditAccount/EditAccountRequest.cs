#nullable disable

using System.ComponentModel.DataAnnotations;

namespace SportBids.Api.Contracts.Account.EditAccount;

public class EditAccountRequest
{
    [MaxLength(50)]
    public string FirstName { get; init; }

    [MaxLength(50)]
    public string LastName { get; init; }
}

