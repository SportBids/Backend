using Microsoft.AspNetCore.Identity;

namespace SportBids.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new();

    public ICollection<Prediction> Predictions { get; set; }
}
