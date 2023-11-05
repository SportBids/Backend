using SportBids.Api.Contracts.Tournament.GetTournament;

namespace SportBids.Api.Contracts.Tournament.UpdateMatch;

public class UpdateMatchRequest
{
    public Guid TournamentId { get; set; }
    public Guid MatchId { get; set; }
    public Guid Team1Id { get; set; }
    public Guid Team2Id { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public bool Finished { get; set; }
    public ScoreDto Score { get; set; }
}
