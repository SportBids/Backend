namespace SportBids.Domain;

public class Group : Entity<Guid>
{
    public string Name { get; set; }
    public ICollection<Team> Teams { get; set; }
    public ICollection<Match> Matches { get; set; }
}
