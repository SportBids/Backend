#nullable disable
using SportBids.Api.Contracts.Tournament.GetTournament;

namespace SportBids.Api.Contracts.Tournament.GetTournaments;

public class GetTournamentsResponse
{
    public IEnumerable<TournamentDetailsDto> Tournaments { get; set; }
}
