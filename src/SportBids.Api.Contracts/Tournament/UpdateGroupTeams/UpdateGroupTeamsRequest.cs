namespace SportBids.Api.Contracts.Tournament.UpdateGroupTeams;

public class UpdateGroupTeamsRequest
{
    public Guid TournamentId { get; set; }
    public Guid GroupId { get; set; }
    public List<Guid> TeamIds { get; set; } = null!;
}
