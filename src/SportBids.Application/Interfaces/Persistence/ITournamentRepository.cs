using SportBids.Domain;

namespace SportBids.Application.Interfaces.Persistence;

public interface ITournamentRepository : IRepositoryBase<Tournament, Guid>
{
    Task<Tournament?> GetTournamentWithGroupsMatchesAndTeamsAsync(Guid tournamentId, CancellationToken cancellationToken);
    Task<Tournament?> GetTournamentMatchesAndTeamsAsync(Guid tournamentId, CancellationToken cancellationToken);
    Task<Tournament?> GetTournamentFullInfo(Guid tournamentId, CancellationToken cancellationToken);
}
