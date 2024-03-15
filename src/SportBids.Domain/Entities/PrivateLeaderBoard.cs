#nullable disable

namespace SportBids.Domain.Entities;

public class PrivateLeaderBoard : Entity<Guid>
{
    public string Name { get; set; }
    public ICollection<AppUser> Members { get; set; }
    public AppUser Owner { get; set; }
    public string JoinCode { get; set; }
}
