#nullable disable

using SportBids.Domain.Entities;
namespace SportBids.Domain;

public class Prediction : Entity<Guid>
{
    public int Points { get; set; }
    public Score Score { get; set; }

    public Guid MatchId { get; set; }
    public Match Match { get; set; }

    public Guid OwnerId { get; set; }
    public AppUser Owner { get; set; }
    public Guid CreatedById { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
}
