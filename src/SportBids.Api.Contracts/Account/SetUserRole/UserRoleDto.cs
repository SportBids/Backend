#nullable disable

namespace SportBids.Api.Contracts.Account.SetUserRole;

public class UserRoleDto
{
    public Guid UserId { get; set; }
    public string Role { get; set; }
}
