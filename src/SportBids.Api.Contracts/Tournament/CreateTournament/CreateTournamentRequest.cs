#nullable disable

namespace SportBids.Api.Contracts.Tournament.CreateTournament;

public class CreateTournamentRequest
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime FinishAt { get; set; }
    public int NumberOfTeams { get; set; }
    public int NumberOfGroups { get; set; }
}
