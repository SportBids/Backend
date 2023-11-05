using SportBids.Domain;

namespace SportBids.Application.Interfaces.Persistence;

public interface ITeamRepository : IRepositoryBase<Team, Guid>
{
    Task<IEnumerable<Team>> GetTeamsAsync(Guid tournamentId, IEnumerable<Guid> teamIds, CancellationToken cancellationToken);
    Task<Team?> GetTeamAsync(Guid tournamentId, Guid teamId, CancellationToken cancellationToken);
}
