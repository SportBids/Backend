using Microsoft.AspNetCore.Identity;

namespace SportBids.Domain.Entities;

public class AppUser : IdentityUser<Guid>, IEquatable<AppUser>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new();

    public ICollection<Prediction>? Predictions { get; set; }
    public ICollection<PrivateLeaderBoard> PrivateLeaderBoards { get; set; }

    public bool Equals(AppUser? other)
    {
        return other is not null && Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
