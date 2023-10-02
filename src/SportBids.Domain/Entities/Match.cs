namespace SportBids.Domain;

public class Match : Entity<Guid>
{
    public Team Team1 { get; set; }
    public Team Team2 { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public bool Finished { get; set; }
    public Score Score { get; set; }
    public ICollection<Prediction> Predictions { get; set; }
}

// public class GroupMatch : Match
// {

// }

// public class KnockOutMatch : Match
// {

// }
