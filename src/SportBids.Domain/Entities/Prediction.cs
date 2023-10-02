using SportBids.Domain.Entities;
namespace SportBids.Domain;

public class Prediction
{
    public int Points { get; set; }
    public Score Score { get; set; }

    public Guid MatchId { get; set; }
    public Match Match { get; set; }

    public Guid OwnerId { get; set; }
    public AppUser Owner { get; set; }
    // public Guid CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
