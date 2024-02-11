namespace SportBids.Domain.Entities;

public class Team : Entity<Guid>
{
    public string Name { get; set; } = null!;
    public string? Logo { get; set; }
    public bool IsKnockedOut { get; set; }
    // public Tournament Tournament { get; set; } = null!;
    // public Group Group { get; set; } = null!;
}
