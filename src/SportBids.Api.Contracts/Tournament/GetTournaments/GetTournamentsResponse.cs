#nullable disable
namespace SportBids.Api.Contracts.Tournament.GetTournaments;

public class GetTournamentsResponse
{
    public IEnumerable<GetTournamentsTournamentDto> Tournaments { get; set; }
}
