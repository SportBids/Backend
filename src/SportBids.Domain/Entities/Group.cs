#nullable disable

namespace SportBids.Domain.Entities;

public class Group : Entity<Guid>
{
    public string Name { get; set; }
    public Tournament Tournament { get; set; } = null!;
    public ICollection<Team> Teams { get; set; }
    public ICollection<Match> Matches { get; set; }
}
