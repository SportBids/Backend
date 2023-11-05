namespace SportBids.Api.Contracts.Tournament.UpdateTeam;

public class UpdateTeamRequest
{
    public Guid TournamentId { get; set; }
    public Guid TeamId { get; set; }
    public string Name { get; set; } = null!;
    public string? Logo { get; set; }
    public bool IsKnockedOut { get; set; }
}
