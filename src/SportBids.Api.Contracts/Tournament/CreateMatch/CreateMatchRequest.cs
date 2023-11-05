namespace SportBids.Api.Contracts.Tournament.CreateMatch;

public class CreateMatchRequest
{
    public Guid TournamentId { get; set; }
    public Guid Team1Id { get; set; }
    public Guid Team2Id { get; set; }
    public DateTimeOffset StartAt { get; set; }
}
