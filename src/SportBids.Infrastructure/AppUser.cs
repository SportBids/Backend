using Microsoft.AspNetCore.Identity;

namespace SportBids.Infrastructure;

public sealed class AppUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
