namespace SportBids.Api.Contracts.Tournament.DeleteKnockoutMatch;

public class DeleteKnockOutMatchRequest
{
    public Guid TournamentId { get; set; }
    public Guid MatchId { get; set; }
}
