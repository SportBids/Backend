namespace SportBids.Domain;

public class Team : Entity<Guid>
{
    public string Name { get; set; }
    public string? Logo { get; set; }
    public bool IsKnockedOut { get; set; }
}
